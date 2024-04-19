/* --------------------------------------------------------------------------------
# Created by: Fabian Ramelsberger
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fusion;
using Fusion.Addons.ConnectionManagerAddon;
using Fusion.XR.Shared;
using UnityEngine;
using Random = UnityEngine.Random;


//<summary>
//CubeManagerScript description
//	</summary>
public class CubeManagerScript : NetworkBehaviour
{
    [Serializable]
    //TODO own class
    public class Player
    {
        public int PlayerIndex;
        public Material playerMaterial;
        //TODO call update materials in seter
        public List<NetworkHandColliderGrabbableCube> PlayerCubes;

        public void UpdatePlayerCubesMaterials()
        {
            PlayerCubes.ForEach(cube =>
            {
                cube._cubeMeshRenderer.sharedMaterial = playerMaterial;
            });
        }
    }

    public static CubeManagerScript Instance;
    
    [SerializeField] private List<Player> _playerList;
    [SerializeField] private ConnectionManager _connectionManager;

    public List<Player> PlayerList
    {
        get => _playerList;
        set => _playerList = value;
    }

     public void Awake()
    {
        if (Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        
    }

     // is called on Network Event Component in the Connection Manager GameObject
    public async void PlayerLeftDistributeCubes(NetworkRunner runner, PlayerRef playerRef)
    {
        Debug.Log("Playerleft!");
        Player player = GetPlayerWithId(playerRef.PlayerId);
        int ObjectIdStayedBehind = Random.Range(0, player.PlayerCubes.Count);
        for (int i = 0; i < player.PlayerCubes.Count; i++)
        {
            player.PlayerCubes[i].Object.RequestStateAuthority();
            await player.PlayerCubes[i].Object.EnsureHasStateAuthority();

            if (ObjectIdStayedBehind == i)
            {
                Player localPlayer = GetPlayerWithId(runner.LocalPlayer.PlayerId);
                localPlayer.PlayerCubes.Add(player.PlayerCubes[i]);
                localPlayer.UpdatePlayerCubesMaterials();
                Debug.Log(player.PlayerCubes[i].gameObject.name + "was assigned to player");
            }
            else
            {
                runner.Despawn(player.PlayerCubes[i].Object);
            }
        }

        player.PlayerCubes = new List<NetworkHandColliderGrabbableCube>();
    }


    public Material GetPlayerMaterial(int playerId)
    {
        var player = GetPlayerWithId(playerId);
        return player.playerMaterial;
    }

    private Player GetPlayerWithId(int playerId)
    {
        Player player =  PlayerList.Find(player => player.PlayerIndex == playerId);
        if (player == null)
        {
            Debug.LogError($"Player with {playerId} could not be found.");
        }

        return player;
    }

    [Rpc] 
    public void RPC_AddCubeToPlayer(int playerId, NetworkHandColliderGrabbableCube networkHandColliderGrabbableCube)
    {
        var player = GetPlayerWithId(playerId);
        if (!player.PlayerCubes.Contains(networkHandColliderGrabbableCube)) {
            player.PlayerCubes.Add(networkHandColliderGrabbableCube);
        }

        player.UpdatePlayerCubesMaterials();
    }
}

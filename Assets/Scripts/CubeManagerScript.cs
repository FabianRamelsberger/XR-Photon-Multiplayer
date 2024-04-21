/* --------------------------------------------------------------------------------
# Created by: Fabian Ramelsberger
# Created Date: 2024
# --------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.Threading.Tasks;
using Fusion;
using Fusion.Addons.ConnectionManagerAddon;
using Fusion.XR.Shared;
using Fusion.XR.Shared.Rig;
using UnityEngine;
using Random = UnityEngine.Random;


//<summary>
//CubeManagerScript description
//	</summary>
public class CubeManagerScript : NetworkBehaviour
{
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

    public void TeleportToStartPosition(NetworkRig networkRig, int playerId)
    {
        networkRig.HardwareRig.Teleport(GetPlayerWithId
            (playerId).PlayerSpawnPoint.position);
        networkRig.HardwareRig.Rotate(GetPlayerWithId
            (playerId).PlayerSpawnPoint.eulerAngles.y);
    }

     // is called on Network Event Component in the Connection Manager GameObject
    public void PlayerLeftDistributeCubes(NetworkRunner runner, PlayerRef playerRef)
    {
        Player player = GetPlayerWithId(playerRef.PlayerId);
        int ObjectIdStayedBehind = Random.Range(0, player.PlayerCubes.Count);
        for (int i = 0; i < player.PlayerCubes.Count; i++)
        {
            player.PlayerCubes[i].Object.RequestStateAuthority();

            if (ObjectIdStayedBehind == i)
            {
                Player localPlayer = GetPlayerWithId(runner.LocalPlayer.PlayerId);
                localPlayer.PlayerCubes.Add(player.PlayerCubes[i]);
                localPlayer.UpdatePlayerCubesMaterials();
            }
            else
            {
                WaitUntilHasAuthorityAndDespawn(runner, player.PlayerCubes[i].Object, playerRef);
            }
        }

        player.PlayerCubes = new List<NetworkHandColliderGrabbableCube>();
    }

    private async void WaitUntilHasAuthorityAndDespawn(NetworkRunner runner, NetworkObject networkObject,
        PlayerRef playerRef)
    {
        await networkObject.EnsureHasStateAuthority(playerRef);
        runner.Despawn(networkObject);
    }

    public Material GetPlayerMaterial(int playerId)
    {
        var player = GetPlayerWithId(playerId);
        return player.playerMaterial;
    }

    public Player GetPlayerWithId(int playerId)
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

/* --------------------------------------------------------------------------------
# Created by: Fabian Ramelsberger
# Created Date: 2024
# --------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Fusion.Addons.ConnectionManagerAddon;
using Fusion.XR.Shared;
using Fusion.XR.Shared.Rig;
using FusionHelpers;
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
    
    public struct NetworkStructExample : INetworkStruct
    {
        [Networked, Capacity(16)]
        public NetworkDictionary<PlayerRef, int> DictOfInt => default;
    }

    [Networked] public ref NetworkStructExample NetworkedStructRef => ref MakeRef<NetworkStructExample>();
    [Networked] public bool HasSharedCubeSpawned { get; set; }
    [SerializeField] private NetworkHandColliderGrabbableCube _sharedCubePrefab;
    [SerializeField] private Transform _sharedCubeSpawnPoint;
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

    public override void Spawned()
    {
        Debug.Log($"Spawned Network Session for Runner: {Runner}");
        Runner.RegisterSingleton(this);

        SpawnSharedCube();
    }

    private void SpawnSharedCube()
    {
        if (HasSharedCubeSpawned == false)
        {
            HasSharedCubeSpawned = true;
            Runner.Spawn(_sharedCubePrefab, _sharedCubeSpawnPoint.position, _sharedCubeSpawnPoint.rotation,
                PlayerRef.None, InitializeObjBeforeSpawn);
        }
    }

    private void InitializeObjBeforeSpawn(NetworkRunner runner, NetworkObject obj)
    {
    }

    public void TeleportToStartPosition(NetworkRig networkRig, PlayerRef playerRef)
    {
        networkRig.HardwareRig.Teleport(GetPlayerWithId
            (playerRef).PlayerSpawnPoint.position);
        networkRig.HardwareRig.Rotate(GetPlayerWithId
            (playerRef).PlayerSpawnPoint.eulerAngles.y);
    }

     // is called on Network Event Component in the Connection Manager GameObject
    public void PlayerLeftDistributeCubes(NetworkRunner runner, PlayerRef playerRef)
    {
        Player player = GetPlayerWithId(playerRef);
        int ObjectIdStayedBehind = Random.Range(0, player.PlayerCubes.Count);
        for (int i = 0; i < player.PlayerCubes.Count; i++)
        {
            player.PlayerCubes[i].Object.RequestStateAuthority();

            if (ObjectIdStayedBehind == i)
            {
                Player localPlayer = GetPlayerWithId(runner.LocalPlayer);
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

    public Material GetPlayerMaterial(PlayerRef playerRef)
    {
        var player = GetPlayerWithId(playerRef);
        return player.playerMaterial;
    }

    public Player GetPlayerWithId(PlayerRef playerRef)
    {
        Player player =  PlayerList.Find(player => player.playerRef == playerRef);
        if (player == null)
        {
            Debug.LogError($"Player with {playerRef} could not be found.");
        }

        return player;
    }

    [Rpc] 
    public void RPC_AddCubeToPlayer(PlayerRef playerRef, NetworkHandColliderGrabbableCube networkHandColliderGrabbableCube)
    {
        var player = GetPlayerWithId(playerRef);
        if (!player.PlayerCubes.Contains(networkHandColliderGrabbableCube)) {
            player.PlayerCubes.Add(networkHandColliderGrabbableCube);
        }

        player.UpdatePlayerCubesMaterials();
    }

    public void SetPlayerToFreeSpot(PlayerRef player)
    {
        int assignedPlace = 0;
        if (NetworkedStructRef.DictOfInt.TryGet(player, out int value))
        {
            if(value != 0) //because the default value is 0, we start with 1
            {
                assignedPlace = NetworkedStructRef.DictOfInt[player];
            }
        }
        
        if (assignedPlace != 0)
        {
            _playerList[assignedPlace].playerRef = player;
            _playerList[assignedPlace].DebugPlayerRef = player.ToString();
        }
        else
        {
            PlayerRef nonePlayer = PlayerRef.None;
            Player freePlayer =AssignPlayerToList(player, nonePlayer);
            if (freePlayer != null)
            {
                 int index = _playerList.IndexOf(freePlayer);
                 NetworkedStructRef.DictOfInt.Set(player, index);
            }
           
            
        }
    }
    public void RemovePlayer(NetworkRunner runner, PlayerRef player)
    {
        PlayerRef nonePlayer = PlayerRef.None;
        Player playerToBeRemoved =AssignPlayerToList(nonePlayer, player);
        if (playerToBeRemoved != null)
        {
            NetworkedStructRef.DictOfInt.Remove(player);
        }
    }

    private Player AssignPlayerToList(PlayerRef newPlayerRef, PlayerRef toBeReplacedPlayerRef)
    {
        Player freePlayer = _playerList.FirstOrDefault(
            playerCollection => playerCollection.playerRef == toBeReplacedPlayerRef);

        if (freePlayer != null)
        {
            freePlayer.playerRef = newPlayerRef;
            freePlayer.DebugPlayerRef = newPlayerRef.ToString();
            return freePlayer;
        }
        return null;
    }
}

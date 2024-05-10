/* --------------------------------------------------------------------------------
# Created by: Fabian Ramelsberger
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Fusion.XR.Shared;
using Fusion.XR.Shared.Rig;
using FusionHelpers;
using UnityEngine;
using Random = UnityEngine.Random;

//<summary>
//The PlayerManagerScript manages player instances and interactions in a networked game environment.
//It handles player properties, such as spawn positions and cube ownership, and manages a shared,
//grabbable cube that players can interact with once spawned.
//The class also facilitates teleportation of players to start positions, distributes cubes when players leave,
//and dynamically updates player positions within a networked list structure.
//Additionally, it supports RPCs to add cubes to players and manages the lifecycle of player references
//and cube objects within the game.+
//</summary>

//	</summary>
public class PlayerManagerScript : NetworkBehaviour
{
    public static PlayerManagerScript Instance;
    
    [Header("PlayerList with properties")]
    [Tooltip("List of player properties, like colours, grabbed/owned cubes, " +
             "spawn positions. Networked is only the PlayerRef and there place at PlayerRefPlaceInPlayerListNetworkStruct")]
    [SerializeField] private List<Player> _playerList;

    [Header("Shared Cube")]
    [SerializeField] private Transform _sharedCubeSpawnPoint;
   
    [Networked] private bool _hasSharedCubeSpawned { get; set; }
    [Tooltip("The shared cube can only be spawned once." +
             "It is initialized as PlayerRef.None -> so every Player can grab it." +
             "Only after one Player grabbed it could he hold it.")]
    [SerializeField] private NetworkHandColliderGrabbableCube _sharedCubePrefab;
    [Header("References")]
    [SerializeField] private ConnectionManager _connectionManager;

    private struct PlayerRefPlaceInPlayerListNetworkStruct : INetworkStruct
    {
        [Networked, Capacity(16)]
        public NetworkDictionary<PlayerRef, int> DictOfPlaces => default;
    }
    [Networked] private ref PlayerRefPlaceInPlayerListNetworkStruct PlayerPlaceStructRef => 
        ref MakeRef<PlayerRefPlaceInPlayerListNetworkStruct>();

    #region UnityFunctions

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
    #endregion

    public override void Spawned()
    {
        Runner.RegisterSingleton(this);
        SpawnSharedCube();
    }

    private void SpawnSharedCube()
    {
        if (_hasSharedCubeSpawned == false)
        {
            _hasSharedCubeSpawned = true;
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
        int objectIdStayedBehind = Random.Range(0, player.PlayerCubeList.Count);
        for (int i = 0; i < player.PlayerCubeList.Count; i++)
        {
            player.PlayerCubeList[i].Object.RequestStateAuthority();

            if (objectIdStayedBehind == i)
            {
                Player localPlayer = GetPlayerWithId(runner.LocalPlayer);
                localPlayer.PlayerCubeList.Add(player.PlayerCubeList[i]);
                localPlayer.UpdatePlayerCubesMaterials();
            }
            else
            {
                WaitUntilHasAuthorityAndDespawn(runner, player.PlayerCubeList[i].Object, playerRef);
            }
        }
        
        player.PlayerCubeList.Clear();
    }
    
    // is called on Network Event Component in the Connection Manager GameObject
    public void PlayerLeftClearTickTackToe(NetworkRunner runner, PlayerRef playerRef)
    {
        Player pl = GetPlayerWithId(playerRef);
        
        Player player = GetPlayerWithId(playerRef);
        for (int i = 0; i < player.PlayerToeList.Count; i++)
        {
            player.PlayerToeList[i].Object.RequestStateAuthority();
            WaitUntilHasAuthorityAndDespawn(runner, player.PlayerToeList[i].Object, playerRef);
        }
        
        player.PlayerToeList.Clear();
    }

    public void DespawnNetworkObject(NetworkObject networkObject)
    {
        WaitUntilHasAuthorityAndDespawn(Runner, networkObject,
            networkObject.StateAuthority);
    }
    
    private async void WaitUntilHasAuthorityAndDespawn(NetworkRunner runner, NetworkObject networkObject,
        PlayerRef playerRef)
    {
        try {
                await networkObject.EnsureHasStateAuthority(playerRef);
                runner.Despawn(networkObject);
        }
        catch
        {
            // Sometimes players despawn objects before the host can act, as clients destroy their objects upon leaving
        }
    }

    public Material GetPlayerMaterial(PlayerRef playerRef)
    {
        var player = GetPlayerWithId(playerRef);
        return player.PlayerMaterial;
    }

    public Player GetPlayerWithId(PlayerRef playerRef)
    {
        Player player =  _playerList.Find(player => player.PlayerRef == playerRef);
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
        if (!player.PlayerCubeList.Contains(networkHandColliderGrabbableCube)) {
            if (networkHandColliderGrabbableCube.GetComponent<ToeCubeElement>())
            {
                player.PlayerToeList.Add(networkHandColliderGrabbableCube);
            }
            else
            {
                player.PlayerCubeList.Add(networkHandColliderGrabbableCube);
            }
        }

        player.UpdatePlayerCubesMaterials();
    }

    public void SetPlayerToFreeSpot(PlayerRef player)
    {
        int assignedPlace = 0;
        if (PlayerPlaceStructRef.DictOfPlaces.TryGet(player, out int value))
        {
            if(value != 0) //because the default value is 0, we start with 1
            {
                assignedPlace = PlayerPlaceStructRef.DictOfPlaces[player];
            }
        }
        
        if (assignedPlace != 0)
        {
            _playerList[assignedPlace].SetPlayerRef(player);
        }
        else
        {
            PlayerRef nonePlayer = PlayerRef.None;
            Player freePlayer =AssignPlayerToList(player, nonePlayer);
            if (freePlayer != null)
            {
                int index = _playerList.IndexOf(freePlayer);
                PlayerPlaceStructRef.DictOfPlaces.Set(player, index);
            }
        }
    }
    public void RemovePlayer(NetworkRunner runner, PlayerRef player)
    {
        PlayerRef nonePlayer = PlayerRef.None;
        Player playerToBeRemoved =AssignPlayerToList(nonePlayer, player);
        if (playerToBeRemoved != null)
        {
            PlayerPlaceStructRef.DictOfPlaces.Remove(player);
        }
    }

    private Player AssignPlayerToList(PlayerRef newPlayerRef, PlayerRef toBeReplacedPlayerRef)
    {
        Player freePlayer = _playerList.FirstOrDefault(
            playerCollection => playerCollection.PlayerRef == toBeReplacedPlayerRef);

        if (freePlayer != null)
        {
            freePlayer.SetPlayerRef(newPlayerRef);
            return freePlayer;
        }
        return null;
    }
}

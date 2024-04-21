using System.Collections.Generic;
using Fusion;
using Fusion.Addons.ConnectionManagerAddon;
using Fusion.XR.Shared.Grabbing.NetworkHandColliderBased;
using UnityEngine;
using Random = UnityEngine.Random;

public class CubeSpawnerScript : MonoBehaviour
{
    [SerializeField] private List<NetworkHandColliderGrabbable> _playerCubeToSpawnList;
    [Range(1,6)]
    [SerializeField] private int _amountOfCubesPerPlayerToSpawn;
    [SerializeField] private List<NetworkHandColliderGrabbable> _independentCubeToSpawnList;
    
    [SerializeField] private ConnectionManager _connectionManager;
    [SerializeField] private CubeManagerScript _cubeManagerScript;
    
    public delegate void OnBeforeSpawned(NetworkRunner runner, NetworkObject obj);

    private void Awake()
    {
        if (_connectionManager == null)
        {
            Debug.LogError($"Please assign the ConnectionManager to the CubeSpawnerScript in {gameObject.name}");
        }
        _connectionManager.OnPlayerJoinedAction += SpawnPlayerDependingPrefabs;
        _connectionManager.OnPlayerLeftAction += RemovePlayerDependingPrefabs;
    }

    private void RemovePlayerDependingPrefabs(PlayerRef playerRef)
    {
       
    }

    private void SpawnPlayerDependingPrefabs(PlayerRef playerRef)
    {
        Player player = _cubeManagerScript.GetPlayerWithId(playerRef);
        List<Transform> cubeSpawnPoints = player.CubeSpawnPoints;
        for (int i = 0; i < _amountOfCubesPerPlayerToSpawn; i++)
        {
             
            if (cubeSpawnPoints.Count < i) return;
            NetworkHandColliderGrabbable randomPlayerCube = GetRandomCube();
        
            _connectionManager.runner.Spawn(
                randomPlayerCube, cubeSpawnPoints[i].position, Quaternion.identity, playerRef, InitializeObjBeforeSpawn);
            Debug.Log("Spawn cube");
        }
    }

    private NetworkHandColliderGrabbable GetRandomCube()
    {
        return _playerCubeToSpawnList[Random.Range(0, _playerCubeToSpawnList.Count)];
    }

   
    private void InitializeObjBeforeSpawn(NetworkRunner runner, NetworkObject obj)
    {
     
    }
}

/* --------------------------------------------------------------------------------
# Created by: Fabian Ramelsberger
# Created Date: 2024
# --------------------------------------------------------------------------------*/
using System.Collections.Generic;
using Fusion;
using Fusion.Addons.ConnectionManagerAddon;
using Fusion.XR.Shared.Grabbing.NetworkHandColliderBased;
using UnityEngine;
using Random = UnityEngine.Random;

//<summary>
//The PlayerCubeSpawner is responsible for spawning cubes specific to each player in a networked game.
//It uses a list of NetworkHandColliderGrabbable prefabs to spawn a predetermined number
//of cubes for each player at designated spawn points.
//	</summary>
[RequireComponent(typeof(PlayerManagerScript))]
public class PlayerCubeSpawner : MonoBehaviour
{
    [SerializeField] private List<NetworkHandColliderGrabbable> _playerCubeToSpawnList;
    [Range(1,6)]
    [SerializeField] private int _amountOfCubesPerPlayerToSpawn;
    [SerializeField] private ConnectionManager _connectionManager;
    private PlayerManagerScript _playerManagerScript;
    public delegate void OnBeforeSpawned(NetworkRunner runner, NetworkObject obj);

    private void Awake()
    {
        if (_connectionManager == null)
        {
            Debug.LogError($"Please assign the ConnectionManager to the CubeSpawnerScript in {gameObject.name}");
        }
        _connectionManager.OnPlayerJoinedAction += SpawnPlayerDependingPrefabs;
        _playerManagerScript = GetComponent<PlayerManagerScript>();
    }

    private void SpawnPlayerDependingPrefabs(PlayerRef playerRef)
    {
        Player player = _playerManagerScript.GetPlayerWithId(playerRef);
        List<Transform> cubeSpawnPoints = player.CubeSpawnPoints;
        for (int i = 0; i < _amountOfCubesPerPlayerToSpawn; i++)
        {
            if (cubeSpawnPoints.Count < i) return;
            NetworkHandColliderGrabbable randomPlayerCube = GetRandomCube();
            _connectionManager.runner.Spawn(
                randomPlayerCube, cubeSpawnPoints[i].position, Quaternion.identity, playerRef, InitializeObjBeforeSpawn);
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
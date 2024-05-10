/* --------------------------------------------------------------------------------
# Created by: Fabian Ramelsberger
# Created Date: 2024
# --------------------------------------------------------------------------------*/
using System.Collections.Generic;
using Fusion;
using Fusion.XR.Shared.Grabbing.NetworkHandColliderBased;
using UnityEngine;
using Random = UnityEngine.Random;

//<summary>
//The PlayerPropertySpawner is responsible for spawning cubes, and play material (Tick Tack Toe)
//specific to each player in a networked game.
//It uses a list of NetworkHandColliderGrabbable prefabs to spawn a predetermined number
//of cubes for each player at designated spawn points.
//	</summary>
[RequireComponent(typeof(PlayerManagerScript))]
public class PlayerPropertySpawner : MonoBehaviour
{
    [SerializeField] private List<NetworkHandColliderGrabbable> _playerCubeToSpawnList;
    [Range(1,6)]
    [SerializeField] private int _amountOfCubesPerPlayerToSpawn;
    [SerializeField] private ConnectionManager _connectionManager;
    
     [Header("Tick tack toe")]
     [Range(1,6)]
     [SerializeField] private int _amountOfTickTackToesPerPlayerToSpawn;

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
            _connectionManager.Runner.Spawn(
                randomPlayerCube, cubeSpawnPoints[i].position, Quaternion.identity, playerRef, InitializeObjBeforeSpawn);
        }

        SpawnTickTackToeElements(playerRef, player);
    }

    private void SpawnTickTackToeElements(PlayerRef playerRef, Player player)
    {
        NetworkHandColliderGrabbable grabbable = null;
        grabbable = player.TickTackToeGameObject;
        //Currently there are no more than two players allowed.
        if (grabbable == null)
        {
            return;
        }
        
        List<Transform> toeSpawnPoints = player.ToeSpawnPoints;
        for (int i = 0; i < _amountOfTickTackToesPerPlayerToSpawn; i++)
        {
            _connectionManager.Runner.Spawn(
                grabbable, toeSpawnPoints[i].position, Quaternion.identity, playerRef, InitializeObjBeforeSpawn);
            if (toeSpawnPoints.Count < i) return;
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

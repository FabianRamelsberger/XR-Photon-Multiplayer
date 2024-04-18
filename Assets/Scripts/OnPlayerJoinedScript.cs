using Fusion;
using Fusion.Addons.ConnectionManagerAddon;
using Fusion.XR.Shared.Grabbing.NetworkHandColliderBased;
using UnityEngine;

[RequireComponent(typeof(ConnectionManager))]
public class OnPlayerJoined : MonoBehaviour
{
    [SerializeField] private NetworkHandColliderGrabbable _playerCube;
    [SerializeField] private Transform _spawnTransform;
    private ConnectionManager _connectionManager;
    public delegate void OnBeforeSpawned(NetworkRunner runner, NetworkObject obj);

    private void Awake()
    {
        _connectionManager = GetComponent<ConnectionManager>();
        _connectionManager.OnPlayerJoinedAction += SpawnPlayerDependingPrefabs;
    }

    private void SpawnPlayerDependingPrefabs(PlayerRef player)
    {
        _connectionManager.runner.Spawn(_playerCube, _spawnTransform.position, Quaternion.identity, player, InitializeObjBeforeSpawn);
    }
    
    private void InitializeObjBeforeSpawn(NetworkRunner runner, NetworkObject obj)
    {
     
    }
}

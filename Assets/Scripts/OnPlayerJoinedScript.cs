using Fusion.Addons.ConnectionManagerAddon;
using Fusion.XR.Shared.Grabbing.NetworkHandColliderBased;
using UnityEngine;

[RequireComponent(typeof(ConnectionManager))]
public class OnPlayerJoined : MonoBehaviour
{
    [SerializeField] private NetworkHandColliderGrabbable _playerCube;
    [SerializeField] private Transform _spawnTransform;
    private ConnectionManager _connectionManager;
    
    private void Awake()
    {
        _connectionManager = GetComponent<ConnectionManager>();
        _connectionManager.OnPlayerJoinedAction += SpawnPlayerDependingPrefabs;
    }

    private void SpawnPlayerDependingPrefabs(int playerId, bool isLocalPlayer)
    {
        Instantiate(_playerCube,_spawnTransform);
    }
}

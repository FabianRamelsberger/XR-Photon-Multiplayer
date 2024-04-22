/* --------------------------------------------------------------------------------
# Created by: Fabian Ramelsberger
# Created Date: 2024
# --------------------------------------------------------------------------------*/
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fusion;
using Fusion.XR.Shared.Rig;
using FusionHelpers;
using UnityEngine;
using UnityEngine.SceneManagement;

//<summary>
// Handles:
// - connection launch (either with room name or matchmaking session properties)
//- user representation spawn on connection
//</summary>

public class ConnectionManager : MonoBehaviour, INetworkRunnerCallbacks
{
    [Flags]
    public enum ConnectionCriterias
    {
        RoomName = 1,
        SessionProperties = 2
    }

    [Header("Room configuration")] private GameMode gameMode = GameMode.Shared;
    [SerializeField] private string roomName = "";
    bool connectOnStart = false;

    [Tooltip(
        "Set it to 0 to use the DefaultPlayers value, from the Global NetworkProjectConfig (simulation section)")]
    [SerializeField]
    private int playerCount = 0;

    [Header("Room selection criteria")] private ConnectionCriterias connectionCriterias = ConnectionCriterias.RoomName;

    [Header("Fusion settings")] [Tooltip("Fusion runner. Automatically created")] [SerializeField, ReadOnly]
    private NetworkRunner _runner;

    public NetworkRunner Runner => _runner;
    private INetworkSceneManager sceneManager;

    [Header("Local user spawner")] [SerializeField]
    private NetworkObject userPrefab;

    [SerializeField] private List<Transform> _playerSpawnTransformList;

    // Dictionary of spawned user prefabs, to store them on the server for host topology, and destroy them on disconnection (for shared topology, use Network Objects's "Destroy When State Authority Leaves" option)
    public Action<PlayerRef> OnPlayerJoinedAction;

    bool ShouldConnectWithRoomName => (connectionCriterias & ConnectionCriterias.RoomName) != 0;
    bool ShouldConnectWithSessionProperties => (connectionCriterias & ConnectionCriterias.SessionProperties) != 0;

    private void Awake()
    {
        CheckRunner();
    }

    private void CheckRunner()
    {
        // Check if a runner exist on the same game object
        if (_runner == null) _runner = GetComponent<NetworkRunner>();

        // Create the Fusion runner and let it know that we will be providing user input
        if (_runner == null) _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;
    }

    private async void Start()
    {
        // Launch the connection at start
        if (connectOnStart) await Connect();
    }

    // we launch via a button
    public async void OnConnectExternally()
    {
        CheckRunner();
        await Connect();
    }

    // disconnect via button
    public void OnDisconnectExternally()
    {
        _runner.Shutdown(false, ShutdownReason.Ok);
        SceneManager.LoadScene(0);
    }

    Dictionary<string, SessionProperty> AllConnectionSessionProperties
    {
        get
        {
            var propDict = new Dictionary<string, SessionProperty>();
            return propDict;
        }
    }

    protected virtual NetworkSceneInfo CurrentSceneInfo()
    {
        var activeScene = SceneManager.GetActiveScene();
        SceneRef sceneRef = default;
        if (activeScene.buildIndex < 0 || activeScene.buildIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError("Current scene is not part of the build settings");
        }
        else
        {
            sceneRef = SceneRef.FromIndex(activeScene.buildIndex);
        }

        var sceneInfo = new NetworkSceneInfo();
        if (sceneRef.IsValid)
        {
            sceneInfo.AddSceneRef(sceneRef);
        }

        return sceneInfo;
    }

    public async Task Connect()
    {
        // Create the scene manager if it does not exist
        if (sceneManager == null) sceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>();

        // Start or join (depends on gamemode) a session with a specific name
        var args = new StartGameArgs()
        {
            GameMode = gameMode,
            Scene = CurrentSceneInfo(),
            SceneManager = sceneManager
        };
        // Connection criteria
        if (ShouldConnectWithRoomName)
        {
            args.SessionName = roomName;
        }

        if (ShouldConnectWithSessionProperties)
        {
            args.SessionProperties = AllConnectionSessionProperties;
        }

        // Room details
        if (playerCount > 0)
        {
            args.PlayerCount = playerCount;
        }

        await _runner.StartGame(args);

        string prop = "";
        if (_runner.SessionInfo.Properties != null && _runner.SessionInfo.Properties.Count > 0)
        {
            prop = "SessionProperties: ";
            foreach (var p in _runner.SessionInfo.Properties) prop += $" ({p.Key}={p.Value.PropertyValue}) ";
        }

        Debug.Log($"Session info: Room name {_runner.SessionInfo.Name}. Region: {_runner.SessionInfo.Region}. {prop}");
        if ((connectionCriterias & ConnectionCriterias.RoomName) == 0)
        {
            roomName = _runner.SessionInfo.Name;
        }
    }

    #region Player spawn

    private void OnPlayerJoinedSharedMode(NetworkRunner runner, PlayerRef player)
    {
        if (player == runner.LocalPlayer && userPrefab != null)
        {
            // Spawn the user prefab for the local user
            NetworkObject networkPlayerObject = runner.Spawn(userPrefab, Vector3.zero, Quaternion.identity, player,
                (runner, obj) => { });
            runner.WaitForSingleton<PlayerManagerScript>(
                cubeManager =>
                {
                    cubeManager.SetPlayerToFreeSpot(player);
                    cubeManager.TeleportToStartPosition(
                        networkPlayerObject.GetComponent<NetworkRig>(), player);
                    OnPlayerJoinedAction?.Invoke(player);
                });
        }
        else
        {
            runner.WaitForSingleton<PlayerManagerScript>(
                cubeManager => { cubeManager.SetPlayerToFreeSpot(player); });
        }
    }

    #endregion

    #region INetworkRunnerCallbacks

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        OnPlayerJoinedSharedMode(runner, player);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
    }

    #endregion

    #region INetworkRunnerCallbacks (debug log only)

    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("OnConnectedToServer");
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.Log("Shutdown: " + shutdownReason);
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        Debug.Log("OnDisconnectedFromServer: " + reason);
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.Log("OnConnectFailed: " + reason);
    }

    #endregion

    #region Unused INetworkRunnerCallbacks

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }

    #endregion
}
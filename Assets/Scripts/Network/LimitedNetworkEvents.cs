using System;
using System.Collections.Generic;
using FREngine.Events;
using Fusion;
using Fusion.Sockets;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Network
{
    public class LimitedNetworkEvents : MonoBehaviour, INetworkRunnerCallbacks
    {
        [InfoBox("These are a limited version of the network events from Fusion - Network Events")]
        
        [SerializeReference, SerializeField] private List<IEvent> _connectedEvents = new();
        [SerializeReference, SerializeField] private List<INetworkDisconnectEvent> _disconnectedEvents = new();
        [SerializeReference, SerializeField] private List<INetworkPlayerEvent> _playerLeftEvents = new();
        [SerializeReference, SerializeField] private List<IEvent> _onShutDownEvents = new();

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            Utility.Emit(transform, _onShutDownEvents);
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
            Utility.Emit(transform, _connectedEvents);
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            NetworkUtility.Emit(transform, _playerLeftEvents, runner, player);
        }

        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
        {
            NetworkUtility.Emit(transform, _disconnectedEvents, runner, reason);
        }

        #region notUsed methods

        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }

        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request,
            byte[] token)
        {
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
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

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key,
            ArraySegment<byte> data)
        {
        }

        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
        {
        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
        }

        #endregion
    }
}
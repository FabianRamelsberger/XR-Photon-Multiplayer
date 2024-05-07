using Fusion;
using Fusion.Sockets;
using UnityEngine;

namespace Network
{
    public interface INetworkDisconnectEvent
    {
        public void Execute(Transform emitter, NetworkRunner runner, NetDisconnectReason reason);
    }
}
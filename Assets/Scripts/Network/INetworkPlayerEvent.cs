using Fusion;
using UnityEngine;

namespace Network
{
    public interface INetworkPlayerEvent
    {
        public void Execute(Transform emitter, NetworkRunner runner, PlayerRef player);
    }
}
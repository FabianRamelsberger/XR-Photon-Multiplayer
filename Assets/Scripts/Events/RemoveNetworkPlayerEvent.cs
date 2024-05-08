using Fusion;
using Network;
using UnityEngine;

namespace Gameplay
{
    public class RemoveNetworkPlayerEvent : INetworkPlayerEvent
    {
        public void Execute(Transform emitter, NetworkRunner runner, PlayerRef player)
        {
            PlayerManagerScript.Instance.RemovePlayer(runner, player);
        }
    }
}
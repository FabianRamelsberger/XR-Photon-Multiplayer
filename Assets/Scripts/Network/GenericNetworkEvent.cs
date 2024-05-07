using Fusion;
using Fusion.Sockets;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Network
{
    public interface GenericNetworkEvent
    {
        [System.Serializable, HideLabel]
        public class CustomDisconnectEvent : INetworkDisconnectEvent
        {
            [SerializeField, HideReferenceObjectPicker] private UnityEvent<NetworkRunner, NetDisconnectReason> _unityEvent = 
                new UnityEvent<NetworkRunner, NetDisconnectReason>();

            public void Execute(Transform emitter, NetworkRunner runner, NetDisconnectReason reason)
            {
                _unityEvent?.Invoke(runner, reason);
            }
        }
    }
}
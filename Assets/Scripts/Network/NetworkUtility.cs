using System.Collections.Generic;
using FREngine.Events;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

namespace Network
{
    public class NetworkUtility
    {
        public static void Emit(Transform emitter, List<INetworkDisconnectEvent> disconnectedEvents, NetworkRunner runner, NetDisconnectReason reason)
        {
            if (disconnectedEvents != null)
            {
                foreach (var e in disconnectedEvents)
                {
                    if (e != null)
                    {
                        e.Execute(emitter, runner, reason);
                    }
                    else
                    {
                        Debug.LogError("Event is null for " + emitter, emitter);
                    }
                }
            };
        }
        
        public static void Emit(
            Transform emitter, List<INetworkPlayerEvent> disconnectedEvents, NetworkRunner runner, PlayerRef player
        )
        {
            if (disconnectedEvents != null)
            {
                foreach (var e in disconnectedEvents)
                {
                    if (e != null)
                    {
                        e.Execute(emitter, runner, player);
                    }
                    else
                    {
                        Debug.LogError("Event is null for " + emitter, emitter);
                    }
                }
            };
        }
    }
}
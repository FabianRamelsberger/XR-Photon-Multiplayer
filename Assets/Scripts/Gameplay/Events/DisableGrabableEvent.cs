using FREngine.Events;
using Fusion.XR.Shared.Grabbing.NetworkHandColliderBased;
using UnityEngine;

public class DisableGrabableEvent : IEvent
{
    public void Execute(Transform emitter)
    {
        NetworkHandColliderGrabbable grabbable = emitter.GetComponent<NetworkHandColliderGrabbable>();
        if (grabbable != null)
        {
            grabbable.enabled = false;
        }
    }
}

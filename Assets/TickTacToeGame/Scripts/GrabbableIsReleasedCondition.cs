using System.Collections;
using System.Collections.Generic;
using FREngine.Events;
using Fusion.XR.Shared.Grabbing.NetworkHandColliderBased;
using UnityEngine;

public class GrabbableIsReleasedCondition : ICondition
{
    [SerializeField] private bool _isGrabbed = false;
    public bool Execute(Transform emitter)
    {
        var grabbable = emitter.GetComponentInParent<NetworkHandColliderGrabbable>();
        if (grabbable)
        {
            return grabbable.IsGrabbed == _isGrabbed;
        }
        return false;
    }
}

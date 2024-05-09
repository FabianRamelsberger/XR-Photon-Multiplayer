using System.Collections;
using System.Collections.Generic;
using FREngine.Events;
using Fusion;
using Fusion.XR.Shared.Grabbing.NetworkHandColliderBased;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabberIsLocalPlayerCondition : ICondition
{
    public bool Execute(Transform emitter)
    {
        var grabbable = emitter.GetComponentInParent<NetworkHandColliderGrabbable>();
        PlayerRef localPlayer = PlayerManagerScript.Instance.Runner.LocalPlayer;
        if (grabbable == null || localPlayer == null)
        {
           return false;
        }
        return localPlayer == grabbable.Object.StateAuthority;
    }
}

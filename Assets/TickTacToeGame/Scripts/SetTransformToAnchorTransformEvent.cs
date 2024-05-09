/* --------------------------------------------------------------------------------
# Script Name: FREngine Component
# Created by: Fabian Ramelsberger
# Part of: FREngine
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using Fusion.XR.Shared.Grabbing.NetworkHandColliderBased;
using UnityEngine;

namespace FREngine.Events
{
    public class SetTransformToAnchorTransformEvent : IEvent
    {
        [SerializeField] private Transform _anchorTransform;
        public void Execute(Transform emitter)
        {
            if (_anchorTransform == null)
            {
                Debug.LogError("Please assign a anchorTransform");
                return;
            }
            GameObject gameObject = emitter.GetComponentInParent<ToeCubeElement>().gameObject;
            gameObject.transform.parent = _anchorTransform;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.gameObject.transform.localRotation = new Quaternion(0,0,0,0);
            gameObject.transform.parent = null;
            gameObject.GetComponent<NetworkHandColliderGrabbable>().enabled = false;
        }
    }
}

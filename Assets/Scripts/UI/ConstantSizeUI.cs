/* --------------------------------------------------------------------------------
# Created by: Fabian Ramelsberger
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using System;
using Fusion.XR.Shared.Desktop;
using UnityEngine;

///<summary>
///ConstantSizedUI description
//	</summary>
using UnityEngine;

using UnityEngine;

public class ConstantSizeUI : MonoBehaviour
{
    [SerializeField] private RigSelection _rigSelection;
    private Camera _playerCamera;
    public float CameraDistance = 3.0F;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    private Transform target;

    private void Start()
    {
        if (_rigSelection.IsRigSelected)
        {
            OnUpdateCameraReference();
        }
        else
        {
            _rigSelection.onSelectRig.AddListener(OnUpdateCameraReference);
        }
    }

    private void OnUpdateCameraReference()
    {
        _playerCamera = _rigSelection.RigCamera;
    }
    
    void Update()
    {
        if (_playerCamera == null)
        {
            return;
        }
       
        Vector3 targetPosition = _playerCamera.transform.TransformPoint(new Vector3(0, 0, CameraDistance));
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        var lookAtPos = new Vector3(_playerCamera.transform.position.x, transform.position.y, _playerCamera.transform.position.z);
        transform.LookAt(lookAtPos);  
    }
}

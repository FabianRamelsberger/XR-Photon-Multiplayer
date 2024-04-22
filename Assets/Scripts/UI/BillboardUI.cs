/* --------------------------------------------------------------------------------
# Created by: Fabian Ramelsberger
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using System;
using UnityEngine;

//<summary>
//BillboardUI ensures that a UI element remains facing the player's camera,
//adjusting its position smoothly based on the camera's movements.
//It utilizes the RigSelection component to update the camera reference dynamically.
//</summary>

public class BillboardUI : MonoBehaviour
{
    private RigSelection _rigSelection;
    [Header("Settings")]
    [SerializeField] private float _cameraDistance = 3.0F;
    [SerializeField] private float _smoothTime = 0.3F;
    [SerializeField] private bool rotateTowardsCamera = true;
    [SerializeField] private bool moveTowardsCamera = true;
    
    private Vector3 _velocity = Vector3.zero;
    private Transform _target;
    private Camera _playerCamera;

    private void Awake()
    {
        _rigSelection = UIManager.Instance.RigSelection;
    }

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

        if (moveTowardsCamera)
        {
            Vector3 targetPosition = _playerCamera.transform.TransformPoint(new Vector3(0, 0, _cameraDistance));
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, _smoothTime);
        }

        if (rotateTowardsCamera)
        {
            var lookAtPos = new Vector3(_playerCamera.transform.position.x, transform.position.y,
                _playerCamera.transform.position.z);
            transform.LookAt(lookAtPos);
        }
    }
}

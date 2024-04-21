/* --------------------------------------------------------------------------------
# Created by: Fabian Ramelsberger
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using UnityEngine;

///<summary>
///ConstantSizedUI description
//	</summary>
using UnityEngine;

using UnityEngine;

public class ConstantSizeUI : MonoBehaviour
{
    public Camera playerCamera;
    public float CameraDistance = 3.0F;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    private Transform target;
    
    void Update()
    {
        Vector3 targetPosition = playerCamera.transform.TransformPoint(new Vector3(0, 0, CameraDistance));
       
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        var lookAtPos = new Vector3(playerCamera.transform.position.x, transform.position.y, playerCamera.transform.position.z);
        transform.LookAt(lookAtPos);  
    }
}

/* --------------------------------------------------------------------------------
# Created by: Fabian Ramelsberger
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using UnityEngine;
using TMPro;
using Fusion;
using Fusion.XR.Shared.Grabbing.NetworkHandColliderBased;
using FusionHelpers;
using UnityEditor;

[RequireComponent(typeof(NetworkHandColliderGrabbable))]
public class NetworkHandColliderGrabbableCube : NetworkBehaviour
{
    public MeshRenderer CubeMeshRenderer => _cubeMeshRenderer;
    [SerializeField] private TextMeshProUGUI authorityText;
    [SerializeField] private TextMeshProUGUI debugText;
    [SerializeField] private MeshRenderer _cubeMeshRenderer;
    [SerializeField, ReadOnly] private bool _cubeIsAlreadyOwnedByPlayer = false;

    [Networked] public bool OwnershipIsGiven { get; set; }
    [SerializeField] private bool _cubeIsFromAPlayer = true;

    private void Awake()
    {
        debugText.text = "";
        var grabbable = GetComponent<NetworkHandColliderGrabbable>();
        grabbable.onDidGrab.AddListener(OnDidGrab);
        grabbable.onWillGrab.AddListener(OnWillGrab);
        grabbable.onDidUngrab.AddListener(OnDidUngrab);
    }

    public override void Spawned()
    {
        if (_cubeIsFromAPlayer || OwnershipIsGiven)
        {
            AssignCubeToPlayer();
        }
    }

    private void AssignCubeToPlayer()
    {
        PlayerRef playerId = GetComponent<NetworkObject>().StateAuthority;
        Runner.WaitForSingleton<PlayerManagerScript>(
            playerManager =>
            {
                if (_cubeIsAlreadyOwnedByPlayer == false)
                {
                    _cubeIsAlreadyOwnedByPlayer = true;
                    playerManager.RPC_AddCubeToPlayer(playerId, this);
                }
            });
    }

    private void DebugLog(string debug)
    {
        debugText.text = debug;
        Debug.Log(debug);
    }

    private void UpdateStatusCanvas()
    {
        if (Object.HasStateAuthority)
            authorityText.text = "You have the state authority on this object";
        else
            authorityText.text = "You have NOT the state authority on this object";
    }

    public override void Render()
    {
        base.Render();
        UpdateStatusCanvas();
    }

    void OnDidUngrab()
    {
        DebugLog($"{gameObject.name} ungrabbed");
    }

    void OnWillGrab(NetworkHandColliderGrabber newGrabber)
    {
        DebugLog($"Grab on {gameObject.name} requested by {newGrabber}. Waiting for state authority ...");
    }

    void OnDidGrab(NetworkHandColliderGrabber newGrabber)
    {
        AssignCubeToPlayer();
        OwnershipIsGiven = true;
        DebugLog($"{gameObject.name} grabbed by {newGrabber}");
    }
}
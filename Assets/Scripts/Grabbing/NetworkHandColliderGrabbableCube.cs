using UnityEngine;
using TMPro;
using Fusion;
using Fusion.XR.Shared.Grabbing.NetworkHandColliderBased;

[RequireComponent(typeof(NetworkHandColliderGrabbable))]
public class NetworkHandColliderGrabbableCube : NetworkBehaviour
{
    public TextMeshProUGUI authorityText;
    public TextMeshProUGUI debugText;
    public MeshRenderer _cubeMeshRenderer;
    private void Awake()
    {
        debugText.text = "";
        var grabbable = GetComponent<NetworkHandColliderGrabbable>();
        grabbable.onDidGrab.AddListener(OnDidGrab);
        grabbable.onWillGrab.AddListener(OnWillGrab);
        grabbable.onDidUngrab.AddListener(OnDidUngrab);
        
    }

    private void Start()
    {
        AssignCubeToPlayer();
    }

    private void AssignCubeToPlayer()
    {
        int playerId = GetComponent<NetworkObject>().StateAuthority.PlayerId;
        CubeManagerScript.Instance.RPC_AddCubeToPlayer(playerId, this);
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
        DebugLog($"{gameObject.name} grabbed by {newGrabber}");
    }
}

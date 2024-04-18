/* --------------------------------------------------------------------------------
# Created by: Fabian Ramelsberger
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using System.Collections.Generic;
using Fusion;
using Fusion.XR.Shared.Grabbing.NetworkHandColliderBased;
using UnityEngine;

//<summary>
//PlayerColorInteractor description
//	/summary>
public class PlayerColorInteractor : NetworkBehaviour
{
    [SerializeField] private List<NetworkHandColliderGrabber> _networkHands;
    private Material _playerMaterial;
    [SerializeField] private Color _playerColor;
    [SerializeField] private List<MeshRenderer> _playerMeshRenderers;
    private Color PlayerColor
    {
        get => _playerColor;
        set => _playerColor = value;
    }
    
    void Start()
    {
        _networkHands.ForEach(hand =>
        {
            hand.OnObjectGrabbedAction += OnObjectGrabbedAdjustColorToPlayer;
        });

        int playerId = GetComponent<NetworkObject>().InputAuthority.PlayerId;
        //This should just generate a new colour when the player sets the color
        if (Object.HasStateAuthority)
        {
            _playerColor = GetRandomColor();
            CubeManagerScript.Instance.Rpc_ChangeMaterialColor(playerId,_playerColor);
        }
        _playerMaterial = CubeManagerScript.Instance.GetPlayerMaterial(playerId, _playerColor); 

        _playerMeshRenderers.ForEach(meshRenderer =>
        {
            meshRenderer.sharedMaterial = _playerMaterial;
        });
    }

    private void OnObjectGrabbedAdjustColorToPlayer(NetworkHandColliderGrabbable grabbable)
    {
        grabbable.SetMaterial(_playerMaterial);
    }

    public static Color GetRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value, 1.0f); // 1.0f is for full opacity
    }
}

/* --------------------------------------------------------------------------------
# Created by: Fabian Ramelsberger
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using System.Collections.Generic;
using Fusion;
using Fusion.XR.Shared.Grabbing.NetworkHandColliderBased;
using FusionHelpers;
using UnityEngine;
using Random = UnityEngine.Random;

//<summary>
//PlayerColorInteractor description
//	/summary>
public class PlayerColorInteractor : NetworkBehaviour
{
    [SerializeField] private List<NetworkHandColliderGrabber> _networkHands;
    private Material _playerMaterial;
    [Networked]
    public Color NetworkedPlayerColor { get; set; }
    [SerializeField] private List<MeshRenderer> _playerMeshRenderers;
    private ChangeDetector _changeDetector;

    public override void Spawned()
    {
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
    }
    void Start()
    {
        _networkHands.ForEach(hand =>
        {
            hand.OnObjectGrabbedAction += OnObjectGrabbedAdjustColorToPlayer;
        });

        PlayerRef playerRef = GetComponent<NetworkObject>().InputAuthority;
        //This should just generate a new colour when the player sets the color
        if (Object.HasStateAuthority)
        {
            NetworkedPlayerColor = GetRandomColor();
        }

        Runner.WaitForSingleton<CubeManagerScript>(
            cubeManager => {
                _playerMaterial = cubeManager.GetPlayerMaterial(playerRef);
                _playerMaterial.color = NetworkedPlayerColor;

                _playerMeshRenderers.ForEach(meshRenderer =>
                {
                    meshRenderer.sharedMaterial = _playerMaterial;
                }); });
        
    }

    private void Update()
    {
        foreach (var change in _changeDetector.DetectChanges(this))
        {
            switch (change)
            {
                case nameof(NetworkedPlayerColor):
                {
                    Runner.WaitForSingleton<CubeManagerScript>(
                        cubeManager => { 
                            PlayerRef playerRef = GetComponent<NetworkObject>().InputAuthority;
                            _playerMaterial = cubeManager.GetPlayerMaterial(playerRef);
                            _playerMaterial.color = NetworkedPlayerColor;
                            _playerMeshRenderers.ForEach(meshRenderer =>
                            {
                                meshRenderer.sharedMaterial = _playerMaterial;
                            }); });
                   
                    break;
                }
            }
        }
    }

    private void OnObjectGrabbedAdjustColorToPlayer(NetworkHandColliderGrabbable grabbable)
    {
        grabbable.SetMaterial(_playerMaterial);
    }

    public static Color GetRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value, 1.0f); // 1.0f is for full opacity
    }
    
    void OnColorChanged()
    {
        Debug.Log($"Color changed to: {NetworkedPlayerColor} of player {Object.InputAuthority.PlayerId}");
        //_playerMaterial = CubeManagerScript.Instance.AssignPlayerColor(Object.InputAuthority.PlayerId, _networkedPlayerColor); ;
    }
}

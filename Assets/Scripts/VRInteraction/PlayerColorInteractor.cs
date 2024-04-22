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
// This is used in the Network rig. It set the colour to the Player color.
// Additionally, it sets a random colour and syncs it over the network
//	/summary>
public class PlayerColorInteractor : NetworkBehaviour
{
    [SerializeField] private List<NetworkHandColliderGrabber> _networkHands;
    [Networked] public Color NetworkedPlayerColor { get; set; }
    private Material _playerMaterial;
    [SerializeField] private List<MeshRenderer> _playerMeshRenderers;
    private ChangeDetector _changeDetector;

    public override void Spawned()
    {
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
    }

    private void Start()
    {
        _networkHands.ForEach(hand => { hand.OnObjectGrabbedAction += OnObjectGrabbedAdjustColorToPlayer; });

        PlayerRef playerRef = GetComponent<NetworkObject>().InputAuthority;
        //This should just generate a new colour when the player sets the color
        // we only want to generate a new random colour if we are the player
        if (Object.HasStateAuthority)
        {
            NetworkedPlayerColor = GetRandomColor();
        }

        Runner.WaitForSingleton<PlayerManagerScript>(
            cubeManager =>
            {
                _playerMaterial = cubeManager.GetPlayerMaterial(playerRef);
                _playerMaterial.color = NetworkedPlayerColor;

                _playerMeshRenderers.ForEach(meshRenderer => { meshRenderer.sharedMaterial = _playerMaterial; });
            });
    }

    private void Update()
    {
        foreach (var change in _changeDetector.DetectChanges(this))
        {
            switch (change)
            {
                case nameof(NetworkedPlayerColor):
                {
                    Runner.WaitForSingleton<PlayerManagerScript>(
                        cubeManager =>
                        {
                            PlayerRef playerRef = GetComponent<NetworkObject>().InputAuthority;
                            _playerMaterial = cubeManager.GetPlayerMaterial(playerRef);
                            _playerMaterial.color = NetworkedPlayerColor;
                            _playerMeshRenderers.ForEach(meshRenderer =>
                            {
                                meshRenderer.sharedMaterial = _playerMaterial;
                            });
                        });
                    break;
                }
            }
        }
    }

    private void OnObjectGrabbedAdjustColorToPlayer(NetworkHandColliderGrabbable grabbable)
    {
        grabbable.SetMaterial(_playerMaterial);
    }

    private static Color GetRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value, 1.0f); // 1.0f is for full opacity
    }
}
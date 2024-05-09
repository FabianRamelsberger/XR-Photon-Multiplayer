/* --------------------------------------------------------------------------------
# Created by: Fabian Ramelsberger
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

[Serializable]
public class Player
{
    [SerializeField, ReadOnly] private string _debugPlayerRef;
    private PlayerRef _playerRef;
    [SerializeField] private Material _playerMaterial;
    [SerializeField, ReadOnly] private List<NetworkHandColliderGrabbableCube> _playerCubeList;
    [SerializeField, ReadOnly] private List<NetworkHandColliderGrabbableCube> _playerToeList;
    [SerializeField] private List<Transform> _cubeSpawnPoints;
    [SerializeField] private Transform _playerSpawnPoint;

    [Header("Tick Tack Toe Gameplay")]
    [SerializeField] private List<Transform> _toeSpawnPoints;
    
    public PlayerRef PlayerRef => _playerRef;
    public Material PlayerMaterial => _playerMaterial;
    public List<NetworkHandColliderGrabbableCube> PlayerCubeList => _playerCubeList;
    public List<Transform> CubeSpawnPoints => _cubeSpawnPoints;
    public Transform PlayerSpawnPoint => _playerSpawnPoint;

    public List<Transform> ToeSpawnPoints => _toeSpawnPoints;
    public List<NetworkHandColliderGrabbableCube> PlayerToeList => _playerToeList;


    public void SetPlayerRef(PlayerRef playerRef)
    {
        _playerRef = playerRef;
        _debugPlayerRef = playerRef.ToString();
    }

    public void UpdatePlayerCubesMaterials()
    {
        _playerCubeList.ForEach(cube =>
        {
            if(cube.CubeMeshRenderer)
                cube.CubeMeshRenderer.sharedMaterial = _playerMaterial;
        });
    }
}
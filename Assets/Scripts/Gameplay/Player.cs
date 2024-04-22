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
    [SerializeField] private List<NetworkHandColliderGrabbableCube> _playerCubeList;
    [SerializeField] private List<Transform> _cubeSpawnPoints;
    [SerializeField] private Transform _playerSpawnPoint;

    public PlayerRef PlayerRef => _playerRef;
    public Material PlayerMaterial => _playerMaterial;
    public List<NetworkHandColliderGrabbableCube> PlayerCubeList => _playerCubeList;
    public List<Transform> CubeSpawnPoints => _cubeSpawnPoints;
    public Transform PlayerSpawnPoint => _playerSpawnPoint;

    public void SetPlayerRef(PlayerRef playerRef)
    {
        _playerRef = playerRef;
        _debugPlayerRef = playerRef.ToString();
    }

    public void UpdatePlayerCubesMaterials()
    {
        _playerCubeList.ForEach(cube => { cube.CubeMeshRenderer.sharedMaterial = _playerMaterial; });
    }
}
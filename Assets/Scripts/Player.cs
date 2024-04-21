using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

[Serializable]
public class Player
{
    public string DebugPlayerRef;
    public PlayerRef playerRef;
    public Material playerMaterial;
    public List<NetworkHandColliderGrabbableCube> PlayerCubes;
    public List<Transform> CubeSpawnPoints;
    public Transform PlayerSpawnPoint;
    public void UpdatePlayerCubesMaterials()
    {
        PlayerCubes.ForEach(cube =>
        {
            cube.CubeMeshRenderer.sharedMaterial = playerMaterial;
        });
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Player
{
    public int PlayerIndex;
    public Material playerMaterial;
    public List<NetworkHandColliderGrabbableCube> PlayerCubes;
    public List<Transform> CubeSpawnPoints;
    public Transform PlayerSpawnPoint;
    public void UpdatePlayerCubesMaterials()
    {
        PlayerCubes.ForEach(cube =>
        {
            cube._cubeMeshRenderer.sharedMaterial = playerMaterial;
        });
    }
}

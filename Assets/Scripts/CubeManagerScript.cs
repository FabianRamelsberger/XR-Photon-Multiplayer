/* --------------------------------------------------------------------------------
# Created by: Fabian Ramelsberger
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;



//<summary>
//CubeManagerScript description
//	</summary>
public class CubeManagerScript : NetworkBehaviour
{
    [Serializable]
    public class Player
    {
        public int PlayerIndex;
        public Material playerMaterial;
        public List<NetworkHandColliderGrabbableCube> PlayerCubes;
    }

    public static CubeManagerScript Instance;

    
    [SerializeField] private List<Player> _playerList;
    public List<Player> PlayerList
    {
        get => _playerList;
        set => _playerList = value;
    }

     public void Awake()
    {
        if (Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public Material GetPlayerMaterial(int playerId, Color playerColor)
    {
        Player player =  PlayerList.Find(player => player.PlayerIndex == playerId);
        if (player == null)
        {
            Debug.LogError($"Player with {playerId} could not be found.");
        }
        return player.playerMaterial;
    }
    
    [Rpc]
    public void Rpc_ChangeMaterialColor(int playerId, Color playerColor)
    {
        Player player =  PlayerList.Find(player => player.PlayerIndex == playerId);
        player.playerMaterial.color = playerColor;
    }
}

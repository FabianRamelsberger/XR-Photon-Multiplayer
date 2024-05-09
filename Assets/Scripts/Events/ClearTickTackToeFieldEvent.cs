using Fusion;
using Network;
using UnityEngine;

public class ClearTickTackToeFieldEvent : INetworkPlayerEvent
{
    public void Execute(Transform emitter, NetworkRunner runner, PlayerRef player)
    {
       PlayerManagerScript.Instance.PlayerLeftClearTickTackToe(runner,player);
    }
}

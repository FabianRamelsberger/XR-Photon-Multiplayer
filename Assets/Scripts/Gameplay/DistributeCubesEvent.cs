using Fusion;
using Network;
using UnityEngine;

public class DistributeCubesEvent : INetworkPlayerEvent
{
    public void Execute(Transform emitter, NetworkRunner runner, PlayerRef player)
    {
       PlayerManagerScript.Instance.PlayerLeftDistributeCubes(runner,player);
    }
}

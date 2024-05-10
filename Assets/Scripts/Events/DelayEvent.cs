using System.Collections;
using System.Collections.Generic;
using FREngine.Events;
using Fusion;
using Fusion.Sockets;
using Network;
using UnityEngine;

public class DelayEvent : IEvent, INetworkPlayerEvent
{
    [SerializeField] private float _waitForSeconds = 1.0f;
    [SerializeField,SerializeReference] private List<IEvent> _eventsToExecuteAfterWait = new();
    [SerializeField,SerializeReference] private List<INetworkPlayerEvent> _networkPlayerEvents = new();
    public void Execute(Transform emitter)
    {
        if (emitter.GetComponent<MonoBehaviour>())
        {
            emitter.GetComponent<MonoBehaviour>().StartCoroutine(WaitForSeconds(emitter));
        }
    }

    private IEnumerator WaitForSeconds(Transform emitter)
    {
        yield return new WaitForSeconds(_waitForSeconds);
        Utility.Emit(emitter, _eventsToExecuteAfterWait);
    }
    
    private IEnumerator WaitForSeconds(Transform emitter, NetworkRunner runner, PlayerRef player)
    {
        yield return new WaitForSeconds(_waitForSeconds);
        NetworkUtility.Emit(emitter, _networkPlayerEvents, runner, player);
    }
    public void Execute(Transform emitter, NetworkRunner runner, PlayerRef player)
    {
        if (emitter.GetComponent<MonoBehaviour>())
        {
            emitter.GetComponent<MonoBehaviour>().StartCoroutine(WaitForSeconds(emitter,runner, player));
        }
    }
}

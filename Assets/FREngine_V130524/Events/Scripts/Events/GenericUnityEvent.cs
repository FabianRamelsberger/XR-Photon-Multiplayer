using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace FREngine.Events
{
    [System.Serializable, HideLabel]
    public class CustomEvent : IEvent
    {
        [SerializeField, HideReferenceObjectPicker] private UnityEvent _unityEvent = new UnityEvent();

        public void Execute(Transform emitter)
        {
            _unityEvent?.Invoke();
        }
    }

}
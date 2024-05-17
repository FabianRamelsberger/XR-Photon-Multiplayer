/* --------------------------------------------------------------------------------
# Script Name: FREngine Component
# Created by: Fabian Ramelsberger
# Part of: FREngine
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using System.Collections.Generic;
using FREngine.Events;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FREngine.Events
{
    public class GenericEventEmitter : SerializedMonoBehaviour
    {
        [SerializeReference, SerializeField] private List<IEvent> _events = new();
        [SerializeField, ReadOnly] private float _lastTrigger = -1f;

        protected void Emit()
        {
            ExecuteEvents(_events, transform);
        }
        
        protected void Emit(Transform customTransform)
        {
            ExecuteEvents(_events, customTransform);
        }
        
        private void ExecuteEvents(List<IEvent> genericEvents, Transform t)
        {
            if (CheckEvents(genericEvents) == false)
            {
                return;
            }
            _lastTrigger = Time.time;
            foreach (IEvent e in genericEvents)
            {
                e.Execute(t);
            }
        }
        
        private bool CheckEvents(List<IEvent> genericEvents)
        {
            foreach (IEvent e in genericEvents)
            {
                if (e == null)
                {
                    Debug.LogError("Event is null for " + name, this);
                    return false;
                }
            }

            return true;
        }
    }
}

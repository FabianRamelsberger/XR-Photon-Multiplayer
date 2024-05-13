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
    public class GenericIntegerEventEmitter : SerializedMonoBehaviour
    {
        [SerializeReference, SerializeField] private List<IIntegerEvent> _events = new();
        [SerializeField, ReadOnly] private float _lastTrigger = -1f;

        protected void Emit(int value)
        {
            ExecuteEvents(_events, transform, value);
        }
        
        protected void Emit(Transform customTransform, int value)
        {
            ExecuteEvents(_events, customTransform, value);
        }

        protected void Emit(List<IIntegerEvent> events, int value)
        {
            ExecuteEvents(events, transform, value);
        }

        private void ExecuteEvents(List<IIntegerEvent> genericEvents, Transform t, int value)
        {
            if (CheckEvents(genericEvents) == false)
            {
                return;
            }
            _lastTrigger = Time.time;
            foreach (IIntegerEvent e in genericEvents)
            {
                e.Execute(t, value);
            }
        }
        
        private bool CheckEvents(List<IIntegerEvent> genericEvents)
        {
            foreach (IIntegerEvent e in genericEvents)
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

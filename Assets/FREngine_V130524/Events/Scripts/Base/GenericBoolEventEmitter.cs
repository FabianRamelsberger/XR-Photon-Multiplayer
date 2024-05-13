/* --------------------------------------------------------------------------------
# Script Name: FREngine Component
# Created by: Fabian Ramelsberger
# Part of: FREngine
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FREngine.Events
{
    public class GenericBoolEventEmitter : SerializedMonoBehaviour
    {
        [SerializeReference, SerializeField] private List<IBoolEvent> _events = new();
        [SerializeField, Unity.Collections.ReadOnly] private float _lastTrigger = -1f;

        protected void Emit(bool val)
        {
            ExecuteEvents(_events, transform, val);
        }
        
        protected void Emit(Transform customTransform, bool val)
        {
            ExecuteEvents(_events, customTransform, val);
        }

        protected void Emit(List<IBoolEvent> events, bool val)
        {
            ExecuteEvents(events, transform, val);
        }

        private void ExecuteEvents(List<IBoolEvent> genericEvents, Transform t, bool val)
        {
            if (CheckEvents(genericEvents) == false)
            {
                return;
            }
            _lastTrigger = Time.time;
            foreach (IBoolEvent e in genericEvents)
            {
                e.Execute(t, val);
            }
        }
        
        private bool CheckEvents(List<IBoolEvent> genericEvents)
        {
            foreach (IBoolEvent e in genericEvents)
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

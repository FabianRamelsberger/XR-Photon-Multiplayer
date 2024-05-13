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
    public class IfConditionBoolEvent: IBoolEvent
    {
        [SerializeField, SerializeReference]
        [InlineProperty, LabelText("THEN EVENTS")]
        private List<IEvent> _thenEvents = new();
        
        [SerializeField, SerializeReference]
        [InlineProperty, LabelText("ELSE EVENTS")]
        private List<IEvent> _elseEvents = new();

        public void Execute(Transform emitter, bool value)
        {
            if (value == false)
            {
                foreach (IEvent e in _elseEvents)
                {
                    e.Execute(emitter);
                }
            }
            else
            {
                foreach (IEvent e in _thenEvents)
                {
                    e.Execute(emitter);
                } 
            }
        }
    }
}

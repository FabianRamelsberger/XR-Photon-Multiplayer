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
    public class IfConditionEvent: IEvent
    {
        [SerializeField, SerializeReference]
        [InlineProperty, LabelText("IF")]
        private List<ICondition> _ifConditions = new();
        
        [SerializeField, SerializeReference]
        [InlineProperty, LabelText("THEN EVENTS")]
        private List<IEvent> _thenEvents = new();
        
        [SerializeField, SerializeReference]
        [InlineProperty, LabelText("ELSE EVENTS")]
        private List<IEvent> _elseEvents = new();
        
        public void Execute(Transform emitter)
        {
            foreach (ICondition condition in _ifConditions)
            {
                if (condition.Execute(emitter) == false)
                {
                    foreach (IEvent e in _elseEvents)
                    {
                        e.Execute(emitter);
                    }
                    return;
                }
            }

            foreach (IEvent e in _thenEvents)
            {
                e.Execute(emitter);
            }
        }
    }
}

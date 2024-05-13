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
    public class IfIntegerConditionEvent: IIntegerEvent
    {
        [SerializeField, SerializeReference]
        [InlineProperty, LabelText("IF")]
        private List<IIntegerCondition> _ifConditions = new();
        
        [SerializeField, SerializeReference]
        [InlineProperty, LabelText("THEN EVENTS")]
        private List<IIntegerEvent> _thenEvents = new();
        
        [SerializeField, SerializeReference]
        [InlineProperty, LabelText("ELSE EVENTS")]
        private List<IIntegerEvent> _elseEvents = new();
        
        public void Execute(Transform emitter, int value)
        {
            foreach (IIntegerCondition condition in _ifConditions)
            {
                if (condition.Execute(emitter, value) == false)
                {
                    foreach (IIntegerEvent e in _elseEvents)
                    {
                        e.Execute(emitter, value);
                    }
                    return;
                }
            }

            foreach (IIntegerEvent e in _thenEvents)
            {
                e.Execute(emitter, value);
            };
        }
    }
}

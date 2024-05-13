/* --------------------------------------------------------------------------------
# FREngine
# Created by: Fabian Ramelsberger
# Part of: FREngine
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using UnityEngine;

namespace FREngine.Events
{
    public class EnableComponentEvent:IEvent
    {
        [SerializeField] private MonoBehaviour _component;
        [SerializeField] private bool _enable;
        
        public void Execute(Transform emitter)
        {
            _component.enabled = _enable;
        }
    }
}
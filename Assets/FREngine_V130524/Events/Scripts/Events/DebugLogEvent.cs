/* --------------------------------------------------------------------------------
# FREngine
# Created by: Fabian Ramelsberger
# Part of: FREngine
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using UnityEngine;

namespace FREngine.Events
{
    public class DebugLogEvent:IEvent, IBoolEvent
    {
        [SerializeField] private string _logString;
        
        public void Execute(Transform emitter)
        {
            Debug.Log(_logString);
        }

        public void Execute(Transform emitter, bool value)
        {
            Debug.Log($" {_logString}: the component is active {value}");

        }
    }
}
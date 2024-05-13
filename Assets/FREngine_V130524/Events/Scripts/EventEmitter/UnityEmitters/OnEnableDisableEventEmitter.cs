/* --------------------------------------------------------------------------------
# Script Name: FREngine Component
# Created by: Fabian Ramelsberger
# Part of: FREngine
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using UnityEngine;

namespace FREngine.Events
{
    public class OnEnableDisableEventEmitter:GenericBoolEventEmitter
    {
        [SerializeField] private bool _invertBool = false;
        
        private void OnEnable()
        {
            var enable = InvertBool(true);
            Emit(enable);
        }
        
        private void OnDisable()
        {
            var enable = InvertBool(false);
            Emit(enable);
        }
        
        private bool InvertBool(bool startBool)
        {
            bool enable = startBool;
            if (_invertBool)
                enable = !enable;
            return enable;
        }
    }
}

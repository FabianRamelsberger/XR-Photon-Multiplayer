/* --------------------------------------------------------------------------------
# Script Name: FREngine Component
# Created by: Fabian Ramelsberger
# Part of: FREngine
# Created Date: 2024
# --------------------------------------------------------------------------------*/
using UnityEngine;

namespace FREngine.Events
{
    public class OnTriggerExitEventEmitter: GenericEventEmitter
    {
        private void OnTriggerExit(Collider other)
        {
            if (enabled == false)
            {
                return;
            }
            
            Emit(other.transform);
        }
    }
}

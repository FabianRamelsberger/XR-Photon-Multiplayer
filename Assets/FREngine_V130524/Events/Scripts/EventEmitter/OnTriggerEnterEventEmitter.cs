/* --------------------------------------------------------------------------------
# Script Name: FREngine Component
# Created by: Fabian Ramelsberger
# Part of: FREngine
# Created Date: 2024
# --------------------------------------------------------------------------------*/
using UnityEngine;

namespace FREngine.Events
{
    public class OnTriggerEnterEventEmitter: GenericEventEmitter
    {
        private void OnTriggerEnter(Collider other)
        {
            if (enabled == false)
            {
                return;
            }
            Emit(other.transform);
        }
    }
}

/* --------------------------------------------------------------------------------
# Script Name: FREngine Component
# Created by: Fabian Ramelsberger
# Part of: FREngine
# Created Date: 2024
# --------------------------------------------------------------------------------*/
using UnityEngine;

namespace FREngine.Events
{
    public class DestroySelf:IEvent
    {
        public void Execute(Transform emitter)
        {
            GameObject.Destroy(emitter.gameObject);
        }
    }
}

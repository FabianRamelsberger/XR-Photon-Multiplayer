/* --------------------------------------------------------------------------------
# Script Name: FREngine Component
# Created by: Fabian Ramelsberger
# Part of: FREngine
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using UnityEngine;

namespace FREngine.Events
{
    public interface IBoolEvent
    {
        public void Execute(Transform emitter, bool value);
    }
}

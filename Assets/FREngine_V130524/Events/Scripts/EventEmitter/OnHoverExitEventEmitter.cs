/* --------------------------------------------------------------------------------
# Script Name: FREngine Component
# Created by: Fabian Ramelsberger
# Part of: FREngine
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FREngine.Events
{
    public class OnHoverExitEventEmitter: GenericEventEmitter, IPointerExitHandler
    {
        

        public void OnPointerExit(PointerEventData eventData)
        {
            Emit();
        }
    }
}

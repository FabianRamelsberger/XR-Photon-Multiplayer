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
    public class OnHoverEnterEventEmitter: GenericEventEmitter, IPointerEnterHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            Emit();
        }
    }
}

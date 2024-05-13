using System.Collections.Generic;
using UnityEngine;

namespace FREngine.Events
{
    public class UnityMouseInteractionEvents : MonoBehaviour
    {
        
        [SerializeReference, SerializeField] private List<IEvent> _enterHoverEvents = new();
        [SerializeReference, SerializeField] private List<IEvent> _exitHoverEvents = new();
        [SerializeReference, SerializeField] private List<IEvent> _clickedEvents = new();
        private bool _isHovering = false;

        // Event when the mouse hovers over the sprite
        private void OnMouseOver()
        {
            if(_isHovering== false)
            {
                _isHovering = true;
                Utility.Emit(transform,_enterHoverEvents);
            }
        }

        // Event when the mouse exits the sprite
        private void OnMouseExit()
        {
            if(_isHovering== true)
            {
                _isHovering = false;
                Utility.Emit(transform,_exitHoverEvents);
            }
        }

        // Event when the mouse clicks on the sprite
        private void OnMouseDown()
        {
            Utility.Emit(transform,_clickedEvents);
        }
    }
}

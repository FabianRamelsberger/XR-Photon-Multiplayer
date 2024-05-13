/* --------------------------------------------------------------------------------
# Script Name: FREngine Component
# Created by: Fabian Ramelsberger
# Part of: FREngine
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using UnityEngine;

namespace FREngine.Events
{
    public class EnableGameObjectEvent : IEvent, IBoolEvent
    {
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private bool _invert = false;
        public void Execute(Transform emitter)
        {
            if (_invert)
            {
                _gameObject.SetActive(false);
            }
            else
            {
                _gameObject.SetActive(true);
            }
        }

        public void Execute(Transform emitter, bool value)
        {
            bool setActive = _invert ? value : !value; 
            _gameObject.SetActive(setActive);
        }
    }
}

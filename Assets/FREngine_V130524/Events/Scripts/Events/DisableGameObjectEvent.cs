/* --------------------------------------------------------------------------------
# FREngine
# Created by: Fabian Ramelsberger
# Part of: FREngine
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using UnityEngine;

namespace FREngine.Events
{
    public class DisableGameObjectEvent:IEvent
    {
        [SerializeField] private GameObject _gameObject;
        public void Execute(Transform emitter)
        {
                _gameObject.SetActive(false);
        }
    }
}
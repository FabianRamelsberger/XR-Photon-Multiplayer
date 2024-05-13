/* --------------------------------------------------------------------------------
# Script Name: FREngine Component
# Created by: Fabian Ramelsberger
# Part of: FREngine
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace FREngine.Events
{
    [Serializable]
    public struct EnableGameObject
    {
        public GameObject GameObject;
        public bool Enable;
    }
    
    public class EnableGameObjectListEvent : IEvent
    {
        [SerializeField] private List<EnableGameObject> _gameObjectList = new();
        public void Execute(Transform emitter)
        {
            _gameObjectList.ForEach(enableGameObject =>
            {
                enableGameObject.GameObject.SetActive(enableGameObject.Enable);
            });
        }
    }
}

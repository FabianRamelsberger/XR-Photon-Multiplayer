/* --------------------------------------------------------------------------------
# Script Name: FREngine Component
# Created by: Fabian Ramelsberger
# Part of: FREngine
# Created Date: 2024
# --------------------------------------------------------------------------------*/
using UnityEngine;

namespace FREngine.Events
{
    public class CreateGameObjectEvent:IEvent
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Transform _parentTransform;
        public void Execute(Transform emitter)
        {
            if (_prefab == null)
            {
                Debug.LogError("no prefab assigned to instanciate");
                return;
            }

            GameObject go = null;
            if (_parentTransform)
            {
                go = GameObject.Instantiate(_prefab, _parentTransform);
            }
            else
            {
                go =GameObject.Instantiate(_prefab);
            }

            go.transform.position = _parentTransform.position;
            go.transform.rotation = new Quaternion();

        }
    }
}

using FREngine.Events;
using UnityEngine;

namespace TickTacToeGame.Scripts
{
    public class CreateToeVisualAndDestroyOld:IEvent
    {
        [SerializeField] private Transform _parentTransform;

        public void Execute(Transform emitter)
        {
            ToeCubeElement toeCubeElement = emitter.GetComponentInParent<ToeCubeElement>();
            if (toeCubeElement)
            {
                GameObject go = null;
                if (_parentTransform)
                {
                    go = GameObject.Instantiate(toeCubeElement.VisualPrefab, _parentTransform);
                }
                else
                {
                    go =GameObject.Instantiate(toeCubeElement.VisualPrefab);
                }

                go.transform.position = _parentTransform.position;
                go.transform.rotation = new Quaternion();
                GameObject.Destroy(emitter.gameObject);
            }
        }
    }
}
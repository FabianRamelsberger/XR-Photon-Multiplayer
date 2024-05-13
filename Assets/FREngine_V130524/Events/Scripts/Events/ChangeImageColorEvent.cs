using UnityEngine;
using UnityEngine.UI;

namespace FREngine.Events
{
    public class ChangeImageColorEvent: IEvent, IIntegerEvent
    {
        [SerializeField] private Color _color;
        public void Execute(Transform emitter)
        {
            emitter.GetComponent<Image>().color = _color;
        }

        public void Execute(Transform emitter, int value)
        {
            emitter.GetComponent<Image>().color = _color;
        }
    }
}
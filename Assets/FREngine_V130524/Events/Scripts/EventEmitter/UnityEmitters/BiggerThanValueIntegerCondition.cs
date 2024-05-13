using UnityEngine;

namespace FREngine.Events
{
    public class BiggerThanValueIntegerCondition: IIntegerCondition
    {
        [Range(0,100)]
        [SerializeField] private int value =0;

        public bool Execute(Transform emitter, int value)
        {
            float randomExecutionNumber = Random.Range(0, 100);
            if(value > randomExecutionNumber)
            {
                return true;
            }

            return false;;
        }
    }
}

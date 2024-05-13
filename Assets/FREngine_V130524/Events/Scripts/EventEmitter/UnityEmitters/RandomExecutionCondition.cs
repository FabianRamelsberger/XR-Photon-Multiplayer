using UnityEngine;

namespace FREngine.Events
{
    public class RandomExecutionCondition: ICondition
    {
        [Range(0,100)]
        [SerializeField] private float _executionProb=0;
        public bool Execute(Transform emitter)
        {
            float randomExecutionNumber = Random.Range(0, 100);
            if(_executionProb > randomExecutionNumber)
            {
                return true;
            }

            return false;
        }
    }
}

/* --------------------------------------------------------------------------------
# Script Name: FREngine Component
# Created by: Fabian Ramelsberger
# Part of: FREngine
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using UnityEngine;

namespace FREngine.Events
{
    public class SetAnimationTriggerEvent : IEvent
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string triggerName = "myBool";
        public void Execute(Transform emitter)
        {
            if (animator == null)
                emitter.GetComponent<Animator>();
            
            if(animator)
                animator.SetTrigger(triggerName);
        }
    }
}

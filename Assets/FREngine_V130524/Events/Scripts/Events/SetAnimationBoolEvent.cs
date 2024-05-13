/* --------------------------------------------------------------------------------
# Script Name: FREngine Component
# Created by: Fabian Ramelsberger
# Part of: FREngine
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using UnityEngine;

namespace FREngine.Events
{
    public class SetAnimationBoolEvent : IEvent, IBoolEvent
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string boolName = "myBool";
        [SerializeField] private bool _enable = true;
        public void Execute(Transform emitter)
        {
            if (animator == null)
                emitter.GetComponent<Animator>();
            
            if(animator)
                animator.SetBool(boolName, _enable);
        }

        public void Execute(Transform emitter, bool value)
        {
            if (animator == null)
                emitter.GetComponent<Animator>();
            
            if(animator)
                animator.SetBool(boolName, value);
        }
    }
}

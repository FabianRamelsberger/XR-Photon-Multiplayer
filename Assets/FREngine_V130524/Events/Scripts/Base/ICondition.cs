/* --------------------------------------------------------------------------------
# Script Name: FREngine Component
# Created by: Fabian Ramelsberger
# Part of: FREngine
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using UnityEngine;

namespace FREngine.Events
{
    public interface ICondition
    {
        public bool Execute(Transform emitter);
    }
    
    public interface IIntegerCondition
    {
        public bool Execute(Transform emitter,int value);
    }
}

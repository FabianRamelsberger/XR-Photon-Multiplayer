/* --------------------------------------------------------------------------------
# Script Name: FREngine Component
# Created by: Fabian Ramelsberger
# Part of: FREngine
# Created Date: 2024
# --------------------------------------------------------------------------------*/

namespace FREngine.Events
{
    public class OnStartEventEmitter: GenericEventEmitter
    {
        private void Start()
        {
            Emit();
        }
    }
}
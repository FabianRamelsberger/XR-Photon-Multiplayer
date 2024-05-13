/* --------------------------------------------------------------------------------
# Script Name: FREngine Component
# Created by: Fabian Ramelsberger
# Part of: FREngine
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using System.Collections.Generic;
using UnityEngine;

namespace FREngine.Events
{
    public class Utility
    {
        public static void Emit(Transform emitter, List<IEvent> events)
        {
            if (events != null)
            {
                foreach (var e in events)
                {
                    if (e != null)
                    {
                        e.Execute(emitter);
                    }
                    else
                    {
                        Debug.LogError("Event is null for " + emitter, emitter);
                    }
                }
            }
        }
        
        public static void Emit(Transform emitter, List<IBoolEvent> events, bool boolean)
        {
            if (events != null)
            {
                foreach (var e in events)
                {
                    if (e != null)
                    {
                        e.Execute(emitter, boolean);
                    }
                    else
                    {
                        Debug.LogError("Event is null for " + emitter, emitter);
                    }
                }
            }
        }
        

        
    }
}

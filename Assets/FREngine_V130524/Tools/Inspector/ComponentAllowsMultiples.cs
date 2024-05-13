using UnityEngine;

namespace FREngine.Tools.Inspector
{
    public static class ComponentAllowsMultiples
    {
        public static bool Check(Component component)
        {
            System.Type componentType = component.GetType();
            DisallowMultipleComponent[] attributes = (DisallowMultipleComponent[])
                componentType.GetCustomAttributes(typeof(DisallowMultipleComponent),true);
            return attributes.Length == 0;
        }
    }
}

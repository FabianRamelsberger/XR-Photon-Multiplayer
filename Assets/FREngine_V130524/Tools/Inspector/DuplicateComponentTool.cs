using UnityEditor;
using UnityEngine;

namespace FREngine.Tools.Inspector
{
    public static class DuplicateComponentTool
    {
        private const string menuPath = "CONTEXT/Component/Duplicate";
        
        [MenuItem(menuPath, priority = 503)]
        public static void DuplicateComponentOption(MenuCommand command)
        {
            Component sourceComponent = command.context as Component;
            Component newComponent = Undo.AddComponent(sourceComponent.gameObject,sourceComponent.GetType());

            SerializedObject source = new SerializedObject(sourceComponent);
            SerializedObject target = new SerializedObject(newComponent);
            SerializedProperty iterator = source.GetIterator();
            while (iterator.NextVisible(true))
            {
                target.CopyFromSerializedProperty(iterator);
            }
            target.ApplyModifiedProperties();
        }
        
        [MenuItem(menuPath, validate = true), ]
        public static bool DuplicateComponentOptionValidation(MenuCommand command)
        {
            return ComponentAllowsMultiples.Check(command.context as Component);
        }
    }
}

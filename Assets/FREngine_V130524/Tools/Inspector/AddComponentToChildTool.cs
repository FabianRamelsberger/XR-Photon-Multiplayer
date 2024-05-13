#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace FREngine.Tools.Inspector
{
    public class AddComponentToChildTool
    {
        private const string menuPath = "CONTEXT/Component/Extract";
        [MenuItem(menuPath, priority = 504)]
        public static void ExtractMenuOption(MenuCommand command)
        {
            Component sourceComponent = command.context as Component;

            int undGroupIndex = Undo.GetCurrentGroup();
            Undo.IncrementCurrentGroup();
            
            GameObject gameObject = new GameObject(sourceComponent.GetType().Name);
            gameObject.transform.parent = sourceComponent.transform;
            gameObject.transform.localScale = Vector3.one;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;
            
            Undo.RegisterCreatedObjectUndo(gameObject, "Created child object");

            if (!ComponentUtility.CopyComponent(sourceComponent) || !ComponentUtility.PasteComponentAsNew(gameObject))
            {
                Debug.LogError("Cannot extract component", sourceComponent.gameObject);
                Undo.CollapseUndoOperations(undGroupIndex);
                Undo.PerformUndo();
                return;
            }
            
            Undo.DestroyObjectImmediate(sourceComponent);
            
            Undo.CollapseUndoOperations(undGroupIndex);
        }
    }
}
#endif

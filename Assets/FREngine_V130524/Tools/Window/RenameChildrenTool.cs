namespace FREngine.Tools.Window
{
    using UnityEngine;
    using UnityEditor;

    public class RenameChildrenTool : EditorWindow
    {
        string prefix = "";
        bool includeNumbering = false;

        // Add menu named "Rename Children Tool" to the Unity "Tools" menu
        [MenuItem("Tools/Rename Children Tool")]
        public static void ShowWindow()
        {
            // Show existing window instance. If one doesn't exist, make one.
            EditorWindow.GetWindow(typeof(RenameChildrenTool));
        }

        void OnGUI()
        {
            GUILayout.Label("Base Settings", EditorStyles.boldLabel);
            prefix = EditorGUILayout.TextField("Prefix", prefix);
            includeNumbering = EditorGUILayout.Toggle("Include Numbering", includeNumbering);

            if (GUILayout.Button("Rename Children"))
            {
                RenameChildren();
            }
            
            if (GUILayout.Button("Rename Selected"))
            {
                RenameSelected();
            }
        }

        void RenameChildren()
        {
            // Check if a GameObject is selected
            if (Selection.activeGameObject != null)
            {
                int counter = 1;
                foreach (Transform child in Selection.activeGameObject.transform)
                {
                    // Construct new name
                    string newName = prefix;
                    if (includeNumbering)
                    {
                        newName += counter.ToString();
                    }

                    child.gameObject.name = newName;
                    counter++;
                }
            }
            else
            {
                Debug.LogWarning("No GameObject selected. Please select a parent GameObject to rename its children.");
            }
        }
        
        void RenameSelected()
        {
            if (Selection.gameObjects.Length > 0)
            {
                int counter = 1;
                foreach (GameObject obj in Selection.gameObjects)
                {
                    obj.name = ConstructName(counter);
                    counter++;
                }
            }
            else
            {
                Debug.LogWarning("No GameObjects selected. Please select one or more GameObjects to rename.");
            }
        }
    
        string ConstructName(int counter)
        {
            string newName = prefix;
            if (includeNumbering)
            {
                newName += counter.ToString();
            }
            return newName;
        }
    }
}
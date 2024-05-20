namespace FREngine.Tools.Window
{
    using UnityEngine;
    using UnityEditor;

    /// <summary>
    /// Provides a GUI within the Unity Editor for managing GameObjects in the hierarchy,
    /// including renaming, duplicating, and parenting operations.
    /// </summary>
    public class HierarchyProductivityTool : EditorWindow
    {
        [SerializeField] private string prefix = "";
        [SerializeField] private bool includeNumbering = false;
        private const float ButtonWidth = 200;
        private const float ButtonHeight = 30;

        [MenuItem("Tools/Hierarchy Productivity Tool")]
        public static void ShowWindow()
        {
            GetWindow(typeof(HierarchyProductivityTool));
        }

        void OnGUI()
        {
            GUILayout.Label("Rename Operations", EditorStyles.boldLabel);
            prefix = EditorGUILayout.TextField("Prefix", prefix);
            includeNumbering = EditorGUILayout.Toggle("Include Numbering", includeNumbering);

            // Rename buttons with blue color
            GUI.backgroundColor = Color.blue;
            if (GUILayout.Button(new GUIContent("Rename Children", EditorGUIUtility.IconContent("d_RotateTool").image), GUILayout.Width(ButtonWidth), GUILayout.Height(ButtonHeight)))
            {
                GameObjectActions.RenameChildren(Selection.activeGameObject, prefix, includeNumbering);
            }

            if (GUILayout.Button(new GUIContent("Rename Selected", EditorGUIUtility.IconContent("d_MoveTool").image), GUILayout.Width(ButtonWidth), GUILayout.Height(ButtonHeight)))
            {
                GameObjectActions.RenameSelected(Selection.gameObjects, prefix, includeNumbering);
            }

            GUILayout.Space(10);
            GUILayout.Label("Hierarchy Management", EditorStyles.boldLabel);

            // Clear and remove buttons with red color
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button(new GUIContent("Clear Children", EditorGUIUtility.IconContent("d_TreeEditor.Trash").image), GUILayout.Width(ButtonWidth), GUILayout.Height(ButtonHeight)))
            {
                GameObjectActions.ClearChildren(Selection.activeGameObject);
            }

            if (GUILayout.Button(new GUIContent("Remove Selected", EditorGUIUtility.IconContent("d_TreeEditor.Trash").image), GUILayout.Width(ButtonWidth), GUILayout.Height(ButtonHeight)))
            {
                GameObjectActions.RemoveSelected(Selection.gameObjects);
            }

            // Duplicate button with green color
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button(new GUIContent("Duplicate Selected", EditorGUIUtility.IconContent("d_PrefabOverlayAdded Icon").image), GUILayout.Width(ButtonWidth), GUILayout.Height(ButtonHeight)))
            {
                GameObjectActions.DuplicateSelected(Selection.gameObjects);
            }

            // Set same parent and unparent buttons with distinct colors
            GUI.backgroundColor = Color.yellow;
            if (GUILayout.Button(new GUIContent("Set Same Parent", EditorGUIUtility.IconContent("d_Linked").image), GUILayout.Width(ButtonWidth), GUILayout.Height(ButtonHeight)))
            {
                GameObjectActions.SetSameParent(Selection.gameObjects);
            }

            GUI.backgroundColor = Color.cyan;
            if (GUILayout.Button(new GUIContent("Unparent Selected", EditorGUIUtility.IconContent("d_Unlink").image), GUILayout.Width(ButtonWidth), GUILayout.Height(ButtonHeight)))
            {
                GameObjectActions.UnparentSelected(Selection.gameObjects);
            }

            GUI.backgroundColor = Color.white; // Reset the background color for any other GUI elements
            GUILayout.Space(10);
            GUILayout.Label("All actions include Undo functionality.", EditorStyles.miniLabel);
        }
    }
}

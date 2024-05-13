/* --------------------------------------------------------------------------------
# Created by: Fabian Ramelsberger
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using System.Linq;
using FRHelper;
using UnityEditor;
using UnityEngine;


///<summary>
///ValidationHighlighter description
////	</summary>
[InitializeOnLoad]
public class ValidationHighlighter
{
    private static readonly Color _backgroundColor = new Color(0.7843f, 0.7843f, 0.7843f);
    private static readonly Color _backgroundProColor = new Color(0.2196f, 0.2196f, 0.2196f);
    private static readonly Color _backgroundSelectedColor = new Color(0.22745f, 0.447f, 0.6902f);
    private static readonly Color _backgroundSelectedProColor = new Color(0.1725f, 0.3647f, 0.5294f);
    
    static ValidationHighlighter()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGUI;
    }

    private static void OnHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        bool isSelected = Selection.instanceIDs.Contains(instanceID);
        GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (obj != null)
        {
            IValidatable validatable = obj.GetComponent<IValidatable>();
            if (validatable != null && validatable.IsValid == false)
            {
                selectionRect.x += 18.5f;
                
                Color backgroundColor = EditorGUIUtility.isProSkin ? _backgroundProColor : _backgroundColor;
                if (isSelected)
                {
                    backgroundColor = EditorGUIUtility.isProSkin ? _backgroundSelectedProColor : _backgroundSelectedColor;
                }
                
                GUIStyle hierarchyTextStyle = new GUIStyle(GUI.skin.label);
                hierarchyTextStyle.normal.textColor = Color.red;
                
                float with = hierarchyTextStyle.CalcSize(new GUIContent(obj.name)).x;
                Rect backgroundRect = selectionRect;
                backgroundRect.width = with;
                EditorGUI.DrawRect(backgroundRect, backgroundColor);

                
                EditorGUI.LabelField(selectionRect, obj.name, hierarchyTextStyle);
                
                EditorApplication.RepaintHierarchyWindow();
            }
            
        }
    }
}

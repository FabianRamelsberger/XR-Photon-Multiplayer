/* --------------------------------------------------------------------------------
# Created by: Fabian Ramelsberger
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using UnityEditor;
using UnityEngine;

///<summary>
///HorizontalLineDrawer description
///	</summary>

[CustomPropertyDrawer(typeof(HorizontalLineAttribute))]
public class HorizontalLineDrawer : DecoratorDrawer
{
    public override float GetHeight()
    {
        HorizontalLineAttribute attr = attribute as HorizontalLineAttribute;
        return Mathf.Max(attr.Padding, attr.Thickness);
    }

    public override void OnGUI(Rect position)
    {
        HorizontalLineAttribute attr = attribute as HorizontalLineAttribute;

        position.height = attr.Thickness;
        position.y += attr.Padding * 0.5f;

        Color backgroundColor = EditorGUIUtility.isProSkin ? 
            new Color(0.3f, 0.3f, 0.3f, 1) : 
            new Color(0.7f, 0.7f, 0.7f, 1);
        EditorGUI.DrawRect(position, backgroundColor);
        
    }
}

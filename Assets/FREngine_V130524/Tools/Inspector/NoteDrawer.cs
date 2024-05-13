/* --------------------------------------------------------------------------------
# Created by: Fabian Ramelsberger
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using UnityEditor;
using UnityEngine;
///<summary>
///NoteDrawer description
////	</summary>
[CustomPropertyDrawer(typeof(NoteAttribute))]
public class NoteDrawer : DecoratorDrawer
{
    private const float _padding = 20f;
    private float _height = 0f;
    
    public override float GetHeight()
    {
        NoteAttribute noteAttribute = attribute as NoteAttribute;

        GUIStyle style = EditorStyles.helpBox;
        style.alignment = TextAnchor.MiddleLeft;
        style.wordWrap = true;
        style.padding = new RectOffset(10, 10, 10, 10);
        style.fontSize = 12;

        _height = style.CalcHeight(new GUIContent(noteAttribute.Text), Screen.width);
        return _height + _padding;
    }

    public override void OnGUI(Rect position)
    {
        NoteAttribute noteAttribute = attribute as NoteAttribute;

        position.height = _height;
        position.y += _padding*0.5f;
        
        EditorGUI.HelpBox(position, noteAttribute.Text, MessageType.None);
    }
}

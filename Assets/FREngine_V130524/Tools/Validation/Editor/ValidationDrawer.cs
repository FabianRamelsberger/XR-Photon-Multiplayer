/* --------------------------------------------------------------------------------
# Created by: Fabian Ramelsberger
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using UnityEditor;
using UnityEngine;

///<summary>
///ValidationDrawer description
///</summary>
///
[CustomPropertyDrawer(typeof(ValidationAttribute))]
public class ValidationDrawer : PropertyDrawer
{
   private const int _boxPadding = 10;
   private const float _padding = 10f;
   private const float _offset = 20f;

   private float _height = 10f;
   private float _helpBoxHeight = 0f;
   private static readonly Color _errorBackgroundColor = new Color(1f,.2f,.2f, 0.1f);

   public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
   {
      if (property.objectReferenceValue == null)
      {
         ValidationAttribute validationAttribute = attribute as ValidationAttribute;

         GUIStyle style = EditorStyles.helpBox;
         style.alignment = TextAnchor.MiddleLeft;
         style.wordWrap = true;
         style.padding = new RectOffset(_boxPadding, _boxPadding, _boxPadding, _boxPadding);
         style.fontSize = 12;

         _helpBoxHeight = style.CalcHeight(new GUIContent(validationAttribute.Text), Screen.width);

         _height = _helpBoxHeight + base.GetPropertyHeight(property, label) + _offset;

         return _height;
      }
      else
      {
         return base.GetPropertyHeight(property, label);
      }
   }

   public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
   {
      if (property.objectReferenceValue == null)
      {
         ValidationAttribute validationAttribute = attribute as ValidationAttribute;
         
         position.height = _helpBoxHeight;
         position.y += _padding* .5f;
         EditorGUI.HelpBox(position, validationAttribute.Text, MessageType.Error);

         position.height = _height;
         EditorGUI.DrawRect(position, _errorBackgroundColor);

         position.y += _helpBoxHeight + _padding;
         position.height = base.GetPropertyHeight(property, label);
         EditorGUI.PropertyField(position, property, new GUIContent(property.displayName));
      }
      else
      {
         EditorGUI.PropertyField(position, property, new GUIContent(property.displayName));
      }
   }
}

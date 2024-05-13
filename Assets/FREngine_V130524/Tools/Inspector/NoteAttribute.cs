/* --------------------------------------------------------------------------------
# Created by: Fabian Ramelsberger
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using Unity.Properties;
using UnityEngine;


///<summary>
///NotAttribute description
////	</summary>
[System.AttributeUsage(System.AttributeTargets.Field)]
public class NoteAttribute : PropertyAttribute
{
    public string Text = string.Empty;

    public NoteAttribute(string text)
    {
        Text = text;
    }
}

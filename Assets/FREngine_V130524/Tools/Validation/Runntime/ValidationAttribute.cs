/* --------------------------------------------------------------------------------
# Created by: Fabian Ramelsberger
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using UnityEngine;


///<summary>
///ValidationAttribute description
////	</summary>
public class ValidationAttribute: PropertyAttribute
{
    public string Text = string.Empty;

    public ValidationAttribute(string text)
    {
        Text = text;
    }
}

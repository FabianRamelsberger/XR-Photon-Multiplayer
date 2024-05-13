/* --------------------------------------------------------------------------------
# Created by: Fabian Ramelsberger
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using UnityEngine;

///<summary>
///ColorRandomizer description
////	</summary>
public class ColorRandomizer : MonoBehaviour {

    [SerializeField] private Material _material;
    
    private readonly Color _defaultColor = Color.red;

    public void ChangeColor() {
        _material.color = GetColor(_material.color);
    }

    //We'll use this method to get a color from the server later
    private Color GetColor(Color currentColor) {
        return _defaultColor;
    }
}

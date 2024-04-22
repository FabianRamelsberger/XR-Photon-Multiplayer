/* --------------------------------------------------------------------------------
# Created by: Fabian Ramelsberger
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using System;
using UnityEngine;

//<summary>
//ColorManager description
//	</summary>
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.Collections;
using Random = UnityEngine.Random;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    [Serializable]
    public struct TogglesWithImages
    {
        public Toggle toggle;
        public Image toggleColorImage;
    }
    [SerializeField] private List<TogglesWithImages> _toggleImages;
    [SerializeField] private Button _joinButton;

    public RigSelection RigSelection => _rigSelection;
    [SerializeField] private RigSelection _rigSelection;
    public Color SelectedColor => _selectedColor;
    [SerializeField, ReadOnly] private Color _selectedColor;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else 
            Destroy(this);
    }

    void Start()
    {
        _joinButton.interactable = false;
        RegisterToggles();
    }

    private void RegisterToggles()
    {
        foreach (var toggleImage in _toggleImages)
        {
            toggleImage.toggle.onValueChanged.AddListener((isOn) => 
                SetColorEnableJoin(toggleImage));
        }
    }

    private void SetColorEnableJoin(TogglesWithImages toggleImage)
    {
        Image image = toggleImage.toggleColorImage;
        _selectedColor = image.color;
        _joinButton.interactable = true;
    }
}


/* --------------------------------------------------------------------------------
# Script Name: FREngine Component
# Created by: Fabian Ramelsberger
# Part of: FREngine
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using System;
using System.Net.Mime;
using FREngine.Events;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class OnChangeImageFillAmountEmiter : GenericIntegerEventEmitter
{
    
    private float lastFillAmount;
    private Image _targetImage;
    // Start is called before the first frame update
    void Start()
    {
        if (_targetImage == null)
            _targetImage = GetComponent<Image>();

        if (_targetImage != null)
            lastFillAmount = _targetImage.fillAmount;
    }

    void Update()
    {
        if (_targetImage.fillAmount != lastFillAmount)
        {
            double fillAmount = Math.Round(_targetImage.fillAmount * 100);
            int intAmount = (int)fillAmount;
            Emit(intAmount);
            lastFillAmount = _targetImage.fillAmount;
        }
    }
}

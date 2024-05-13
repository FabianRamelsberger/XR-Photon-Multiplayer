/* --------------------------------------------------------------------------------
# Created by: Fabian Ramelsberger
# Part of: FREngine
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace FREngine.Events
{
    public class OnButtonClickEventEmitter: GenericEventEmitter
    {
        [FormerlySerializedAs("_button")]
        [InfoBox(
            "If this list is empty it will try to get the button component on the gameObject.\n")]
        [SerializeField]
        private List<Button> _buttonList = new ();
        
        private void Start()
        {
            if (_buttonList.Count == 0)
            {
                _buttonList.Add(GetComponent<Button>());
            }
            _buttonList.ForEach(button =>
            {
                button.onClick.AddListener(OnClickEvent);
            });
        }

        private void OnClickEvent()
        {
            Emit();
        }
    }
}

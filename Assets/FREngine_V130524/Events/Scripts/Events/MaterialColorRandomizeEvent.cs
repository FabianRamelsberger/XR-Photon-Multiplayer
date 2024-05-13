/* --------------------------------------------------------------------------------
# Script Name: FREngine Component
# Created by: Fabian Ramelsberger
# Part of: FREngine
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using UnityEngine;

namespace FREngine.Events
{
    public class MaterialColorRandomizeEvent : IEvent
    {
        [SerializeField] private Material _material;
        private readonly Color _defaultColor = Color.red;

        public void Execute(Transform emitter)
        {
            ChangeColor() ;
        }

        private void ChangeColor() {
            _material.color = GetColor(_material.color);
        }

        private Color GetColor(Color currentColor) {
            return _defaultColor;
        }
    }
}

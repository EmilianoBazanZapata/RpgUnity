using Game.Shared.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Scripts
{
    public class HealthBarUI : MonoBehaviour
    {
        private Entity _entity;
        private CharacterStats _characterStats;
        private RectTransform _transform;
        private Slider _slider;

        private bool _initialized;

        private void Update()
        {
            _transform = GetComponent<RectTransform>();
            _entity = GetComponentInParent<Entity>();
            _slider = GetComponentInChildren<Slider>();
            _characterStats = GetComponentInParent<CharacterStats>();
            
            if (_characterStats != null)
                _characterStats.OnHealthChanged += UpdateHealthUI;
            
            UpdateHealthUI();
            
            _initialized = true;
        }

        private void UpdateHealthUI()
        {
            if (_slider == null || _characterStats == null) return;
            
            _slider.maxValue = _characterStats.GetMaxHealthValue();
            _slider.value = _characterStats.GetHealth();
        }

        public void ResetHealtUIValue()
        {
            if (_slider == null || _characterStats == null) return;

            _slider.maxValue = _characterStats.GetMaxHealthValue();
            _slider.value = _characterStats.GetMaxHealthValue();
        }
    }
}
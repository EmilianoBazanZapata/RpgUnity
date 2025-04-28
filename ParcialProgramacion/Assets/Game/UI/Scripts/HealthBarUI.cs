using Game.Shared.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Scripts
{
    public class HealthBarUI: MonoBehaviour
    {
        private Entity _entity;
        private CharacterStats _characterStats;
        private RectTransform _transform;
        private Slider _slider;

        private void Start()
        {
            _transform = GetComponent<RectTransform>();
            _entity = GetComponentInParent<Entity>();
            _slider = GetComponentInChildren<Slider>();
            _characterStats = GetComponentInParent<CharacterStats>();

            _entity.OnFlipped += FlipUI;

            _characterStats._onHealthChanged += UpdateHealthUI;

            UpdateHealthUI();
        }

        private void UpdateHealthUI()
        {
            _slider.maxValue = _characterStats.GetMaxHealthValue();
            _slider.value = _characterStats.GetHealt();
        }

        private void FlipUI() => _transform.Rotate(0, 180, 0);

        private void OnDisable()
        {
            _entity.OnFlipped -= FlipUI;
            _characterStats._onHealthChanged -= UpdateHealthUI;
        }

    }
}
using System;
using Game.Shared.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Shared.Scripts
{
    public class CharacterStats : MonoBehaviour
    {
        #region Serialized Stats

        [Header("Major Stats")]
        public Stat strength;
        public Stat agility;
        public Stat vitality;

        [Header("Offensive Stats")]
        public Stat damage;
        public Stat critChance;
        public Stat critPower;

        [Header("Defensive Stats")]
        public Stat maxHealth;
        public Stat armor;
        public Stat evasion;

        #endregion

        #region Health Management

        [SerializeField] private int _currentHealth;
        public Action OnHealthChanged;

        public bool IsDead { get; private set; }

        #endregion

        #region Elemental Damage (Future Extensions)

        private float _igniteDamageTimer;
        private float _igniteDamageCooldown = 0.3f;
        private int _igniteDamage;
        private int _shockDamage;

        #endregion

        #region Unity Methods

        protected virtual void Start()
        {
            critPower.SetDefaultValue(150);
            _currentHealth = GetMaxHealthValue();
        }

        protected virtual void Update()
        {
            if (_currentHealth < 0 && !IsDead)
                Die();
        }

        #endregion

        #region Damage Logic

        public virtual void DoDamage(CharacterStats targetStats)
        {
            if (TargetCanAvoidAttack(targetStats)) return;

            var totalDamage = damage.GetValue() + strength.GetValue();

            if (CanCrit())
                totalDamage = CalculateCriticalDamage(totalDamage);

            totalDamage = CheckTargetArmor(targetStats, totalDamage);

            targetStats.TakeDamage(totalDamage);
        }

        private bool TargetCanAvoidAttack(CharacterStats targetStats)
        {
            var totalEvasion = targetStats.evasion.GetValue() + targetStats.agility.GetValue();
            return Random.Range(0, 100) < totalEvasion;
        }

        private int CheckTargetArmor(CharacterStats targetStats, int damage)
        {
            damage -= targetStats.armor.GetValue();
            return Mathf.Clamp(damage, 0, int.MaxValue);
        }

        private int CalculateCriticalDamage(int baseDamage)
        {
            var critMultiplier = (critPower.GetValue() + strength.GetValue()) * 0.01f;
            return Mathf.RoundToInt(baseDamage * critMultiplier);
        }

        private bool CanCrit() => Random.Range(0, 100) <= (critChance.GetValue() + agility.GetValue());

        #endregion

        #region Health Modifiers

        public virtual void TakeDamage(int damage)
        {
            DecreaseHealthBy(damage);

            GetComponent<Entity>().DamageImpact();

            if (_currentHealth <= 0 && !IsDead)
                Die();
        }

        protected virtual void DecreaseHealthBy(int damage)
        {
            _currentHealth -= damage;
            OnHealthChanged?.Invoke();
        }

        public virtual void IncreaseHealthBy(int amount)
        {
            _currentHealth += amount;
            _currentHealth = Mathf.Min(_currentHealth, GetMaxHealthValue());
            OnHealthChanged?.Invoke();
        }

        public void ResetHealth()
        {
            _currentHealth = GetMaxHealthValue();
            IsDead = false;
            OnHealthChanged?.Invoke();
        }

        protected virtual void Die()
        {
            IsDead = true;
        }

        public int GetMaxHealthValue() => maxHealth.GetValue() + vitality.GetValue() * 5;

        public float GetHealth() => _currentHealth;

        #endregion

        #region Utility

        public Stat GetStat(StatType statType)
        {
            return statType switch
            {
                StatType.strength => strength,
                StatType.agility => agility,
                StatType.vitality => vitality,
                StatType.damage => damage,
                StatType.critChance => critChance,
                StatType.critPower => critPower,
                StatType.health => maxHealth,
                StatType.armor => armor,
                StatType.evasion => evasion,
                _ => null
            };
        }

        #endregion
        
    }
}

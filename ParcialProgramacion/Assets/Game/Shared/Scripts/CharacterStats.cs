using System;
using Game.Enemies;
using Game.Shared.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Shared.Scripts
{
    public class CharacterStats : MonoBehaviour
    {
        [Header("Major stats")] public Stat strength;
        public Stat agility;
        public Stat vitality;

        [Header("Offensive stats")] public Stat damage;
        public Stat critChance;
        public Stat critPower;

        [Header("Defensive stats")] public Stat maxHealth;
        public Stat armor;
        public Stat evasion;

        public bool isDead { get; private set; }

        [SerializeField] private int _currentHealth;
        public Action _onHealthChanged;
        private float _igniteDamageCoolDown = .3f;
        private float _igniteDamageTimer;
        private int _igniteDamage;
        private int _shockDamage;

        protected virtual void Update()
        {
            if (_currentHealth < 0 && !isDead)
            {
                Debug.Log("Dead");
                Die();
            }
        }

        protected virtual void Start()
        {
            critPower.SetDefaultValue(150);
            _currentHealth = GetMaxHealthValue();
        }

        public virtual void DoDamage(CharacterStats targetStats)
        {
            if (TargetCanAvoidAttack(targetStats))
                return;

            var totalDamage = damage.GetValue() + strength.GetValue();
            
            if (CanCrit())
                totalDamage = CalculateCriticalDamage(totalDamage);

            totalDamage = CheckTargetArmor(targetStats, totalDamage);

            targetStats.TakeDamage(totalDamage);
        }

        private void HitNearestTargetWithShockStrike()
        {
            var colliders = Physics2D.OverlapCircleAll(transform.position, 25);

            var closestDistance = Mathf.Infinity;
            Transform closestEnemy = null;

            foreach (var hit in colliders)
            {
                if (hit.GetComponent<Enemy>() != null &&
                    Vector2.Distance(transform.position, hit.transform.position) > 1)
                {
                    var distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                    if (distanceToEnemy < closestDistance)
                    {
                        closestDistance = distanceToEnemy;
                        closestEnemy = hit.transform;
                    }
                }

                if (closestEnemy == null)
                    closestEnemy = transform;
            }
        }

        private int CheckTargetArmor(CharacterStats targetStats, int totalDamage)
        {
            totalDamage -= targetStats.armor.GetValue();

            totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);

            return totalDamage;
        }

        private bool TargetCanAvoidAttack(CharacterStats targetStats)
        {
            var totalEvasion = targetStats.evasion.GetValue() + targetStats.agility.GetValue();

            return Random.Range(0, 100) < totalEvasion;
        }

        public virtual void TakeDamage(int damage)
        {
            DecreaseHealthBy(damage);

            GetComponent<Entity>().DamageImpact();

            if (_currentHealth <= 0 && !isDead)
                Die();
        }

        protected virtual void DecreaseHealthBy(int damage)
        {
            _currentHealth -= damage;

            if (_onHealthChanged != null)
                _onHealthChanged();
        }

        public virtual void IncreaseHealthBy(int amount)
        {
            _currentHealth += amount;

            if (_currentHealth > GetMaxHealthValue())
                _currentHealth = GetMaxHealthValue();

            if (_onHealthChanged != null)
                _onHealthChanged();
        }

        private int CalculateCriticalDamage(int damage) =>
            Mathf.RoundToInt(damage * (critPower.GetValue() + strength.GetValue()) * .01f);

        protected virtual void Die() => isDead = true;
        private bool CanCrit() => Random.Range(0, 100) <= critChance.GetValue() + agility.GetValue();
        public int GetMaxHealthValue() => maxHealth.GetValue() + vitality.GetValue() * 5;
        public float GetHealt() => _currentHealth;

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
                _ => null
            };
        }
    }
}
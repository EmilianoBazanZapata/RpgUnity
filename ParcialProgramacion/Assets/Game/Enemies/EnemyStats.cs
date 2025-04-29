using System;
using Game.Shared.Scripts;
using UnityEngine;

namespace Game.Enemies
{
    public class EnemyStats: CharacterStats
    {
        private Enemy _enemy;
        private ItemDrop _itemDrop;

        [Header("Level details")]
        [SerializeField] private int _level = 1;
        [Range(0f, 1f)] 
        [SerializeField] private float _percantageModifier = .4f;

        protected override void Start()
        {
            ApplyModifiers();

            base.Start();
        
            _enemy = GetComponent<Enemy>();
            _itemDrop = GetComponent<ItemDrop>();
        }

        private void ApplyModifiers()
        {
            Modify(strength);
            Modify(agility);
            Modify(vitality);
        
            Modify(damage);
            Modify(critChance);
            Modify(critPower);
        
            Modify(maxHealth);
            Modify(armor);
            Modify(evasion);
        }

        private void Modify(Stat stat)
        {
            for (int i = 0; i < _level; i++)
            {
                var modifier = stat.GetValue() * _percantageModifier;
                stat.AddModifier((int)modifier);
            }
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);

            _enemy.DamageImpact();
        }

        protected override void Die()
        {
            base.Die();
            _enemy.Die();
            //_itemDrop.GenerateDrop();
        }
    }
}
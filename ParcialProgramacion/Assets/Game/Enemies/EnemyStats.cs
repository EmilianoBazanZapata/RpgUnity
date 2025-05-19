using Game.Shared.Scripts;
using UnityEngine;

namespace Game.Enemies
{
    /// <summary>
    /// Controla las estadísticas del enemigo y su comportamiento al recibir daño y morir.
    /// </summary>
    public class EnemyStats : CharacterStats
    {
        #region Serialized Fields

        [Header("Detalles del nivel")]
        [SerializeField] private int _level = 1;

        [Range(0f, 1f)]
        [SerializeField] private float _percentageModifier = 0.4f;

        #endregion

        #region Private Fields

        private Enemy _enemy;
        private ItemDrop _itemDrop;

        #endregion

        #region Unity Methods

        protected override void Start()
        {
            base.Start();
            _enemy = GetComponent<Enemy>();
            _itemDrop = GetComponent<ItemDrop>();

            ApplyModifiers();
        }

        #endregion

        #region Stats Modifiers

        /// <summary>
        /// Aplica modificadores a las estadísticas del enemigo según su nivel.
        /// </summary>
        private void ApplyModifiers()
        {
            ModifyStat(strength);
            ModifyStat(agility);
            ModifyStat(vitality);

            ModifyStat(damage);
            ModifyStat(critChance);
            ModifyStat(critPower);

            ModifyStat(maxHealth);
            ModifyStat(armor);
            ModifyStat(evasion);
        }

        private void ModifyStat(Stat stat)
        {
            for (int i = 0; i < _level; i++)
            {
                float modifier = stat.GetValue() * _percentageModifier;
                stat.AddModifier((int)modifier);
            }
        }

        #endregion

        #region Combat Logic

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            _enemy?.DamageImpact();
        }

        protected override void Die()
        {
            base.Die();

            _enemy?.Die();
            _itemDrop?.GenerateDrop();
        }

        #endregion
    }
}

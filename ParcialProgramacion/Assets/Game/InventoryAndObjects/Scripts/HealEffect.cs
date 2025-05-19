using Game.Character.Scripts;
using Game.InventoryAndObjects.ScriptableObjects;
using UnityEngine;

namespace Game.InventoryAndObjects.Scripts
{
    [CreateAssetMenu(fileName = "Item Effect", menuName = "Data/ItemEffect/Heal Effect")]
    public class HealEffect : ItemEffect
    {
        [Header("Configuración de curación")]
        [Tooltip("Porcentaje de la vida máxima que se curará (0.0 a 1.0)")]
        [Range(0f, 1f)]
        [SerializeField] private float _healPercent = 0.25f;

        public override void ExecuteEffect(Transform enemyPosition)
        {
            var player = Player.Instance;
            if (player == null) return;

            var playerStats = player.GetComponent<PlayerStats>();
            if (playerStats == null) return;

            var maxHealth = playerStats.GetMaxHealthValue();
            var healAmount = Mathf.RoundToInt(maxHealth * _healPercent);

            playerStats.IncreaseHealthBy(healAmount);
        }
    }
}
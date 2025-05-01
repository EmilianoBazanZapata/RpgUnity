using Game.InventoryAndObjects.ScriptableObjects;
using Game.Player.Scripts;
using Game.Player.Scripts.Managers;
using UnityEngine;

namespace Game.InventoryAndObjects.Scripts
{
    [CreateAssetMenu(fileName = "Item Effect", menuName = "Data/ItemEffect/Heal Effect")]
    public class HealEffect : ItemEffect
    {
        [Range(0f, 1f)] [SerializeField] private float _healPercent;

        public override void ExecuteEffect(Transform enemyPosition)
        {
            var playerStats = PlayerManager.Instance.Player.GetComponent<PlayerStats>();

            var healAmount = Mathf.RoundToInt(playerStats.GetMaxHealthValue() * _healPercent);
        
            playerStats.IncreaseHealthBy(healAmount);
        }
    }
}
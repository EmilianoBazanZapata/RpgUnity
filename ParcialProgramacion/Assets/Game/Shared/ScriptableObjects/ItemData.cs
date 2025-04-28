using System.Text;
using Game.Shared.Enums;
using UnityEngine;

namespace Game.Shared.ScriptableObjects
{
    [CreateAssetMenu(fileName = "new Item Data", menuName = "Data/Menu")]
    public class ItemData : ScriptableObject
    {
        public ItemType itemType;
        public string itemName;
        public Sprite icon;
    
        [Range(0, 100)]
        public float dropChance;
        protected StringBuilder StringBuilder = new StringBuilder();

        public virtual string GetDescription()
        {
            return "";
        }
    }
}
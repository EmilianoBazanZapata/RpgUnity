using System;
using Game.Inventory.ScriptableObjects;

namespace Game.Inventory.Scripts
{
    [Serializable]
    public class InventoryItem
    {
        public ItemData itemData;
        public int stackSize;

        public InventoryItem(ItemData newItemData)
        {
            itemData = newItemData;
            AddStack();
        }

        public void AddStack() => stackSize++;
        public void RemoveStack() => stackSize--;
    }
}
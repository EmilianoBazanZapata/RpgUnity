using System;
using Game.InventoryAndObjects.ScriptableObjects;

namespace Game.InventoryAndObjects.Scripts
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
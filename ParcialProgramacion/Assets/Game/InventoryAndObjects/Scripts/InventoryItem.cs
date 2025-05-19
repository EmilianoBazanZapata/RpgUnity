using System;
using Game.InventoryAndObjects.ScriptableObjects;

namespace Game.InventoryAndObjects.Scripts
{
    /// <summary>
    /// Representa un ítem del inventario con su cantidad acumulada (stack).
    /// </summary>
    [Serializable]
    public class InventoryItem
    {
        public ItemData itemData;  // Referencia al ítem en sí
        public int stackSize;      // Cantidad de ítems acumulados

        public InventoryItem(ItemData newItemData)
        {
            itemData = newItemData;
            AddStack(); // Al crear, se empieza con 1 unidad
        }

        /// <summary>
        /// Incrementa en 1 la cantidad del ítem.
        /// </summary>
        public void AddStack() => stackSize++;

        /// <summary>
        /// Disminuye en 1 la cantidad del ítem. Nunca baja de 0.
        /// </summary>
        public void RemoveStack()
        {
            if (stackSize > 0)
                stackSize--;
        }
    }
}
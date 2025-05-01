using System.Collections.Generic;
using Game.InventoryAndObjects.Scripts;
using Game.Shared.Scripts;
using UnityEngine;

namespace Game.Player.Scripts
{
    public class PlayerItemDrop : ItemDrop
    {
        [Header("Player's drop")] 
        [SerializeField] private float _chanceToLoseItems;
        [SerializeField] private float _chanceToLoseMaterials;

        public override void GenerateDrop()
        {
            var inventory = Inventory.instance;

            var itemsToUnequip = new List<InventoryItem>();
            var materialsToLose = new List<InventoryItem>();

            foreach (var item in inventory.GetEquipmentList())
            {
                if (!(Random.Range(0, 100) <= _chanceToLoseItems)) continue;
                DropItem(item.itemData);
                itemsToUnequip.Add(item);
            }

            foreach (var item in itemsToUnequip)
            {
                inventory.UnequipItem(item.itemData as ItemDataEquipment);
            }

            foreach (var item in inventory.GetStashList())
            {
                if (!(Random.Range(0, 100) <= _chanceToLoseMaterials)) continue;
                DropItem(item.itemData);
                materialsToLose.Add(item);
            }

            // foreach (var item in materialsToLose)
            // {
            //     inventory.RemoveItem(item.itemData);
            // }
        }
    }
}
using Game.InventoryAndObjects.Scripts;
using Game.Shared.Enums;
using UnityEngine.EventSystems;

namespace Game.UI.Scripts
{
    public class EquipmentSlot: ItemSlot
    {
        public EquipmentType slotType;

        private void OnValidate()
        {
            gameObject.name = "Equipment Slot - " + slotType;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (item == null || item.itemData == null)
                return;

            Inventory.instance.UnequipItem(item.itemData as ItemDataEquipment);
            Inventory.instance.AddItem(item.itemData as ItemDataEquipment);
        
            ui.itemToolTip.HideToolTip();
        
            CleanUpSlot();
        }
    }
}
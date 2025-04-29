using Game.InventoryAndObjects.Scripts;
using TMPro;
using UnityEngine;

namespace Game.UI.Scripts
{
    public class ItemTooltip : ToolTip
    {
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private TextMeshProUGUI itemTypeText;
        [SerializeField] private TextMeshProUGUI itemDescription;

        [SerializeField] private int defaultFontSize = 32;

        public void ShowToolTip(ItemDataEquipment item)
        {
            if (item == null)
                return;

            itemNameText.text = item.itemName;
            itemTypeText.text = item.equipmentType.ToString();
            itemDescription.text = item.GetDescription();

            AdjustFontSize(itemNameText);
            AdjustPosition();

            gameObject.SetActive(true);
        }

        public void HideToolTip() 
        {
            itemNameText.fontSize = defaultFontSize;
            gameObject.SetActive(false);
        }
    }
}
using Game.InventoryAndObjects.Scripts;
using Game.Shared.Enums;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace Game.UI.Scripts
{
    public class ItemSlot: MonoBehaviour , IPointerDownHandler ,IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;

    protected UserInterface ui;
    public InventoryItem item;

    protected virtual void Start()
    {
        ui = GetComponentInParent<UserInterface>();
    }

    public void UpdateSlot(InventoryItem _newItem)
    {
        item = _newItem;

        itemImage.color = Color.white;

        if (item != null)
        {
            if (itemImage.sprite != item.itemData.icon)
            {
                itemImage.sprite = item.itemData.icon;
            }

            if (item.stackSize > 1)
            {
                if (itemText.text != item.stackSize.ToString())
                {
                    itemText.text = item.stackSize.ToString();
                }
            }
            else
            {
                if (itemText.text != "")
                {
                    itemText.text = "";
                }
            }
        }
    }


    public void CleanUpSlot()
    {
        item = null;

        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null)
            return;

        ui.itemToolTip.HideToolTip();

        if (Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(item.itemData);
            return;
        }

        if (item.itemData.itemType == ItemType.Equipment)
            Inventory.instance.EquipItem(item.itemData);

    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null)
            return;
        
        ui.itemToolTip.ShowToolTip(item.itemData as ItemDataEquipment);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null)
            return;

        ui.itemToolTip.HideToolTip();
    }
}
}
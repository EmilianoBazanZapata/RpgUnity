using System;
using System.Collections.Generic;
using Game.InventoryAndObjects.ScriptableObjects;
using Game.Managers;
using Game.Shared.Enums;
using Game.UI.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.InventoryAndObjects.Scripts
{
    public class Inventory: MonoBehaviour
{
    [FormerlySerializedAs("startingEquipment")] public List<ItemData> startingItems = new List<ItemData>();
    public static Inventory instance;
    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;
    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashSDictionary;
    public List<InventoryItem> equipment;
    public Dictionary<ItemDataEquipment, InventoryItem> equipmentDictionary;
    
    [Header("Inventory UI")] [SerializeField]
    private Transform _inventorySlotParent;
    [SerializeField] private Transform _stashSlotParent;
    [SerializeField] private Transform _equipmentSlotParent;
    private ItemSlot[] _inventoryItemSlots;
    private ItemSlot[] _stashItemSlots;
    private EquipmentSlot[] _equipmentItemSlots;
    private ItemDataEquipment _oldEquipment;
    [SerializeField] private Transform statSlotParent;

    [Header("items cooldown")] private float _flaskCoolDown;
    private float _lasTimeUsed;
    
    private StatSlot[] statSlot;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();
        _inventoryItemSlots = _inventorySlotParent.GetComponentsInChildren<ItemSlot>();

        stash = new List<InventoryItem>();
        stashSDictionary = new Dictionary<ItemData, InventoryItem>();
        _stashItemSlots = _stashSlotParent.GetComponentsInChildren<ItemSlot>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemDataEquipment, InventoryItem>();

        _equipmentItemSlots = _equipmentSlotParent.GetComponentsInChildren<EquipmentSlot>();
        statSlot = statSlotParent.GetComponentsInChildren<StatSlot>();

        AddStartingitems();
    }

    private void AddStartingitems()
    {
        for (int i = 0; i < startingItems.Count; i++)
        {
            AddItem(startingItems[i]);
        }
    }

    public void EquipItem(ItemData _item)
    {
        var newEquipment = _item as ItemDataEquipment;

        var newItem = new InventoryItem(newEquipment);

        _oldEquipment = null;

        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
                _oldEquipment = item.Key;
        }

        if (_oldEquipment != null)
        {
            UnequipItem(_oldEquipment);
            AddItem(_oldEquipment);
        }

        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers();

        RemoveItem(_item , newItem.stackSize);
    }

    public void UnequipItem(ItemDataEquipment itemToDelete)
    {
        if (!equipmentDictionary.TryGetValue(itemToDelete, out InventoryItem value)) return;

        equipment.Remove(value);
        equipmentDictionary.Remove(itemToDelete);
        itemToDelete.RemoveModifiers();
    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < _equipmentItemSlots.Length; i++)
        {
            foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == _equipmentItemSlots[i].slotType)
                    _equipmentItemSlots[i].UpdateSlot(item.Value);
            }
        }

        for (int i = 0; i < _inventoryItemSlots.Length; i++)
        {
            _inventoryItemSlots[i].CleanUpSlot();
        }

        for (int i = 0; i < _stashItemSlots.Length; i++)
        {
            _stashItemSlots[i].CleanUpSlot();
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            _inventoryItemSlots[i].UpdateSlot(inventory[i]);
        }

        for (int i = 0; i < stash.Count; i++)
        {
            _stashItemSlots[i].UpdateSlot(stash[i]);
        }
        UpdateStatsUI();
    }

    public void AddItem(ItemData item)
    {
        switch (item.itemType)
        {
            case ItemType.Equipment:
                AddToInventory(item);
                break;
            case ItemType.Material:
                AddToStash(item);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        UpdateSlotUI();
    }

    private void AddToStash(ItemData item)
    {
        if (stashSDictionary.TryGetValue(item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            var newItem = new InventoryItem(item);
            stash.Add(newItem);
            stashSDictionary.Add(item, newItem);
        }
    }

    public void AddToInventory(ItemData item)
    {
        if (inventoryDictionary.TryGetValue(item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            var newItem = new InventoryItem(item);
            inventory.Add(newItem);
            inventoryDictionary.Add(item, newItem);
        }
    }

    public void RemoveItem(ItemData item, int amount)
    {
        // INVENTORY
        if (inventoryDictionary.TryGetValue(item, out InventoryItem invItem))
        {
            if (invItem.stackSize <= amount)
            {
                inventory.Remove(invItem);
                inventoryDictionary.Remove(item);
            }
            else
            {
                invItem.stackSize -= amount;
            }
        }

        // STASH
        if (stashSDictionary.TryGetValue(item, out InventoryItem stashItem))
        {
            if (stashItem.stackSize <= amount)
            {
                stash.Remove(stashItem);
                stashSDictionary.Remove(item);
            }
            else
            {
                stashItem.stackSize -= amount;
            }
        }

        UpdateSlotUI();
    }


    public bool CanCraft(ItemDataEquipment itemToCraft, List<InventoryItem> requiredMaterials)
    {
        var materialsToRemove = new List<InventoryItem>();

        for (int i = 0; i < requiredMaterials.Count; i++)
        {
            var required = requiredMaterials[i];

            if (stashSDictionary.TryGetValue(required.itemData, out InventoryItem stashValue))
            {
                if (stashValue.stackSize < required.stackSize)
                {
                    SoundManager.Instance.PlaySound(SoundType.ErrorCraft);

                    Debug.Log("Not enough materials");
                    return false;
                }
                else
                {
                    materialsToRemove.Add(required);
                    SoundManager.Instance.PlaySound(SoundType.Craft);
                }
            }
            else
            {
                Debug.Log("Material not found in stash");
                return false;
            }
        }
        
        for (int i = 0; i < materialsToRemove.Count; i++)
        {
            RemoveItem(materialsToRemove[i].itemData, materialsToRemove[i].stackSize);
        }
        
        AddItem(itemToCraft);
        return true;
    }
    
    
    public List<InventoryItem> GetEquipmentList() => equipment;
    public List<InventoryItem> GetStashList() => stash;

    public ItemDataEquipment GetEquipment(EquipmentType type)
    {
        ItemDataEquipment equipedItem = null;

        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == type)
                equipedItem = item.Key;
        }

        return equipedItem;
    }
    
    public void UseFlask()
    {
        var currentFlask = GetEquipment(EquipmentType.Flask);
        
        if (currentFlask == null) return;
        
        if (Time.time > _lasTimeUsed + currentFlask.itemCoolDown)
        {
            _lasTimeUsed = Time.time;
            currentFlask.ExecuteItemEffect(null);
        }
        else
        {
            Debug.Log("Cooldown");
        }
    }
    
    public void UpdateStatsUI()
    {
        for (int i = 0; i < statSlot.Length; i++)
        {
            statSlot[i].UpdateStatValueUI();
        }
    }
}
}
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
    public class Inventory : MonoBehaviour
    {
        [FormerlySerializedAs("startingEquipment")]
        public List<ItemData> startingItems = new();

        public static Inventory instance;

        public List<InventoryItem> inventory = new();
        public List<InventoryItem> stash = new();
        public List<InventoryItem> equipment = new();

        public Dictionary<ItemData, InventoryItem> inventoryDict = new();
        public Dictionary<ItemData, InventoryItem> stashDict = new();
        public Dictionary<ItemDataEquipment, InventoryItem> equipmentDict = new();

        [Header("Inventory UI")]
        [SerializeField] private Transform _inventorySlotParent;
        [SerializeField] private Transform _stashSlotParent;
        [SerializeField] private Transform _equipmentSlotParent;
        [SerializeField] private Transform _statSlotParent;

        private ItemSlot[] _inventorySlots;
        private ItemSlot[] _stashSlots;
        private EquipmentSlot[] _equipmentSlots;
        private StatSlot[] _statSlots;

        private ItemDataEquipment _oldEquipment;

        [Header("Items cooldown")]
        private float _lastTimeUsed;

        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            _inventorySlots = _inventorySlotParent.GetComponentsInChildren<ItemSlot>();
            _stashSlots = _stashSlotParent.GetComponentsInChildren<ItemSlot>();
            _equipmentSlots = _equipmentSlotParent.GetComponentsInChildren<EquipmentSlot>();
            _statSlots = _statSlotParent.GetComponentsInChildren<StatSlot>();

            foreach (var item in startingItems)
                AddItem(item);
        }

        public void AddItem(ItemData item)
        {
            if (item == null) return;

            switch (item.itemType)
            {
                case ItemType.Equipment:
                    AddToDictionary(item, inventory, inventoryDict);
                    break;
                case ItemType.Material:
                    AddToDictionary(item, stash, stashDict);
                    break;
            }

            UpdateSlotUI();
        }

        private void AddToDictionary(ItemData item, List<InventoryItem> list, Dictionary<ItemData, InventoryItem> dict)
        {
            if (dict.TryGetValue(item, out var entry))
                entry.AddStack();
            else
            {
                var newItem = new InventoryItem(item);
                list.Add(newItem);
                dict[item] = newItem;
            }
        }

        public void RemoveItem(ItemData item, int amount)
        {
            RemoveFromDictionary(item, amount, inventory, inventoryDict);
            RemoveFromDictionary(item, amount, stash, stashDict);
            UpdateSlotUI();
        }

        private void RemoveFromDictionary(ItemData item, int amount, List<InventoryItem> list, Dictionary<ItemData, InventoryItem> dict)
        {
            if (!dict.TryGetValue(item, out var entry)) return;

            if (entry.stackSize <= amount)
            {
                list.Remove(entry);
                dict.Remove(item);
            }
            else
                entry.stackSize -= amount;
        }

        public void EquipItem(ItemData item)
        {
            if (item is not ItemDataEquipment equipData) return;

            var newItem = new InventoryItem(equipData);
            _oldEquipment = null;

            foreach (var eq in equipmentDict)
            {
                if (eq.Key.equipmentType == equipData.equipmentType)
                    _oldEquipment = eq.Key;
            }

            if (_oldEquipment != null)
            {
                UnequipItem(_oldEquipment);
                AddItem(_oldEquipment);
            }

            equipment.Add(newItem);
            equipmentDict[equipData] = newItem;
            equipData.AddModifiers();
            RemoveItem(item, newItem.stackSize);
        }

        public void UnequipItem(ItemDataEquipment item)
        {
            if (!equipmentDict.TryGetValue(item, out var entry)) return;

            equipment.Remove(entry);
            equipmentDict.Remove(item);
            item.RemoveModifiers();
        }

        private void UpdateSlotUI()
        {
            foreach (var slot in _equipmentSlots)
                slot.CleanUpSlot();

            foreach (var item in equipmentDict)
            {
                foreach (var slot in _equipmentSlots)
                {
                    if (item.Key.equipmentType == slot.slotType)
                        slot.UpdateSlot(item.Value);
                }
            }

            foreach (var slot in _inventorySlots) slot.CleanUpSlot();
            foreach (var slot in _stashSlots) slot.CleanUpSlot();

            for (int i = 0; i < inventory.Count; i++)
                _inventorySlots[i].UpdateSlot(inventory[i]);

            for (int i = 0; i < stash.Count; i++)
                _stashSlots[i].UpdateSlot(stash[i]);

            foreach (var slot in _statSlots)
                slot.UpdateStatValueUI();
        }

        public bool CanCraft(ItemDataEquipment itemToCraft, List<InventoryItem> requiredMaterials)
        {
            var materialsToRemove = new List<InventoryItem>();

            foreach (var required in requiredMaterials)
            {
                if (!stashDict.TryGetValue(required.itemData, out var stashEntry) || stashEntry.stackSize < required.stackSize)
                {
                    SoundManager.Instance.PlaySound(SoundType.ErrorCraft);
                    Debug.Log("Not enough materials");
                    return false;
                }

                materialsToRemove.Add(required);
            }

            foreach (var item in materialsToRemove)
                RemoveItem(item.itemData, item.stackSize);

            AddItem(itemToCraft);
            SoundManager.Instance.PlaySound(SoundType.Craft);
            return true;
        }

        public void UseFlask()
        {
            var flask = GetEquipment(EquipmentType.Flask);
            if (flask == null) return;

            if (Time.time > _lastTimeUsed + flask.itemCoolDown)
            {
                _lastTimeUsed = Time.time;
                flask.ExecuteItemEffect(null);
            }
            else
            {
                Debug.Log("Cooldown");
            }
        }

        public ItemDataEquipment GetEquipment(EquipmentType type)
        {
            foreach (var item in equipmentDict)
                if (item.Key.equipmentType == type)
                    return item.Key;

            return null;
        }

        public List<InventoryItem> GetEquipmentList() => equipment;
        public List<InventoryItem> GetStashList() => stash;
    }
}

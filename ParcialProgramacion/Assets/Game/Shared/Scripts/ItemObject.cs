using Game.InventoryAndObjects.ScriptableObjects;
using Game.InventoryAndObjects.Scripts;
using UnityEngine;

namespace Game.Shared.Scripts
{
    public class ItemObject: MonoBehaviour
    {
        [SerializeField] private ItemData _itemData;
        [SerializeField] private Rigidbody2D _rigidbody2D;
    
        private void OnValidate()
        {
            SetupVisuals();
        }

        private void SetupVisuals()
        {
            if (_itemData == null)
                return;
        
            GetComponent<SpriteRenderer>().sprite = _itemData.icon;
            gameObject.name = "Item Object - " + _itemData.itemName;
        }

        public void SetupItem(ItemData itemData, Vector2 velocity)
        {
            _itemData = itemData;
            _rigidbody2D.velocity = velocity;
        
            SetupVisuals();
        }

        public void PickUpItem()
        {

            if (Inventory.instance == null)
            {
                Debug.LogError("Inventory instance is null.");
                return;
            }

            Inventory.instance.AddItem(_itemData);
            Destroy(gameObject);
        }
    }
}
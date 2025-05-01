using Game.Shared.Scripts;
using UnityEngine;

namespace Game.InventoryAndObjects.Scripts
{
    public class ItemObjectTrigger : MonoBehaviour
    {
        private ItemObject myItemObject => GetComponentInParent<ItemObject>();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) return;
            
            myItemObject.PickUpItem();
        }
    }
}
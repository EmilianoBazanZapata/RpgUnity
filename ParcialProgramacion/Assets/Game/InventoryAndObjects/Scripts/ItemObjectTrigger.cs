using Game.Shared.Scripts;
using UnityEngine;

namespace Game.InventoryAndObjects.Scripts
{
    /// <summary>
    /// Componente que detecta cuándo el jugador entra en contacto con un objeto recogible.
    /// </summary>
    public class ItemObjectTrigger : MonoBehaviour
    {
        private ItemObject _itemObject;

        private void Awake()
        {
            _itemObject = GetComponentInParent<ItemObject>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) return;
            
            _itemObject.PickUpItem();
        }
    }
}
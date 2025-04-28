using System.Collections.Generic;
using Game.Shared.ScriptableObjects;
using UnityEngine;

namespace Game.Shared.Scripts
{
    public class ItemDrop : MonoBehaviour
    {
        [SerializeField] private ItemData[] possibleDrop;
        [SerializeField] private List<ItemData> _drop = new();
        [SerializeField] private GameObject _dropPrefab;
        [SerializeField] private ItemData _item;
        [SerializeField] private int posibleAmountDrop;

        public virtual void GenerateDrop()
        {
            foreach (var itemData in possibleDrop)
            {
                if (Random.Range(0, 100) <= itemData.dropChance)
                {
                    _drop.Add(itemData);
                }
            }

            for (int i = 0; i < posibleAmountDrop; i++)
            {
                var randomItem = _drop[Random.Range(0, _drop.Count - 1)];

                _drop.Remove(randomItem);
                DropItem(randomItem);
            }
        }

        public void DropItem(ItemData itemData)
        {
            var newDrop = Instantiate(_dropPrefab, transform.position, Quaternion.identity);

            var randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(15, 20));

            newDrop.GetComponent<ItemObject>().SetupItem(itemData, randomVelocity);
        }
    }
}
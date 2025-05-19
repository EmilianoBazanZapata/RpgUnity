using System.Collections.Generic;
using Game.InventoryAndObjects.ScriptableObjects;
using UnityEngine;

namespace Game.Shared.Scripts
{
    /// <summary>
    /// Componente que permite dropear objetos con probabilidad al morir un enemigo u ocurrir un evento.
    /// </summary>
    public class ItemDrop : MonoBehaviour
    {
        #region Serialized Fields

        [Header("Datos de posibles drops")]
        [SerializeField] private ItemData[] _possibleDrop;
        [SerializeField] private int _possibleAmountDrop = 1;

        [Header("Prefab del objeto a instanciar")]
        [SerializeField] private GameObject _dropPrefab;

        [Header("Rango de fuerza inicial del drop")]
        [SerializeField] private Vector2 _dropForceX = new(-5f, 5f);
        [SerializeField] private Vector2 _dropForceY = new(15f, 20f);

        #endregion

        #region Private Fields

        private readonly List<ItemData> _selectedDrops = new();

        #endregion

        #region Public Methods

        /// <summary>
        /// Genera drops aleatorios en base a la probabilidad de cada ítem.
        /// </summary>
        public virtual void GenerateDrop()
        {
            _selectedDrops.Clear();

            foreach (var itemData in _possibleDrop)
            {
                if (Random.Range(0, 100) <= itemData.dropChance)
                    _selectedDrops.Add(itemData);
            }

            for (int i = 0; i < _possibleAmountDrop && _selectedDrops.Count > 0; i++)
            {
                int index = Random.Range(0, _selectedDrops.Count);
                var randomItem = _selectedDrops[index];
                _selectedDrops.RemoveAt(index);

                DropItem(randomItem);
            }
        }

        /// <summary>
        /// Instancia el ítem en el mundo con una fuerza aleatoria.
        /// </summary>
        /// <param name="itemData">El ítem a dropear.</param>
        public void DropItem(ItemData itemData)
        {
            GameObject newDrop = Instantiate(_dropPrefab, transform.position, Quaternion.identity);

            Vector2 randomVelocity = new(
                Random.Range(_dropForceX.x, _dropForceX.y),
                Random.Range(_dropForceY.x, _dropForceY.y));

            if (newDrop.TryGetComponent(out ItemObject itemObject))
                itemObject.SetupItem(itemData, randomVelocity);
        }

        #endregion
    }
}

using System.Text;
using Game.Shared.Enums;
using UnityEngine;

namespace Game.InventoryAndObjects.ScriptableObjects
{
    /// <summary>
    /// Representa los datos base de un ítem del juego.
    /// </summary>
    [CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Menu")]
    public class ItemData : ScriptableObject
    {
        [Header("Datos del ítem")]
        public ItemType itemType;           // Tipo de ítem (consumible, arma, etc.)
        public string itemName;             // Nombre del ítem
        public Sprite icon;                 // Ícono para mostrar en la UI

        [Header("Probabilidad de drop")]
        [Range(0, 100)]
        public float dropChance;            // Probabilidad de aparición del ítem (0-100%)

        // Utilizado para construir la descripción del ítem
        protected StringBuilder _stringBuilder = new StringBuilder();

        /// <summary>
        /// Devuelve la descripción del ítem. Se puede sobreescribir en clases hijas.
        /// </summary>
        public virtual string GetDescription()
        {
            _stringBuilder.Clear();
            _stringBuilder.AppendLine($"Nombre: {itemName}");
            _stringBuilder.AppendLine($"Tipo: {itemType}");
            _stringBuilder.AppendLine($"Chance de drop: {dropChance}%");

            return _stringBuilder.ToString();
        }
    }
}
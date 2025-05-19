using UnityEngine;

namespace Game.InventoryAndObjects.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Item Effect", menuName = "Data/Item Effect/Base")]
    public class ItemEffect : ScriptableObject
    {
        [Header("Descripción del efecto")]
        [Tooltip("Texto descriptivo que aparece en el inventario o tooltip.")]
        [TextArea]
        public string effectDescription;

        /// <summary>
        /// Método virtual que ejecuta el efecto del ítem. Debe ser sobreescrito por clases hijas.
        /// </summary>
        /// <param name="target">Transform del objetivo (jugador, enemigo, etc.)</param>
        public virtual void ExecuteEffect(Transform target)
        {
            Debug.LogWarning($"[ItemEffect] Ejecutando efecto base sin implementación: {name}");
        }
    }
}
using UnityEngine;

namespace Game.InventoryAndObjects.ScriptableObjects
{
    [CreateAssetMenu(fileName = "new Item Data", menuName = "Data/Item Effect")]
    public class ItemEffect : ScriptableObject
    {
        [TextArea]
        public string effectDescription;
    
        public virtual void ExecuteEffect(Transform enemyPosition)
        {
            Debug.Log("Effect executed!");
        }
    }
}
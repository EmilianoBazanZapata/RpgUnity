using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Shared.Scripts
{
    [System.Serializable]
    public class Stat
    {
        [SerializeField] private int _baseValue;

        public List<int> modifiers;

        public int GetValue()
        {
            return _baseValue + modifiers.Sum();
        }

        public void AddModifier(int modifier)
        {
            modifiers.Add(modifier);
        }
    
        public void RemoveModifier(int modifier)
        {
            modifiers.Remove(modifier);
        }

        public void SetDefaultValue(int value)
        {
            _baseValue = value;
        }
    }
}
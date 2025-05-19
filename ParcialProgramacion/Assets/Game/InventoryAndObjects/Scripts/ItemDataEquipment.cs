using System.Collections.Generic;
using Game.InventoryAndObjects.ScriptableObjects;
using Game.Shared.Enums;
using UnityEngine;
using Game.Character.Scripts;

namespace Game.InventoryAndObjects.Scripts
{
    [CreateAssetMenu(fileName = "new Equipment Item Data", menuName = "Data/Equipment")]
    public class ItemDataEquipment : ItemData
    {
        [Header("Tipo de equipo")]
        public EquipmentType equipmentType;
        public ItemEffect[] itemEffects;
        public float itemCoolDown;

        [Header("Atributos principales")]
        public int strength;
        public int agility;
        public int intelligence;
        public int vitality;

        [Header("Atributos ofensivos")]
        public int damage;
        public int critChance;
        public int critPower;

        [Header("Atributos defensivos")]
        public int health;
        public int armor;
        public int evasion;
        public int magicResistance;

        [Header("Atributos mágicos")]
        public int fireDamage;
        public int iceDamage;
        public int lightningDamage;

        [Header("Requisitos de crafteo")]
        public List<InventoryItem> craftingMaterials;

        private int _descriptionLength;

        public void AddModifiers()
        {
            var playerStats = Player.Instance.GetComponent<PlayerStats>();

            playerStats.strength.AddModifier(strength);
            playerStats.agility.AddModifier(agility);
            playerStats.vitality.AddModifier(vitality);

            playerStats.damage.AddModifier(damage);
            playerStats.critChance.AddModifier(critChance);
            playerStats.critPower.AddModifier(critPower);

            playerStats.maxHealth.AddModifier(health);
            playerStats.armor.AddModifier(armor);
            playerStats.evasion.AddModifier(evasion);
        }

        public void RemoveModifiers()
        {
            var playerStats = Player.Instance.GetComponent<PlayerStats>();

            playerStats.strength.RemoveModifier(strength);
            playerStats.agility.RemoveModifier(agility);
            playerStats.vitality.RemoveModifier(vitality);

            playerStats.damage.RemoveModifier(damage);
            playerStats.critChance.RemoveModifier(critChance);
            playerStats.critPower.RemoveModifier(critPower);

            playerStats.maxHealth.RemoveModifier(health);
            playerStats.armor.RemoveModifier(armor);
            playerStats.evasion.RemoveModifier(evasion);
        }

        public override string GetDescription()
        {
            _stringBuilder.Length = 0;
            _descriptionLength = 0;

            AddItemDescription(strength, "Strength");
            AddItemDescription(agility, "Agility");
            AddItemDescription(intelligence, "Intelligence");
            AddItemDescription(vitality, "Vitality");

            AddItemDescription(damage, "Damage");
            AddItemDescription(critChance, "Crit Chance");
            AddItemDescription(critPower, "Crit Power");

            AddItemDescription(health, "Health");
            AddItemDescription(armor, "Armor");
            AddItemDescription(evasion, "Evasion");
            AddItemDescription(magicResistance, "Magic Resist");

            AddItemDescription(fireDamage, "Fire Damage");
            AddItemDescription(iceDamage, "Ice Damage");
            AddItemDescription(lightningDamage, "Lightning Damage");

            foreach (var effect in itemEffects)
            {
                if (string.IsNullOrWhiteSpace(effect.effectDescription)) continue;
                _stringBuilder.AppendLine();
                _stringBuilder.AppendLine("Unique: " + effect.effectDescription);
                _descriptionLength++;
            }

            for (int i = _descriptionLength; i < 5; i++)
                _stringBuilder.AppendLine();

            return _stringBuilder.ToString();
        }

        public void ExecuteItemEffect(Transform enemyPosition)
        {
            foreach (var effect in itemEffects)
                effect.ExecuteEffect(enemyPosition);
        }

        private void AddItemDescription(int value, string name)
        {
            if (value == 0) return;

            if (_stringBuilder.Length > 0)
                _stringBuilder.AppendLine();

            _stringBuilder.Append($"+ {value} {name}");
            _descriptionLength++;
        }
    }
}

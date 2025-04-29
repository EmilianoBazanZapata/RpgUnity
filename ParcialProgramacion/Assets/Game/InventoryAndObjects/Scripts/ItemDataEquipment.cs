using System.Collections.Generic;
using Game.Inventory.ScriptableObjects;
using Game.Player.Scripts;
using Game.Player.Scripts.Managers;
using Game.Shared.Enums;
using UnityEngine;

namespace Game.Inventory.Scripts
{
    [CreateAssetMenu(fileName = "new Equipment Item Data", menuName = "Data/Equipment")]
    public class ItemDataEquipment : ItemData
    {
        public EquipmentType equipmentType;
        public ItemEffect[] itemEffects;
        public float itemCoolDown;


        [Header("Major stats")] public int strength;
        public int agility;
        public int intelligence;
        public int vitality;

        [Header("Offensive stats")] public int damage;
        public int critChance;
        public int critPower;

        [Header("Defensive stats")] public int health;
        public int armor;
        public int evasion;
        public int magicResistance;

        [Header("Magic stats")] public int fireDamage;
        public int iceDamage;
        public int lightningDamage;

        [Header("Craft rquirements")] public List<InventoryItem> craftingMaterials;


        private int _descriptionLength;


        public void AddModifiers()
        {
            var playerStats = PlayerManager.Instance.Player.GetComponent<PlayerStats>();

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
            var playerStats = PlayerManager.Instance.Player.GetComponent<PlayerStats>();

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
            stringBuilder.Length = 0;
            _descriptionLength = 0;

            AddItemDescription(strength, "Strength");
            AddItemDescription(agility, "Agility");
            AddItemDescription(intelligence, "Intelligence");
            AddItemDescription(vitality, "Vitality");

            AddItemDescription(damage, "Damage");
            AddItemDescription(critChance, "Crit.Chance");
            AddItemDescription(critPower, "Crit.Power");

            AddItemDescription(health, "Health");
            AddItemDescription(evasion, "Evasion");
            AddItemDescription(armor, "Armor");
            AddItemDescription(magicResistance, "Magic Resist.");

            AddItemDescription(fireDamage, "Fire damage");
            AddItemDescription(iceDamage, "Ice damage");
            AddItemDescription(lightningDamage, "Lighting dmg. ");


            for (int i = 0; i < itemEffects.Length; i++)
            {
                if (itemEffects[i].effectDescription.Length > 0)
                {
                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine("Unique: " + itemEffects[i].effectDescription);
                    _descriptionLength++;
                }
            }


            if (_descriptionLength < 5)
            {
                for (int i = 0; i < 5 - _descriptionLength; i++)
                {
                    stringBuilder.AppendLine();
                    stringBuilder.Append("");
                }
            }


            return stringBuilder.ToString();
        }

        public void ExecuteItemEffect(Transform enemyPosition)
        {
            foreach (var item in itemEffects)
            {
                item.ExecuteEffect(enemyPosition);
            }
        }

        private void AddItemDescription(int _value, string _name)
        {
            if (_value != 0)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder.AppendLine();

                if (_value > 0)
                    stringBuilder.Append("+ " + _value + " " + _name);

                _descriptionLength++;
            }
        }
    }
}
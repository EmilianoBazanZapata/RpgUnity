using Game.Character.Scripts;
using Game.Shared.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI.Scripts
{
    public class StatSlot: MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    private UserInterface ui;

    [SerializeField] private string statName;
    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    [TextArea]
    [SerializeField] private string statDescription;

    private void OnValidate()
    {
        gameObject.name = "Stat - " + statName;


        if(statNameText != null)
            statNameText.text = statName;
    }
    void Start()
    {
        UpdateStatValueUI();

        ui = GetComponentInParent<UserInterface>();
    }

    public void UpdateStatValueUI()
    {
        var playerStats = Player.Instance.GetComponent<PlayerStats>();

        if (playerStats == null) return;
        statValueText.text = playerStats.GetStat(statType).GetValue().ToString();
            
        switch (statType)
        {
            case StatType.health:
                statValueText.text = playerStats.GetMaxHealthValue().ToString();
                break;
            case StatType.damage:
                statValueText.text = (playerStats.damage.GetValue() + playerStats.strength.GetValue()).ToString();
                break;
            case StatType.critPower:
                statValueText.text = (playerStats.critPower.GetValue() + playerStats.strength.GetValue()).ToString();
                break;
            case StatType.critChance:
                statValueText.text = (playerStats.critChance.GetValue() + playerStats.agility.GetValue()).ToString();
                break;
            case StatType.evasion:
                statValueText.text = (playerStats.evasion.GetValue() + playerStats.agility.GetValue()).ToString();
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statToolTip.ShowStatToolTip(statDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statToolTip.HideStatToolTip();
    }
}

}
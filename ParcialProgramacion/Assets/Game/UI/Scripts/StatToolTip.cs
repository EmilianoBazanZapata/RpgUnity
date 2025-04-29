using TMPro;
using UnityEngine;

namespace Game.UI.Scripts
{
    public class StatToolTip: ToolTip
    {
        [SerializeField] private TextMeshProUGUI description;
    
        public void ShowStatToolTip( string _text)
        {
            description.text = _text;
            AdjustPosition();

            gameObject.SetActive(true);
        }

        public void HideStatToolTip()
        {
            description.text = "";
            gameObject.SetActive(false);
        }


    }
}
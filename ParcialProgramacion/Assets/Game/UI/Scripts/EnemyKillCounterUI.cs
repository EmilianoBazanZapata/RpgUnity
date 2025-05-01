using Game.Managers;
using TMPro;
using UnityEngine;

namespace Game.UI.Scripts
{
    public class EnemyKillCounterUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI counterText;

        private void Start()
        {
            GameManager.Instance.OnEnemyKillProgress += UpdateCounter;
            
            UpdateCounter(GameManager.Instance.CurrentKills, GameManager.Instance.EnemiesToKillForVictory);
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.OnEnemyKillProgress -= UpdateCounter;
        }

        private void UpdateCounter(int current, int total)
        {
            counterText.text = $"Enemies Defeated: {current} / {total}";
        }
    }
}
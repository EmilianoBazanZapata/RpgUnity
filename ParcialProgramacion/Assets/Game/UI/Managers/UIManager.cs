using Game.Managers;
using Game.UI.Scripts;
using UnityEngine;

namespace Game.UI.Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private UserInterface userInterface;
        [SerializeField] private GameObject victoryScreen;
        [SerializeField] private GameObject gameOverScreen;

        private void OnEnable()
        {
            GameManager.Instance.OnGameWon += HandleGameWon;
            GameManager.Instance.OnGameOver += HandleGameOver;
        }

        private void OnDisable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameWon -= HandleGameWon;
                GameManager.Instance.OnGameOver -= HandleGameOver;
            }
        }

        private void HandleGameWon()
        {
            if (victoryScreen != null)
                victoryScreen.SetActive(true);

            if (userInterface != null)
                userInterface.SwitchTo(victoryScreen);
        }

        private void HandleGameOver()
        {
            if (gameOverScreen != null)
                gameOverScreen.SetActive(true);

            if (userInterface != null)
                userInterface.SwitchTo(gameOverScreen);
        }
    }
}
using Game.Managers;
using Game.Shared.Enums;
using UnityEngine;

namespace Game.UI.Managers
{
    public class UIManager : MonoBehaviour
    {
        [Header("Pantallas")]
        [SerializeField] private GameObject mainMenuUI;
        [SerializeField] private GameObject inGameUI;
        [SerializeField] private GameObject victoryUI;
        [SerializeField] private GameObject gameOverUI;
        [SerializeField] private GameObject optionsUI;
        [SerializeField] private GameObject characterUI;
        [SerializeField] private GameObject craftUI;

        private void OnEnable()
        {
            GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        }

        private void OnDisable()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        }

        private void Start()
        {
            HandleGameStateChanged(GameManager.Instance.CurrentState); // por si ya hay estado
        }

        public void SwitchToUI(GameObject targetUI)
        {
            if (targetUI == null) return;

            foreach (Transform child in transform)
            {
                // Si es UI secundaria (craft, character, options), puede estar en paralelo al inGameUI
                if (child == inGameUI.transform) continue;

                child.gameObject.SetActive(child.gameObject == targetUI);
            }

            // Asegurar que la UI de juego sigue visible mientras se navega por sub-UIs
            inGameUI?.SetActive(true);
        }
        
        
        private void HandleGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.MainMenu:
                    ShowOnly(mainMenuUI);
                    break;
                case GameState.InGame:
                    ShowOnly(inGameUI);
                    break;
                case GameState.Victory:
                    ShowOnly(victoryUI);
                    break;
                case GameState.GameOver:
                    ShowOnly(gameOverUI);
                    break;
            }
        }

        private void ShowOnly(GameObject uiToShow)
        {
            foreach (Transform child in transform)
                child.gameObject.SetActive(child.gameObject == uiToShow);
        }

        private void Update()
        {
            if (GameManager.Instance.CurrentState != GameState.InGame) return;

            if (Input.GetKeyDown(KeyCode.C))
                ToggleUI(characterUI);

            if (Input.GetKeyDown(KeyCode.B))
                ToggleUI(craftUI);

            if (Input.GetKeyDown(KeyCode.O))
                ToggleUI(optionsUI);
        }

        private void ToggleUI(GameObject ui)
        {
            if (ui != null)
                ui.SetActive(!ui.activeSelf);
        }
    }
}
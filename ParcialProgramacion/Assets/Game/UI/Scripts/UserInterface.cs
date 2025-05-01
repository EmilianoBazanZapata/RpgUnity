using System;
using Game.Managers;
using Game.Shared.Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.UI.Scripts
{
    public class UserInterface : MonoBehaviour
    {
        [Header("Pantallas")] [SerializeField] private GameObject mainMenuUI;
        [SerializeField] private GameObject inGameUI;
        [SerializeField] private GameObject victoryUI;
        [SerializeField] private GameObject gameOverUI;

        [Header("Sub-UIs")] [SerializeField] private GameObject characterUI;
        [SerializeField] private GameObject craftUI;
        [SerializeField] private GameObject optionsUI;
        [SerializeField] private GameObject inControlUI;

        public ItemTooltip itemToolTip;
        public StatToolTip statToolTip;
        public CraftWindow craftWindow;

        private void Start()
        {
            GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
            HandleGameStateChanged(GameManager.Instance.CurrentState);
        }

        private void Awake()
        {
            ShowOnly(mainMenuUI); // Estado inicial
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
                GameManager.Instance.GoToOptions();
        }

        private void ToggleUI(GameObject ui)
        {
            if (ui != null)
                ui.SetActive(!ui.activeSelf);
        }

        private void HandleGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.MainMenu:
                    ShowOnly(mainMenuUI);
                    break;
                case GameState.InGame:
                    ReloadCurrentScene();
                    break;
                case GameState.Victory:
                    ShowOnly(victoryUI);
                    break;
                case GameState.GameOver:
                    ShowOnly(gameOverUI);
                    break;
                case GameState.InControls:
                    ShowOnly(inControlUI);
                    break;
                case GameState.InOptions:
                    ShowOnly(optionsUI);
                    break;
                case GameState.InCharacter:
                    ShowOnly(characterUI);
                    break;
                case GameState.InCraft:
                    ShowOnly(craftUI);
                    break;
                case GameState.ResumeGame:
                    ShowOnly(inGameUI);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public void ShowOnly(GameObject uiToShow)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(child.gameObject == uiToShow);
            }
        }
        
        private void ReloadCurrentScene()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            var currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            GameManager.Instance.ResumeGame();
            SoundManager.Instance.StartBackgroundMusic();
        }
    }
}
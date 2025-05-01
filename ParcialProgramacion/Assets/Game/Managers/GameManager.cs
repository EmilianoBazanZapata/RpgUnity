using System;
using Game.Shared.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Managers
{
    public class GameManager : MonoBehaviour
    {
        public int EnemiesToKillForVictory = 20;
        public int CurrentKills = 0;
        public event Action<int, int> OnEnemyKillProgress;
        
        
        public static GameManager Instance { get; private set; }
        public GameState CurrentState { get; private set; }
        public event Action<GameState> OnGameStateChanged;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SetGameState(GameState newState)
        {
            if (newState == CurrentState) return;

            CurrentState = newState;
            OnGameStateChanged?.Invoke(CurrentState);
        }
        
        public void EnemyKilled()
        {
            CurrentKills++;

            OnEnemyKillProgress?.Invoke(CurrentKills, EnemiesToKillForVictory);

            if (CurrentKills >= EnemiesToKillForVictory)
            {
                WinGame();
            }
        }
        
        public void StartGame() => SetGameState(GameState.StartGame);
        public void WinGame() => SetGameState(GameState.Victory);
        public void LoseGame() => SetGameState(GameState.GameOver);
        public void GoToMenu() => SetGameState(GameState.MainMenu);
        public void GoToControl() => SetGameState(GameState.InControls);        
        public void GoToOptions() => SetGameState(GameState.InOptions);
        public void GoToCharacter() => SetGameState(GameState.InCharacter);
        public void GoToCraft() => SetGameState(GameState.InCraft);
        public void ResumeGame() => SetGameState(GameState.InGame);
        public void InGame() => SetGameState(GameState.InGame);
    }
}
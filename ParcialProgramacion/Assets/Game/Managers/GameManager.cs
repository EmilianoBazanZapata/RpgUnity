using System;
using UnityEngine;

namespace Game.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("VictoryConfiguration")]
        [SerializeField] private int enemiesToKillToWin = 20;

        private int _currentEnemiesKilled;
        public event Action OnEnemyKilled;
        public event Action OnGameWon;
        public event Action OnGameOver;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void EnemyKilled()
        {
            _currentEnemiesKilled++;
            
            Debug.Log($"Enemigos muertos: {_currentEnemiesKilled}");
            
            OnEnemyKilled?.Invoke();

            if (_currentEnemiesKilled >= enemiesToKillToWin)
            {
                WinGame();
            }
        }

        public void WinGame()
        {
            OnGameWon?.Invoke();
        }

        public void LoseGame()
        {
            Debug.Log("¡Perdiste el juego!");
            OnGameOver?.Invoke();
        }
    }
}
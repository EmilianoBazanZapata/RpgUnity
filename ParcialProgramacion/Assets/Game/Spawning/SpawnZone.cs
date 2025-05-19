using System.Collections.Generic;
using Game.Enemies;
using Game.Managers;
using Game.Shared.Enums;
using UnityEngine;

namespace Game.Spawning
{
    public class SpawnZone : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private int _maxEnemies = 3;
        [SerializeField] private float _respawnDistance = 10f;
        [SerializeField] private float _triggerDistance = 7f;
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private float _respawnCooldown = 60f;
        [SerializeField] private ObjectPoolEnemy _enemyPool;
        [SerializeField] private Transform _playerTransform;

        #endregion

        #region Private Fields

        private float _respawnTimer = 0f;
        private bool _readyToRespawn = false;
        private readonly List<GameObject> _currentEnemies = new();

        #endregion

        #region Unity Methods

        private void Start() => TrySpawnEnemies();
        private void Update()
        {
            if (!IsInGameState() || _playerTransform == null)
                return;

            var distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);

            HandleRespawnTimer();

            if (CanRespawn(distanceToPlayer))
                PerformRespawn();
        }
        
        #endregion

        #region Private Methods
        private bool IsInGameState() => GameManager.Instance.CurrentState == GameState.InGame;

        private void HandleRespawnTimer()
        {
            if (_currentEnemies.Count == 0 && !_readyToRespawn)
            {
                _readyToRespawn = true;
                _respawnTimer = 0f;
            }

            if (_readyToRespawn)
                _respawnTimer += Time.deltaTime;
        }

        private bool CanRespawn(float distanceToPlayer)
        {
            return _readyToRespawn &&
                   _respawnTimer >= _respawnCooldown &&
                   distanceToPlayer > _respawnDistance;
        }

        private void PerformRespawn()
        {
            TrySpawnEnemies();
            _readyToRespawn = false;
        }

        private void TrySpawnEnemies()
        {
            var enemiesToSpawn = Mathf.Min(_maxEnemies - _currentEnemies.Count, _spawnPoints.Length);

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                var enemy = _enemyPool.GetObject();
                if (enemy == null)
                    continue;

                enemy.transform.position = _spawnPoints[i].position;

                if (!enemy.TryGetComponent(out Enemy enemyBase))
                    continue;

                enemyBase.SetPool(_enemyPool);
                enemyBase.ResetUIHealth();
                enemy.SetActive(true);

                enemyBase.OnDeath = () => OnEnemyDeath(enemy);

                _currentEnemies.Add(enemy);
            }
        }

        private void OnEnemyDeath(GameObject enemy)
        {
            if (_currentEnemies.Contains(enemy))
                _currentEnemies.Remove(enemy);

            if (_currentEnemies.Count != 0) return;
            
            _readyToRespawn = true;
            _respawnTimer = 0f;
        }

        #endregion
    }
}

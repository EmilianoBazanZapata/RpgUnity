using System.Collections.Generic;
using Game.Enemies;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Spawning
{
    public class SpawnZone : MonoBehaviour
    {
        [SerializeField] private int maxEnemies = 3;
        [SerializeField] private float respawnDistance = 10f;
        [SerializeField] private float triggerDistance = 7f;
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private float respawnCooldown = 60f;

        [FormerlySerializedAs("enemyPool")] [SerializeField]
        private ObjectPoolEnemy enemyPoolEnemy;

        private float _timeSinceAllDead = 0f;
        private bool _allEnemiesDead = false;
        private List<GameObject> _currentEnemies = new();
        private GameObject _player;

        private bool _playerWasClose;

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            TrySpawnEnemies();
        }

        private void Update()
        {
            float distance = Vector2.Distance(transform.position, _player.transform.position);

            // Si todos murieron, iniciar el cooldown si aún no está corriendo
            if (_currentEnemies.Count == 0 && !_allEnemiesDead)
            {
                _allEnemiesDead = true;
                _timeSinceAllDead = 0f;
            }

            // Si el cooldown está corriendo, acumulá el tiempo
            if (_allEnemiesDead)
            {
                _timeSinceAllDead += Time.deltaTime;
            }

            // ✅ Cuando el jugador se aleja lo suficiente Y pasó el tiempo, respawneamos
            if (_allEnemiesDead && _timeSinceAllDead >= respawnCooldown && distance > respawnDistance)
            {
                TrySpawnEnemies();
                _allEnemiesDead = false;
            }
        }

        private void TrySpawnEnemies()
        {
            if (_currentEnemies.Count >= maxEnemies) return;

            var enemiesToSpawn = Mathf.Min(maxEnemies - _currentEnemies.Count, spawnPoints.Length);

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                var enemy = enemyPoolEnemy.GetObject();
                if (enemy == null)
                    continue;

                enemy.transform.position = spawnPoints[i].position;

                var enemyBase = enemy.GetComponent<Enemy>();
                
                enemyBase.SetPool(enemyPoolEnemy);
                enemyBase.ResetUIHealth();
                enemy.SetActive(true);

                // Setear el evento OnDeath una sola vez
                enemyBase.OnDeath = () =>
                {
                    enemyBase.WasDeadBeforeRespawn = true;
                    _currentEnemies.Remove(enemy);

                    if (_currentEnemies.Count == 0)
                    {
                        _allEnemiesDead = true;
                        _timeSinceAllDead = 0f;
                    }
                };

                _currentEnemies.Add(enemy);
            }
        }
    }
}
using Game.Enemies;
using UnityEngine;

namespace Game.Spawning
{ 
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Configuración del Spawner")]
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private bool spawnOnStart = true;
        [SerializeField] private float spawnDelay = 0f;

        [Header("Control de cantidad")]
        [SerializeField] private int maxEnemies = 1;
        private int _currentEnemiesAlive = 0;

        [Header("Pool")]
        [SerializeField] private EnemyPool enemyPool;

        private void Start()
        {
            if (spawnOnStart)
            {
                Invoke(nameof(SpawnEnemy), spawnDelay);
            }
        }

        public void SpawnEnemy()
        {
            if (_currentEnemiesAlive >= maxEnemies)
                return;

            GameObject enemyGO = enemyPool.GetEnemy(spawnPoint.position);
            _currentEnemiesAlive++;

            Enemy enemyScript = enemyGO.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.SubscribeOnDeath(() =>
                {
                    _currentEnemiesAlive--;
                    Invoke(nameof(SpawnEnemy), spawnDelay); // respawn automático
                });
            }
        }
    }
}
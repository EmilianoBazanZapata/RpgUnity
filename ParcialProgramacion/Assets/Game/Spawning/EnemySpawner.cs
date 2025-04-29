using System.Collections.Generic;
using Game.Enemies;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Spawning
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private int _maxEnemies = 10;
        [SerializeField] private float _spawnDelay = 2f;
        private List<GameObject> _activeEnemies = new();
        private ObjectPoolEnemy _poolEnemy;

        private void Awake()
        {
            _poolEnemy = GetComponent<ObjectPoolEnemy>();
        }

        private void Start()
        {
            StartCoroutine(SpawnLoop());
        }

        private IEnumerator<WaitForSeconds> SpawnLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(_spawnDelay);
                TrySpawnEnemy();
            }
        }

        private void TrySpawnEnemy()
        {
            if (_activeEnemies.Count >= _maxEnemies) return;

            var spawnPoint = GetRandomFreeSpawnPoint();
            if (spawnPoint == null) return;

            var enemy = _poolEnemy.GetObject();
            enemy.transform.position = spawnPoint.position;
            enemy.SetActive(true);

            var enemyScript = enemy.GetComponent<Enemy>();
            enemyScript.OnDeath = () =>
            {
                _activeEnemies.Remove(enemy);
                _poolEnemy.ReturnObject(enemy);
            };

            _activeEnemies.Add(enemy);
        }

        private Transform GetRandomFreeSpawnPoint()
        {
            List<Transform> available = new();

            foreach (var point in _spawnPoints)
            {
                var occupied = _activeEnemies.Exists(e =>
                    Vector2.Distance(e.transform.position, point.position) < 0.1f);
                if (!occupied)
                    available.Add(point);
            }

            return available.Count == 0 ? null : available[Random.Range(0, available.Count)];
        }
    }
}
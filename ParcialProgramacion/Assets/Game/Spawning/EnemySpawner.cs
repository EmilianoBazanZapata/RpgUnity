using System.Collections;
using System.Collections.Generic;
using Game.Enemies;
using UnityEngine;

namespace Game.Spawning
{
    /// <summary>
    /// Spawner que genera enemigos en puntos disponibles utilizando un pool de objetos.
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        #region Serialized Fields

        [Header("Configuración de Spawn")]
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private int _maxEnemies = 10;
        [SerializeField] private float _spawnDelay = 2f;

        #endregion

        #region Private Fields

        private readonly List<GameObject> _activeEnemies = new();
        private ObjectPoolEnemy _poolEnemy;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            _poolEnemy = GetComponent<ObjectPoolEnemy>();
        }

        private void Start()
        {
            StartCoroutine(SpawnLoop());
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Corrutina principal que intenta spawnear enemigos periódicamente.
        /// </summary>
        private IEnumerator SpawnLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(_spawnDelay);
                TrySpawnEnemy();
            }
        }

        /// <summary>
        /// Intenta instanciar un nuevo enemigo si no se superó el límite.
        /// </summary>
        private void TrySpawnEnemy()
        {
            if (_activeEnemies.Count >= _maxEnemies)
                return;

            Transform spawnPoint = GetRandomFreeSpawnPoint();
            if (spawnPoint == null)
                return;

            GameObject enemy = _poolEnemy.GetObject();
            enemy.transform.position = spawnPoint.position;
            enemy.SetActive(true);

            if (!enemy.TryGetComponent(out Enemy enemyScript))
                return;

            // Asigna la lógica de eliminación
            enemyScript.OnDeath = () => OnEnemyDeath(enemy);

            _activeEnemies.Add(enemy);
        }

        /// <summary>
        /// Lógica al morir un enemigo: lo remueve del listado y lo devuelve al pool.
        /// </summary>
        private void OnEnemyDeath(GameObject enemy)
        {
            _activeEnemies.Remove(enemy);
            _poolEnemy.ReturnObject(enemy);
        }

        /// <summary>
        /// Devuelve un punto de spawn disponible al azar.
        /// </summary>
        private Transform GetRandomFreeSpawnPoint()
        {
            List<Transform> available = new();

            foreach (Transform point in _spawnPoints)
            {
                bool isOccupied = _activeEnemies.Exists(e =>
                    Vector2.Distance(e.transform.position, point.position) < 0.1f);

                if (!isOccupied)
                    available.Add(point);
            }

            return available.Count == 0
                ? null
                : available[Random.Range(0, available.Count)];
        }

        #endregion
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace Game.Spawning
{
    public class EnemyPool: MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private int initialSize = 10;

        private Queue<GameObject> pool = new Queue<GameObject>();

        private void Awake()
        {
            for (int i = 0; i < initialSize; i++)
            {
                GameObject enemy = Instantiate(enemyPrefab, transform);
                enemy.SetActive(false);
                pool.Enqueue(enemy);
            }
        }

        public GameObject GetEnemy(Vector3 position)
        {
            GameObject enemy;

            if (pool.Count > 0)
            {
                enemy = pool.Dequeue();
            }
            else
            {
                enemy = Instantiate(enemyPrefab, transform); // crecimiento dinámico opcional
            }

            enemy.transform.position = position;
            enemy.SetActive(true);

            return enemy;
        }

        public void ReturnToPool(GameObject enemy)
        {
            enemy.SetActive(false);
            pool.Enqueue(enemy);
        }
    }
}
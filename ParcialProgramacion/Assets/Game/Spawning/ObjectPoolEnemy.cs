using System.Collections.Generic;
using UnityEngine;

namespace Game.Spawning
{
    public class ObjectPoolEnemy: MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private int initialSize = 10;

        private Queue<GameObject> pool = new();

        private void Awake()
        {
            for (int i = 0; i < initialSize; i++)
            {
                var obj = Instantiate(prefab);
                obj.SetActive(false);
                pool.Enqueue(obj);
            }
        }

        public GameObject GetObject()
        {
            if (pool.Count > 0)
                return pool.Dequeue();

            var newObj = Instantiate(prefab);
            newObj.SetActive(false);
            return newObj;
        }

        public void ReturnObject(GameObject obj)
        {
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }
}
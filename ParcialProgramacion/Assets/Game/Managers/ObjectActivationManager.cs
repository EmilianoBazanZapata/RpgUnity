using System.Collections.Generic;
using UnityEngine;

namespace Game.Managers
{
    /// <summary>
    /// Activa o desactiva objetos en la escena basándose en su distancia al jugador.
    /// </summary>
    public class ObjectActivationManager : MonoBehaviour
    {
        public static ObjectActivationManager Instance;

        [Header("Settings")]
        [SerializeField] private float checkRadius = 15f;
        [SerializeField] private float checkInterval = 0.5f;

        private Transform _playerTransform;
        private readonly HashSet<GameObject> _objectsToCheck = new HashSet<GameObject>();
        private float _timer;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }

        private void Start()
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                _timer = checkInterval;
                UpdateObjectVisibility();
            }
        }

        private void UpdateObjectVisibility()
        {
            foreach (var obj in _objectsToCheck)
            {
                if (obj == null) continue;

                float distance = Vector2.Distance(_playerTransform.position, obj.transform.position);
                bool isVisible = distance <= checkRadius;

                if (obj.activeSelf != isVisible)
                    obj.SetActive(isVisible);
            }
        }

        public void RegisterObject(GameObject obj)
        {
            _objectsToCheck.Add(obj);
        }

        public void UnregisterObject(GameObject obj)
        {
            _objectsToCheck.Remove(obj);
        }
    }
}
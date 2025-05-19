using System.Collections.Generic;
using UnityEngine;

namespace Game.Spawning
{
    /// <summary>
    /// Pool de objetos simple para enemigos.
    /// Instancia una cantidad inicial y reutiliza instancias
    /// en lugar de crearlas/destruirlas constantemente.
    /// </summary>
    public class ObjectPoolEnemy : MonoBehaviour
    {
        #region Serialized Fields

        [Header("Configuración del Pool")]
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _initialSize = 10;

        #endregion

        #region Private Fields

        private readonly Queue<GameObject> _pool = new();

        #endregion

        #region Unity Methods

        private void Awake()
        {
            // Instancia la cantidad inicial de objetos y los desactiva
            for (int i = 0; i < _initialSize; i++)
                _pool.Enqueue(CrearObjetoPooled());
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Obtiene un objeto del pool si hay disponible.
        /// Si el pool está vacío, crea uno nuevo.
        /// </summary>
        public GameObject GetObject()
        {
            return _pool.Count > 0 ? _pool.Dequeue() : CrearObjetoPooled();
        }

        /// <summary>
        /// Devuelve un objeto al pool para su futura reutilización.
        /// </summary>
        public void ReturnObject(GameObject obj)
        {
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Instancia un nuevo objeto y lo prepara para ser almacenado en el pool.
        /// </summary>
        private GameObject CrearObjetoPooled()
        {
            var obj = Instantiate(_prefab, transform);
            obj.SetActive(false);
            return obj;
        }

        #endregion
    }
}
using Game.Enemies;
using UnityEngine;

namespace Game.Character.Scripts
{
    /// <summary>
    /// Clase base para habilidades del jugador.
    /// Maneja cooldown y lógica de activación.
    /// </summary>
    public class Skill : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] protected float _cooldown = 2f;
        [SerializeField] protected float _enemySearchRadius = 25f;

        #endregion
        
        public float Cooldown
        {
            get => _cooldown;
            set => _cooldown = Mathf.Max(0, value);
        }

        protected float CooldownTimer
        {
            get => _cooldownTimer;
            set => _cooldownTimer = Mathf.Max(0, value);
        }


        #region Protected Fields

        protected float _cooldownTimer;
        protected Character.Scripts.Player Player;

        #endregion

        #region Unity Methods

        protected virtual void Start()
        {
            Player = Character.Scripts.Player.Instance;
        }

        protected virtual void Update()
        {
            _cooldownTimer -= Time.deltaTime;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Verifica si la habilidad puede usarse (cooldown terminado) y la ejecuta.
        /// </summary>
        /// <returns>true si se usó la habilidad; false si aún está en cooldown.</returns>
        public virtual bool CanUseSkill()
        {
            if (_cooldownTimer > 0)
                return false;

            UseSkill();
            _cooldownTimer = _cooldown;
            return true;
        }

        /// <summary>
        /// Lógica de uso de la habilidad. Se sobreescribe en clases hijas.
        /// </summary>
        public virtual void UseSkill() { }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Busca el enemigo más cercano dentro de un radio dado.
        /// </summary>
        /// <param name="origin">Transform desde donde buscar enemigos.</param>
        /// <returns>Transform del enemigo más cercano o null si no hay.</returns>
        protected virtual Transform FindClosestEnemy(Transform origin)
        {
            var colliders = Physics2D.OverlapCircleAll(origin.position, _enemySearchRadius);
            var closestDistance = Mathf.Infinity;
            Transform closestEnemy = null;

            foreach (var hit in colliders)
            {
                if (!hit.TryGetComponent(out Enemy enemy)) continue;

                float distance = Vector2.Distance(origin.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = hit.transform;
                }
            }

            return closestEnemy;
        }

        #endregion
    }
}

using Game.Shared.Scripts;
using UnityEngine;

namespace Game.Character.Scripts.Controllers
{
    public class CrystalSkillController : MonoBehaviour
    {
        [Header("Configuración")]
        [SerializeField] private LayerMask _whatIsEnemy;

        private Animator _animator;
        private CircleCollider2D _collider;

        private Transform _closestTarget;
        private Player _player;

        private float _crystalExistsTimer;
        private bool _canExplode;
        private bool _canMove;
        private float _moveSpeed;
        private bool _canGrow;
        private float _growSpeed = 5f;

        #region Unity Methods

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _collider = GetComponent<CircleCollider2D>();
        }

        private void Update()
        {
            UpdateTimer();
            MoveTowardsTarget();
            HandleGrowth();
        }

        #endregion

        #region Setup

        public void SetupCrystal(float duration, bool canExplode, bool canMove, float moveSpeed, Transform closestEnemy, Player player)
        {
            _crystalExistsTimer = duration;
            _canExplode = canExplode;
            _canMove = canMove;
            _moveSpeed = moveSpeed;
            _closestTarget = closestEnemy;
            _player = player;
        }

        #endregion

        #region Crystal Behaviour

        private void UpdateTimer()
        {
            _crystalExistsTimer -= Time.deltaTime;

            if (_crystalExistsTimer <= 0f)
                FinishCrystal();
        }

        private void MoveTowardsTarget()
        {
            if (!_canMove || _closestTarget == null)
                return;

            transform.position = Vector2.MoveTowards(
                transform.position,
                _closestTarget.position,
                _moveSpeed * Time.deltaTime
            );

            if (!(Vector2.Distance(transform.position, _closestTarget.position) < 0.5f)) return;
            
            FinishCrystal();
            _canMove = false;
        }

        private void HandleGrowth()
        {
            if (_canGrow)
            {
                transform.localScale = Vector2.Lerp(
                    transform.localScale,
                    new Vector2(3f, 3f),
                    _growSpeed * Time.deltaTime
                );
            }
        }

        public void FinishCrystal()
        {
            _canGrow = true;
            _animator.SetTrigger("Explode");
        }

        // Triggered via animation event
        private void AnimationExplode()
        {
            var colliders = Physics2D.OverlapCircleAll(transform.position, _collider.radius, _whatIsEnemy);

            foreach (var hit in colliders)
            {
                var enemyStats = hit.GetComponent<CharacterStats>();
                if (enemyStats != null)
                    _player.Stats.DoDamage(enemyStats);
            }
        }

        public void SelfDestroy() => Destroy(gameObject);

        #endregion
    }
}

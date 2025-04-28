using Game.Enemies;
using Game.Shared.Scripts;
using UnityEngine;

namespace Game.Player.Scripts.Controllers
{
    public class CrystalSkillController : MonoBehaviour
    {
        [SerializeField] private LayerMask _whatIsEnemy;

        private Animator _animator => GetComponent<Animator>();
        private CircleCollider2D _collider => GetComponent<CircleCollider2D>();
        private Transform _closestTraget;
        private Player _player;
        private float _crystalExistsTimer;
        private bool _canExplode;
        private bool _canMove;
        private float _moveSpeed;
        private bool _canGrow;
        private float _growSpeed = 5;

        private void Update()
        {
            UpdateCrystalTimer();
            UpdateCrystalMovement();
            UpdateCrystalGrowth();
        }

        private void UpdateCrystalTimer()
        {
            _crystalExistsTimer -= Time.deltaTime;

            if (_crystalExistsTimer < 0)
                FinishCrystal();
        }

        private void UpdateCrystalMovement()
        {
            if (!_canMove) return;

            if (_closestTraget == null) return;

            transform.position = Vector2.MoveTowards(
                transform.position,
                _closestTraget.position,
                _moveSpeed * Time.deltaTime
            );

            if (Vector2.Distance(transform.position, _closestTraget.position) < 0.5)
            {
                FinishCrystal();
                _canMove = false;
            }
        }

        private void UpdateCrystalGrowth()
        {
            if (_canGrow)
            {
                transform.localScale = Vector2.Lerp(
                    transform.localScale,
                    new Vector2(3, 3),
                    _growSpeed * Time.deltaTime
                );
            }
        }

        public void SetupCrystal(float crystalDuration,
            bool canExplode,
            bool canMove,
            float moveSpeed,
            Transform closestEnemy,
            Player player)
        {
            _crystalExistsTimer = crystalDuration;
            _crystalExistsTimer = crystalDuration;
            _canExplode = canExplode;
            _canMove = canMove;
            _moveSpeed = moveSpeed;
            _closestTraget = closestEnemy;
            _player = player;
        }

        private void AnimationExplode()
        {
            var colliders = Physics2D.OverlapCircleAll(transform.position, _collider.radius);

            foreach (var hit in colliders)
            {
                if (hit.GetComponent<Enemy>() != null)
                    _player.Stats.DoDamage(hit.GetComponent<CharacterStats>());
            }
        }

        public void FinishCrystal()
        {
            if (_canExplode)
            {
                _canGrow = true;
                _animator.SetTrigger("Explode");
            }
            else
                SelfDestroy();
        }

        public void SelfDestroy() => Destroy(gameObject);
    }
}
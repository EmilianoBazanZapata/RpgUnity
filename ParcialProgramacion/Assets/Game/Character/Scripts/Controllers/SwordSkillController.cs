using Game.Enemies;
using Game.Managers;
using Game.Shared.Enums;
using Game.Shared.Scripts;
using UnityEngine;

namespace Game.Character.Scripts.Controllers
{
    public class SwordSkillController : MonoBehaviour
    {
        [Header("Core Settings")]
        [SerializeField] private float _returnSpeed = 15f;

        private Animator _animator;
        private Rigidbody2D _rigidBody;
        private CircleCollider2D _circleCollider;
        private Player _player;

        private bool _canRotate = true;
        private bool _isReturning;
        private float _freezeTimeDuration;

        #region Unity Methods

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _rigidBody = GetComponent<Rigidbody2D>();
            _circleCollider = GetComponent<CircleCollider2D>();
        }

        private void Update()
        {
            RotateSword();
            ReturnToPlayer();
        }

        #endregion

        #region Setup & Return Logic

        public void SetupSword(Vector2 direction, float gravityScale, Player playerRef, float freezeDuration, float returnSpeed)
        {
            _player = playerRef;
            _freezeTimeDuration = freezeDuration;
            _returnSpeed = returnSpeed;

            _rigidBody.velocity = direction;
            _rigidBody.gravityScale = gravityScale;
            _animator.SetBool("Rotation", true);

            Invoke(nameof(DestroySelf), 3f);
        }

        public void ReturnSword()
        {
            _rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
            transform.parent = null;
            _isReturning = true;
            _animator.SetBool("Rotation", true);
        }

        private void ReturnToPlayer()
        {
            if (!_isReturning) return;

            transform.position = Vector2.MoveTowards(
                transform.position,
                _player.transform.position,
                _returnSpeed * Time.deltaTime
            );

            if (Vector2.Distance(transform.position, _player.transform.position) < 1f)
                _player.CatchTheSword();
        }

        #endregion

        #region Rotation & Collision

        private void RotateSword()
        {
            if (_canRotate)
                transform.right = _rigidBody.velocity;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_isReturning) return;

            if (collision.TryGetComponent(out Enemy enemy))
            {
                ApplySwordDamage(enemy);
                SoundManager.Instance.PlaySound(SoundType.Attack);
            }

            StickToSurface(collision);
        }

        private void StickToSurface(Collider2D collision)
        {
            _canRotate = false;
            _circleCollider.enabled = false;
            _rigidBody.isKinematic = true;
            _rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
            _animator.SetBool("Rotation", false);
            transform.parent = collision.transform;
        }

        private void ApplySwordDamage(Enemy enemy)
        {
            if (!enemy.TryGetComponent(out CharacterStats stats)) return;
            
            _player.Stats.DoDamage(stats);
            enemy.StartCoroutine(enemy.FreezeTimeFor(_freezeTimeDuration)); // asumiendo que es un método público
        }

        #endregion

        private void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}

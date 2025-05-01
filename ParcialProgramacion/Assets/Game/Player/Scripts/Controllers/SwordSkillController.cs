using Game.Enemies;
using Game.Managers;
using Game.Shared.Enums;
using Game.Shared.Scripts;
using UnityEngine;

namespace Game.Player.Scripts.Controllers
{
    public class SwordSkillController : MonoBehaviour
    {
        [Header("Core Settings")] [SerializeField]
        private float returnSpeed;

        private Animator _animator;
        private Rigidbody2D _rigidBody;
        private CircleCollider2D _circleCollider;
        private Player _player;

        private bool _canRotate = true;
        private bool _isReturning;
        private bool _isSpinning;
        private bool _wasStopped;
        private float _freezeTimeDuration;

        private void Awake()
        {
            InitializeComponents();
        }

        private void Update()
        {
            HandleRotation();
            HandleReturning();
        }

        private void InitializeComponents()
        {
            _animator = GetComponentInChildren<Animator>();
            _rigidBody = GetComponent<Rigidbody2D>();
            _circleCollider = GetComponent<CircleCollider2D>();
        }

        public void SetupSword(Vector2 direction, float gravityScale, Player playerRef, float freezeDuration,
            float returnSpeedValue)
        {
            _player = playerRef;
            _freezeTimeDuration = freezeDuration;
            returnSpeed = returnSpeedValue;

            _rigidBody.velocity = direction;
            _rigidBody.gravityScale = gravityScale;

            _animator.SetBool("Rotation", true);

            Invoke(nameof(DestroyMe), 3f);
        }

        public void ReturnSword()
        {
            _rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
            transform.parent = null;
            _isReturning = true;
            _animator.SetBool("Rotation", true);
        }

        private void HandleRotation()
        {
            if (_canRotate)
            {
                transform.right = _rigidBody.velocity;
            }
        }

        private void HandleReturning()
        {
            if (!_isReturning) return;

            transform.position = Vector2.MoveTowards(
                transform.position,
                _player.transform.position,
                returnSpeed * Time.deltaTime
            );

            if (Vector2.Distance(transform.position, _player.transform.position) < 1f)
            {
                _player.CatchTheSword();
            }
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_isReturning) return;

            if (collision.TryGetComponent<Enemy>(out var enemy))
            {
                SwordSkillDamage(enemy);
                SoundManager.Instance.PlaySound(SoundType.Attack);
            }

            StuckInto(collision);
        }
        private void StuckInto(Collider2D collision)
        {
            DisableSwordMovement();

            _animator.SetBool("Rotation", false);
            transform.parent = collision.transform;
        }
        
        private void SwordSkillDamage(Enemy enemy)
        {
            _player.Stats.DoDamage(enemy.GetComponent<CharacterStats>());
            enemy.StartCoroutine("FreezeTimeFor", _freezeTimeDuration);
        }
        
        private void DisableSwordMovement()
        {
            _canRotate = false;
            _circleCollider.enabled = false;
            _rigidBody.isKinematic = true;
            _rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        private void StopWhenSpinning()
        {
            _wasStopped = true;
            _rigidBody.constraints = RigidbodyConstraints2D.FreezePosition;
        }

        private void DestroyMe()
        {
            Destroy(gameObject);
        }
    }
}
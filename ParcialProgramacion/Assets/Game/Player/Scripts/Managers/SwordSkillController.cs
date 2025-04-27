using System.Collections.Generic;
using Game.Enemies.Skeleton.Scripts;
using Game.Shared.Scripts;
using UnityEngine;

namespace Game.Player.Scripts.Managers
{
    public class SwordSkillController : MonoBehaviour
    {
        [Header("Core Settings")] [SerializeField]
        private float returnSpeed;

        [Header("Pierce Settings")] [SerializeField]
        private float pierceAmount;

        [Header("Bounce Settings")] [SerializeField]
        private float bounceSpeed;

        [SerializeField] private List<Transform> enemyTargets = new List<Transform>();

        [Header("Spin Settings")] [SerializeField]
        private float maxTravelDistance;

        [SerializeField] private float spinDuration;
        [SerializeField] private float hitCoolDown;

        private Animator animator;
        private Rigidbody2D rb;
        private CircleCollider2D circleCollider;
        private Player player;

        private bool canRotate = true;
        private bool isReturning;
        private bool isBouncing;
        private bool isSpinning;
        private bool wasStopped;

        private int targetIndex;
        private int bounceAmount;

        private float freezeTimeDuration;
        private float spinTimer;
        private float hitTimer;
        private float spinDirection;

        private void Awake()
        {
            InitializeComponents();
        }

        private void Update()
        {
            HandleRotation();
            HandleReturning();
            HandleBounce();
            HandleSpin();
        }

        private void InitializeComponents()
        {
            animator = GetComponentInChildren<Animator>();
            rb = GetComponent<Rigidbody2D>();
            circleCollider = GetComponent<CircleCollider2D>();
        }

        public void SetupSword(Vector2 direction, float gravityScale, Player playerRef, float freezeDuration,
            float returnSpeedValue)
        {
            player = playerRef;
            freezeTimeDuration = freezeDuration;
            returnSpeed = returnSpeedValue;

            rb.velocity = direction;
            rb.gravityScale = gravityScale;

            animator.SetBool("Rotation", true);
            spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);

            Invoke(nameof(DestroyMe), 3f);
        }

        public void ReturnSword()
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            transform.parent = null;
            isReturning = true;
            animator.SetBool("Rotation", true);
        }

        private void HandleRotation()
        {
            if (canRotate)
            {
                transform.right = rb.velocity;
            }
        }

        private void HandleReturning()
        {
            if (!isReturning) return;

            transform.position = Vector2.MoveTowards(
                transform.position,
                player.transform.position,
                returnSpeed * Time.deltaTime
            );

            if (Vector2.Distance(transform.position, player.transform.position) < 1f)
            {
                player.CatchTheSword();
            }
        }

        private void HandleBounce()
        {
            if (!isBouncing || enemyTargets.Count <= 0) return;

            transform.position = Vector2.MoveTowards(
                transform.position,
                enemyTargets[targetIndex].position,
                bounceSpeed * Time.deltaTime
            );

            if (Vector2.Distance(transform.position, enemyTargets[targetIndex].position) < 1f)
            {
                ProcessBounceHit();
            }
        }

        private void ProcessBounceHit()
        {
            SwordSkillDamage(enemyTargets[targetIndex].GetComponent<Enemy>());
            targetIndex++;
            bounceAmount--;

            if (bounceAmount <= 0)
            {
                isBouncing = false;
                isReturning = true;
            }

            if (targetIndex >= enemyTargets.Count)
            {
                targetIndex = 0;
            }
        }

        private void HandleSpin()
        {
            if (!isSpinning) return;

            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }

            if (wasStopped)
            {
                ProcessSpinning();
            }
        }

        private void ProcessSpinning()
        {
            spinTimer -= Time.deltaTime;
            hitTimer -= Time.deltaTime;

            transform.position = Vector2.MoveTowards(
                transform.position,
                new Vector2(transform.position.x + spinDirection, transform.position.y),
                1.5f * Time.deltaTime
            );

            if (spinTimer < 0)
            {
                isReturning = true;
                isSpinning = false;
            }

            if (hitTimer < 0)
            {
                HandleSpinDamage();
            }
        }

        private void HandleSpinDamage()
        {
            hitTimer = hitCoolDown;
            var colliders = Physics2D.OverlapCircleAll(transform.position, 1f);

            foreach (var hit in colliders)
            {
                if (hit.TryGetComponent<Enemy>(out var enemy))
                {
                    SwordSkillDamage(enemy);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (isReturning) return;

            if (collision.TryGetComponent<Enemy>(out var enemy))
            {
                SwordSkillDamage(enemy);
            }

            SetupTargetForBounce(collision);
            StuckInto(collision);
        }

        private void SwordSkillDamage(Enemy enemy)
        {
            player.Stats.DoDamage(enemy.GetComponent<CharacterStats>());
        }

        private void SetupTargetForBounce(Collider2D collision)
        {
            if (!collision.TryGetComponent<Enemy>(out _)) return;
            if (!isBouncing || enemyTargets.Count > 0) return;

            var colliders = Physics2D.OverlapCircleAll(transform.position, 10f);
            foreach (var hit in colliders)
            {
                if (hit.TryGetComponent<Enemy>(out _))
                {
                    enemyTargets.Add(hit.transform);
                }
            }
        }

        private void StuckInto(Collider2D collision)
        {
            if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
            {
                pierceAmount--;
                return;
            }

            if (isSpinning)
            {
                StopWhenSpinning();
                return;
            }

            DisableSwordMovement();

            if (isBouncing && enemyTargets.Count > 0) return;

            animator.SetBool("Rotation", false);
            transform.parent = collision.transform;
        }

        private void DisableSwordMovement()
        {
            canRotate = false;
            circleCollider.enabled = false;
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        private void StopWhenSpinning()
        {
            wasStopped = true;
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
            spinTimer = spinDuration;
        }

        private void DestroyMe()
        {
            Destroy(gameObject);
        }
    }
}
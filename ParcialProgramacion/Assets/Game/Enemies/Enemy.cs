using System.Collections;
using Game.Enemies.StateMachine;
using Game.Shared.Scripts;
using UnityEngine;

namespace Game.Enemies
{
    public class Enemy : Entity
    {
        [SerializeField] protected LayerMask WhatIsPlayer;

        [Header("Stunned info")] public float stunnDuration;
        public Vector2 stunDirection;
        public bool canBeStunned;
        [SerializeField] protected GameObject counterImage;

        [Header("Move info")] public float moveSpeed;
        public float idleTime;
        public float battleTime;
        private float _defaultMoveSpeed;

        [Header("Attack info")] public float attackDistance;
        public float attackCooldown;
        [HideInInspector] public float lastTimeAttacked;

        public EnemyStateMachine EnemyStateMachine { get; private set; }
        public string lastAnimBoolName { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            EnemyStateMachine = new EnemyStateMachine();

            _defaultMoveSpeed = moveSpeed;
        }

        protected override void Update()
        {
            base.Update();

            EnemyStateMachine.CurrentState.Update();
        }

        public void AssingLastAnimName(string animBoolName) => lastAnimBoolName = animBoolName;

        public virtual void FreezeTimer(bool timeFrozen)
        {
            if (timeFrozen)
            {
                moveSpeed = 0;
                Anim.speed = 0;
            }
            else
            {
                moveSpeed = _defaultMoveSpeed;
                Anim.speed = 1;
            }
        }

        protected virtual IEnumerator FreezeTimeFor(float seconds)
        {
            FreezeTimer(true);

            yield return new WaitForSeconds(seconds);

            FreezeTimer(false);
        }

        public virtual void OpenCOunterAttackWindow()
        {
            canBeStunned = true;
            counterImage.SetActive(true);
        }

        public virtual void CloseCounterAttackWindow()
        {
            canBeStunned = false;
            counterImage.SetActive(false);
        }

        public virtual bool CanBeStunned()
        {
            if (!canBeStunned) return false;

            CloseCounterAttackWindow();
            return true;
        }

        public virtual void AnimationFinishTrigger() => EnemyStateMachine.CurrentState.AnimationFinishTrigger();

        public override void SlowEntityBy(float slowPercentage, float slowDuration)
        {
            moveSpeed = moveSpeed * (1 - slowPercentage);
            Anim.speed = Anim.speed * (1 - slowPercentage);

            Invoke("ReturnDefaultSpeed", slowDuration);
        }

        protected override void ReturnDefaultSpeed()
        {
            base.ReturnDefaultSpeed();
            moveSpeed = _defaultMoveSpeed;
        }

        public virtual RaycastHit2D IsPlayerDetected()
        {
            var playerDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDir, 50, WhatIsPlayer);
            var wallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDir, 50, whatIsGround);

            if (!wallDetected) return playerDetected;

            return wallDetected.distance < playerDetected.distance ? default(RaycastHit2D) : playerDetected;
        }


        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position,
                new Vector3(transform.position.x + attackDistance * FacingDir, transform.position.y));
        }
    }
}
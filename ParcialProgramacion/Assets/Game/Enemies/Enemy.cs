using System;
using System.Collections;
using Game.Enemies.StateMachine;
using Game.Managers;
using Game.Shared.Enums;
using Game.Shared.Scripts;
using Game.Spawning;
using Game.UI.Scripts;
using UnityEngine;

namespace Game.Enemies
{
    public class Enemy : Entity
    {
        #region Serialized Fields

        [Header("Detección y Movimiento")]
        [SerializeField] protected LayerMask WhatIsPlayer;
        [SerializeField] private float _moveSpeed = 2f;
        [SerializeField] private float _idleTime;
        [SerializeField] private float _battleTime;

        [Header("Ataque")]
        [SerializeField] private float _attackDistance = 1f;
        [SerializeField] private float _attackCooldown = 1f;

        [Header("Stun")]
        [SerializeField] private float _stunDuration;
        [SerializeField] private Vector2 _stunDirection;
        [SerializeField] private GameObject _counterImage;

        [Header("UI")]
        [SerializeField] private HealthBarUI _healthBarUI;

        #endregion

        #region Properties

        public EnemyStateMachine EnemyStateMachine { get; private set; }
        public float LastTimeAttacked { get; set; }
        public bool CanBeStunnedNow { get; private set; }
        public float MoveSpeed => _moveSpeed;
        public float AttackDistance => _attackDistance;
        public float AttackCooldown => _attackCooldown;
        public float StunDuration => _stunDuration;
        public Vector2 StunDirection => _stunDirection;
        public bool CanBeStunned => CanBeStunnedNow;
        public float BattleTime => _battleTime;
        public float IdleTime => _idleTime;
        public string LastAnimBoolName { get; private set; }
        public ObjectPoolEnemy Pool { get; private set; }
        public Action OnDeath;
        public bool WasDeadBeforeRespawn { get; set; }

        #endregion

        #region Private Fields

        private float _defaultMoveSpeed;

        #endregion

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();
            EnemyStateMachine = new EnemyStateMachine();
            _defaultMoveSpeed = _moveSpeed;
        }

        protected override void Update()
        {
            base.Update();
            if (GameManager.Instance.CurrentState != GameState.InGame) return;

            EnemyStateMachine.CurrentState.Update();
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right * AttackDistance * FacingDir);
        }

        #endregion

        #region Combat & Status

        public override void SlowEntityBy(float slowPercentage, float slowDuration)
        {
            _moveSpeed *= (1 - slowPercentage);
            Anim.speed *= (1 - slowPercentage);
            Invoke(nameof(ReturnDefaultSpeed), slowDuration);
        }

        protected override void ReturnDefaultSpeed()
        {
            base.ReturnDefaultSpeed();
            _moveSpeed = _defaultMoveSpeed;
        }

        public virtual void FreezeTimer(bool frozen)
        {
            _moveSpeed = frozen ? 0f : _defaultMoveSpeed;
            Anim.speed = frozen ? 0f : 1f;
        }

        public IEnumerator FreezeTimeFor(float seconds)
        {
            FreezeTimer(true);
            yield return new WaitForSeconds(seconds);
            FreezeTimer(false);
        }

        public virtual RaycastHit2D IsPlayerDetected()
        {
            var rayPlayer = Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDir, 50f, WhatIsPlayer);
            var rayWall = Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDir, 50f, whatIsGround);

            if (!rayWall) return rayPlayer;
            return rayWall.distance < rayPlayer.distance ? default : rayPlayer;
        }

        public virtual void AnimationFinishTrigger()
        {
            EnemyStateMachine.CurrentState.AnimationFinishTrigger();
        }

        public void AssignLastAnimName(string animBoolName)
        {
            LastAnimBoolName = animBoolName;
        }

        #endregion

        #region Stun / Counter Logic

        public void OpenCounterAttackWindow()
        {
            CanBeStunnedNow = true;
            _counterImage.SetActive(true);
        }

        public void CloseCounterAttackWindow()
        {
            CanBeStunnedNow = false;
            _counterImage.SetActive(false);
        }

        public bool TryStun()
        {
            if (!CanBeStunnedNow) return false;
            CloseCounterAttackWindow();
            return true;
        }

        #endregion

        #region Pool & UI

        public void SetPool(ObjectPoolEnemy pool) => Pool = pool;

        public void ResetUIHealth() => _healthBarUI.ResetHealtUIValue();

        public void SubscribeOnDeath(Action callback) => OnDeath += callback;

        protected virtual void ReturnToPool() { }

        #endregion
    }
}

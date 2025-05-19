using Game.Enemies.Skeleton.Scripts.States;
using Game.Managers;
using UnityEngine;

namespace Game.Enemies.Skeleton.Scripts
{
    public class EnemySkeleton : Enemy
    {
        #region States

        public SkeletonIdleState IdleState { get; private set; }
        public SkeletonMoveState MoveState { get; private set; }
        public SkeletonBattleState BattleState { get; private set; }
        public SkeletonAttackState AttackState { get; private set; }
        public SkeletonStunnedState StunnedState { get; private set; }
        public SkeletonDeadState DeadState { get; private set; }

        #endregion

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();

            IdleState = new SkeletonIdleState(this, EnemyStateMachine, "Idle", this);
            MoveState = new SkeletonMoveState(this, EnemyStateMachine, "Move", this);
            BattleState = new SkeletonBattleState(this, EnemyStateMachine, "Move", this); // ¿Anim "Battle"?
            AttackState = new SkeletonAttackState(this, EnemyStateMachine, "Attack", this);
            StunnedState = new SkeletonStunnedState(this, EnemyStateMachine, "Stunned", this);
            DeadState = new SkeletonDeadState(this, EnemyStateMachine, "Die", this);
        }

        protected override void Start()
        {
            base.Start();
            EnemyStateMachine.Initialize(IdleState);
        }

        protected override void Update()
        {
            base.Update();

#if UNITY_EDITOR
            // Atajo para testear cambio de estado a Stunned
            if (Input.GetKeyDown(KeyCode.U))
                EnemyStateMachine.ChangeState(StunnedState);
#endif
        }

        #endregion

        #region Overrides

        public bool TryStun()
        {
            if (!CanBeStunnedNow) return false;

            CloseCounterAttackWindow();
            return true;
        }

        public override void Die()
        {
            base.Die();
            EnemyStateMachine.ChangeState(DeadState);
        }

        #endregion

        #region Restart / Pool

        public void NotifyDead()
        {
            GameManager.Instance.EnemyKilled();
            ResetHealthUI();
            SetStateToDefault();
            OnDeath?.Invoke();
            ReturnToPool();
        }

        protected override void ReturnToPool()
        {
            Pool.ReturnObject(gameObject);
        }

        private void ResetHealthUI() => Stats.ResetHealth();
        private void SetStateToDefault() => EnemyStateMachine.ChangeState(IdleState);

        #endregion
    }
}

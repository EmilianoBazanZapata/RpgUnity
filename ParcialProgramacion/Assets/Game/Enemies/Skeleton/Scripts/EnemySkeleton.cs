using Game.Enemies.Skeleton.Scripts.States;
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
    
        protected override void Awake()
        {
            base.Awake();

            IdleState = new SkeletonIdleState(this, EnemyStateMachine, "Idle", this);
            MoveState = new SkeletonMoveState(this, EnemyStateMachine, "Move", this);
            BattleState = new SkeletonBattleState(this, EnemyStateMachine, "Move", this);
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
        
            if(Input.GetKeyDown(KeyCode.U))
                EnemyStateMachine.ChangeState(StunnedState);
        }

        public override bool CanBeStunned()
        {
            if (!base.CanBeStunned()) return false;
            
            EnemyStateMachine.ChangeState(StunnedState);
            return true;
        }

        public override void Die()
        {
            base.Die();
        
            EnemyStateMachine.ChangeState(DeadState);
            
            OnDeath?.Invoke();
        }
    }
}
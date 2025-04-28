using Game.Enemies.StateMachine;
using UnityEngine;

namespace Game.Enemies.Skeleton.Scripts.States
{
    public class SkeletonAttackState : EnemyState
    {
        private EnemySkeleton _enemySkeleton;
        
        public SkeletonAttackState(Enemy enemy, 
                                   EnemyStateMachine enemyStateMachine, 
                                   string animBoolName,
                                   EnemySkeleton enemySkeleton) : base(enemy, enemyStateMachine, animBoolName)
        {
            _enemySkeleton = enemySkeleton;
        }
        public override void Exit()
        {
            base.Exit();

            _enemySkeleton.lastTimeAttacked = Time.time; 
        }

        public override void Update()
        {
            base.Update();

            _enemySkeleton.SetZeroVelocity();

            if (TriggerCalled)
                EnemyStateMachine.ChangeState(_enemySkeleton.BattleState);
        }
    }
}
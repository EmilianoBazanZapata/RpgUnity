using Game.Enemies.StateMachine;
using UnityEngine;

namespace Game.Enemies.Skeleton.Scripts.States
{
    public class SkeletonStunnedState : EnemyState
    {
        private EnemySkeleton _enemySkeleton;
        public SkeletonStunnedState(Enemy enemy, 
                                    EnemyStateMachine enemyStateMachine, 
                                    string animBoolName, 
                                    EnemySkeleton enemySkeleton) : base(enemy, enemyStateMachine, animBoolName)
        {
            _enemySkeleton = enemySkeleton;
        }

        public override void Enter()
        {
            base.Enter();
        
            StateTimer =_enemySkeleton.StunDuration;
        
            Rigidbody2D.velocity = new Vector2(-_enemySkeleton.FacingDir * _enemySkeleton.StunDirection.x, _enemySkeleton.StunDirection.y);
        }
        
        public override void Update()
        {
            base.Update();
        
            if(StateTimer < 0)
                EnemyStateMachine.ChangeState(_enemySkeleton.IdleState);
        }
    }
}
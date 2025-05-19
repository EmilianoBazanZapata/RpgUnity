using Game.Enemies.StateMachine;
using UnityEngine;

namespace Game.Enemies.Skeleton.Scripts.States
{
    public class SkeletonGroundedState : EnemyState
    {
        protected EnemySkeleton EnemySkeleton;
        protected Transform PlayerTransform;

        public SkeletonGroundedState(Enemy enemy,
            EnemyStateMachine enemyStateMachine,
            string animBoolName,
            EnemySkeleton enemySkeleton)
            : base(enemy, enemyStateMachine, animBoolName)
        {
            EnemySkeleton = enemySkeleton;
        }

        public override void Enter()
        {
            base.Enter();
            
            PlayerTransform = Character.Scripts.Player.Instance.transform;
        }

        public override void Update()
        {
            base.Update();

            bool isPlayerDetected = EnemySkeleton.IsPlayerDetected();
            var distanceToPlayer = Vector2.Distance(EnemySkeleton.transform.position, PlayerTransform.position);

            if (isPlayerDetected || distanceToPlayer < EnemySkeleton.agroDistance)
            {
                EnemyStateMachine.ChangeState(EnemySkeleton.BattleState);
            }
        }
    }
}
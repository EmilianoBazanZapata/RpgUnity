using Game.Enemies.StateMachine;
using Game.Player.Scripts.Managers;
using UnityEngine;

namespace Game.Enemies.Skeleton.Scripts.States
{
    public class SkeletonGroundedState : EnemyState
    {
        protected EnemySkeleton EnemySkeleton;
        protected Transform Player;

        public SkeletonGroundedState(Enemy enemy, 
                                     EnemyStateMachine enemyStateMachine, 
                                     string animBoolName,
                                     EnemySkeleton enemySkeleton) : base(enemy, enemyStateMachine, animBoolName)
        {
            EnemySkeleton = enemySkeleton;
        }

        public override void Enter()
        {
            base.Enter();
            Player = PlayerManager.Instance.Player.transform;
        }

        public override void Update()
        {
            base.Update();

            if (EnemySkeleton.IsPlayerDetected() ||
                Vector2.Distance(EnemySkeleton.transform.position, Player.position) < 2)
                EnemyStateMachine.ChangeState(EnemySkeleton.BattleState);

            if (EnemySkeleton.IsPlayerDetected() ||
                Vector2.Distance(EnemySkeleton.transform.position, Player.transform.position) <
                EnemySkeleton.agroDistance)
                EnemyStateMachine.ChangeState(EnemySkeleton.BattleState);
        }
    }
}
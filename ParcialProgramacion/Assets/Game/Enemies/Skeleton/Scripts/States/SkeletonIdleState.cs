using Game.Enemies.StateMachine;

namespace Game.Enemies.Skeleton.Scripts.States
{
    public class SkeletonIdleState : SkeletonGroundedState
    {
        public SkeletonIdleState(Enemy enemy, 
                                 EnemyStateMachine enemyStateMachine, 
                                 string animBoolName, 
                                 EnemySkeleton enemySkeleton) : base(enemy, enemyStateMachine, animBoolName, enemySkeleton)
        {
        }
        
        
        public override void Enter()
        {
            base.Enter();
            StateTimer = EnemySkeleton.idleTime;
        }
        
        public override void Update()
        {
            base.Update();
        
            if (StateTimer < 0)
                EnemyStateMachine.ChangeState(EnemySkeleton.MoveState);   
        }
    }
}
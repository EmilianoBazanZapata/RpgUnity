using Game.Enemies.StateMachine;

namespace Game.Enemies.Skeleton.Scripts.States
{
    public class SkeletonMoveState : SkeletonGroundedState
    {
        public SkeletonMoveState(Enemy enemy,
                                 EnemyStateMachine enemyStateMachine, 
                                 string animBoolName,
                                 EnemySkeleton enemySkeleton) : base(enemy, enemyStateMachine, animBoolName, enemySkeleton)
        {
        }

        public override void Update()
        {
            base.Update();

            EnemySkeleton.SetVelocity(EnemySkeleton.MoveSpeed * EnemySkeleton.FacingDir, Rigidbody2D.velocity.y);

            if (!EnemySkeleton.IsWallDetected() && EnemySkeleton.IsGroundDetected()) return;
            
            EnemySkeleton.Flip();
            EnemyStateMachine.ChangeState(EnemySkeleton.IdleState);
        }
    }
}
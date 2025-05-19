using Game.Enemies.StateMachine;
using Game.Managers;
using Game.Shared.Enums;

namespace Game.Enemies.Skeleton.Scripts.States
{
    public class SkeletonDeadState : EnemyState
    {
        private readonly EnemySkeleton _enemySkeleton;

        public SkeletonDeadState(Enemy enemyBase,
            EnemyStateMachine enemyStateMachine,
            string animBoolName,
            EnemySkeleton enemySkeleton)
            : base(enemyBase, enemyStateMachine, animBoolName)
        {
            _enemySkeleton = enemySkeleton;
        }

        public override void Enter()
        {
            base.Enter();

            SoundManager.Instance.PlaySound(SoundType.EnemyDeath);
        }
    }
}
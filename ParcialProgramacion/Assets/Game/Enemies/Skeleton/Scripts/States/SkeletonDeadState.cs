using Game.Enemies.StateMachine;
using Game.Managers;
using Game.Shared.Enums;

namespace Game.Enemies.Skeleton.Scripts.States
{
    public class SkeletonDeadState : EnemyState
    {
        private EnemySkeleton _enemy;

        public SkeletonDeadState(Enemy enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName,
            EnemySkeleton enemy) : base(enemyBase, enemyStateMachine, animBoolName)
        {
            _enemy = enemy;
        }

        public override void Enter()
        {
            base.Enter();   
            
            SoundManager.Instance.PlaySound(SoundType.EnemyDeath);
        }
    }
}
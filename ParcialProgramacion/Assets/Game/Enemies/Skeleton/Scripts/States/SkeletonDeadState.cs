using Game.Enemies.StateMachine;
using UnityEngine;

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
            
            _enemy.Collider2D.enabled = false;
            _enemy.Rb.bodyType = RigidbodyType2D.Static;
            
            _enemy.HealthBarUI.SetActive(false);
        }
    }
}
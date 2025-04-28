using Game.Enemies.StateMachine;
using Game.Player.Scripts.Managers;
using UnityEngine;

namespace Game.Enemies.Skeleton.Scripts.States
{
    public class SkeletonBattleState : EnemyState
    {
        private Transform _player;
        private EnemySkeleton _enemySkeleton;
        private int _moveDir;
        
        public SkeletonBattleState(Enemy enemy, 
                                   EnemyStateMachine enemyStateMachine, 
                                   string animBoolName,
                                   EnemySkeleton enemySkeleton) : base(enemy, enemyStateMachine, animBoolName)
        {
            _enemySkeleton = enemySkeleton;
        }
        
        public override void Enter()
        {
            base.Enter();
            _player = PlayerManager.Instance.Player.transform;
        }
        
        public override void Update()
        {
            base.Update();

            if (_enemySkeleton.IsPlayerDetected())
            {
                StateTimer = _enemySkeleton.battleTime;
            
                if (_enemySkeleton.IsPlayerDetected().distance < _enemySkeleton.attackDistance)
                    if (CanAttack())
                        EnemyStateMachine.ChangeState(_enemySkeleton.AttackState);
            }
            else
            {
                if(StateTimer < 0 || Vector2.Distance(_player.transform.position, _enemySkeleton.transform.position) > 10)
                    EnemyStateMachine.ChangeState(_enemySkeleton.IdleState);
            }

            if (_player.position.x > _enemySkeleton.transform.position.x)
                _moveDir = 1;
            else if (_player.position.x < _enemySkeleton.transform.position.x)
                _moveDir = -1;

            _enemySkeleton.SetVelocity(_enemySkeleton.moveSpeed * _moveDir, Rigidbody2D.velocity.y);
        }

        private bool CanAttack()
        {
            if (!(Time.time >= _enemySkeleton.lastTimeAttacked + _enemySkeleton.attackCooldown)) return false;
            
            _enemySkeleton.lastTimeAttacked = Time.time;
            
            return true;
        }
    }
}
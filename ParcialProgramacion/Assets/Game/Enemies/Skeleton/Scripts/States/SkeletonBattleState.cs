using Game.Enemies.StateMachine;
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
            _player = Character.Scripts.Player.Instance.transform;
        }
        
        public override void Update()
        {
            base.Update();

            if (_enemySkeleton.IsPlayerDetected())
            {
                StateTimer = _enemySkeleton.BattleTime;
            
                if (_enemySkeleton.IsPlayerDetected().distance < _enemySkeleton.AttackDistance)
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

            _enemySkeleton.SetVelocity(_enemySkeleton.MoveSpeed * _moveDir, Rigidbody2D.velocity.y);
        }

        private bool CanAttack()
        {
            if (!(Time.time >= _enemySkeleton.LastTimeAttacked + _enemySkeleton.AttackCooldown)) return false;
            
            _enemySkeleton.LastTimeAttacked = Time.time;
            
            return true;
        }
    }
}
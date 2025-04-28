using Game.Shared.StateMachines.Interfaces;
using UnityEngine;

namespace Game.Enemies.StateMachine
{
    public class EnemyState : IState
    {
        
        protected EnemyStateMachine EnemyStateMachine;
        protected Enemy Enemy;
        protected bool TriggerCalled;
        protected float StateTimer;
        protected Rigidbody2D Rigidbody2D;
    
        private string _animBoolName;
        
        
        public EnemyState(Enemy enemy, EnemyStateMachine enemyStateMachine, string animBoolName)
        {
            this.Enemy = enemy;
            this.EnemyStateMachine = enemyStateMachine;
            this._animBoolName = animBoolName;
        }

        public virtual void Enter()
        {
            TriggerCalled = false;
            Rigidbody2D = Enemy.Rb;
            Enemy.Anim.SetBool(_animBoolName, true);
        }

        public virtual void Exit()
        {
            Enemy.Anim.SetBool(_animBoolName, false);
            Enemy.AssingLastAnimName(_animBoolName);
        }
    
        public virtual void Update()
        {
            StateTimer -= Time.deltaTime;
        }

        public virtual void AnimationFinishTrigger()
        {
            TriggerCalled = true;
        }
    }
}
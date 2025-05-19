using Game.Shared.StateMachines.Interfaces;
using UnityEngine;

namespace Game.Enemies.StateMachine
{
    /// <summary>
    /// Clase base para los estados de los enemigos. Implementa IState.
    /// </summary>
    public class EnemyState : IState
    {
        #region Protected Fields

        protected Enemy Enemy;
        protected EnemyStateMachine EnemyStateMachine;
        protected Rigidbody2D Rigidbody2D;
        protected float StateTimer;
        protected bool TriggerCalled;

        #endregion

        #region Private Fields

        private readonly string _animBoolName;

        #endregion

        #region Constructor

        public EnemyState(Enemy enemy, EnemyStateMachine enemyStateMachine, string animBoolName)
        {
            Enemy = enemy;
            EnemyStateMachine = enemyStateMachine;
            _animBoolName = animBoolName;
        }

        #endregion

        #region IState Methods

        public virtual void Enter()
        {
            TriggerCalled = false;
            Rigidbody2D = Enemy.Rb;

            Enemy.Anim.SetBool(_animBoolName, true);
        }

        public virtual void Update()
        {
            StateTimer -= Time.deltaTime;
        }

        public virtual void Exit()
        {
            Enemy.Anim.SetBool(_animBoolName, false);
            Enemy.AssignLastAnimName(_animBoolName);
        }

        public virtual void AnimationFinishTrigger()
        {
            TriggerCalled = true;
        }

        #endregion
    }
}
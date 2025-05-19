using Game.Shared.StateMachines.Interfaces;
using UnityEngine;

namespace Game.Character.Scripts.StateMachine
{
    /// <summary>
    /// Clase base para los estados del jugador. Controla entrada, animación y físicas.
    /// </summary>
    public abstract class PlayerState : IState
    {
        protected readonly PlayerStateMachine StateMachine;
        protected readonly Player Player;

        protected float XInput;
        protected float YInput;
        protected float StateTimer;
        protected bool TriggerCalled;

        protected readonly string AnimBoolName;

        protected Rigidbody2D Rigidbody2D { get; private set; }
        protected Animator Animator { get; private set; }

        public PlayerState(Player player, PlayerStateMachine stateMachine, string animBoolName)
        {
            Player = player;
            StateMachine = stateMachine;
            AnimBoolName = animBoolName;
        }

        public virtual void Enter()
        {
            Rigidbody2D = Player.Rb;
            Animator = Player.Anim;

            Animator.SetBool(AnimBoolName, true);
            TriggerCalled = false;
        }

        public virtual void Update()
        {
            StateTimer -= Time.deltaTime;
            HandleInput();

            Animator.SetFloat("yVelocity", Rigidbody2D.velocity.y);
        }

        public virtual void Exit()
        {
            Animator.SetBool(AnimBoolName, false);
        }

        public virtual void AnimationFinishTrigger()
        {
            TriggerCalled = true;
        }

        /// <summary>
        /// Obtiene el input del jugador (horizontal y vertical).
        /// </summary>
        protected virtual void HandleInput()
        {
            XInput = Player.InputHandler.XInput;
            YInput = Player.InputHandler.YInput;
        }
    }
}
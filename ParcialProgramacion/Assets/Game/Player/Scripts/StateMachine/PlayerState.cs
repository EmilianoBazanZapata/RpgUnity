using Game.Shared.StateMachines.Interfaces;
using UnityEngine;

namespace Game.Player.Scripts.StateMachine
{
    /// <summary>
    /// Clase base para los estados del jugador.
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

        public PlayerState(Player player, PlayerStateMachine stateMachine, string animBoolName)
        {
            Player = player;
            StateMachine = stateMachine;
            AnimBoolName = animBoolName;
        }

        public virtual void Enter()
        {
            Player.Anim.SetBool(AnimBoolName, true);
            TriggerCalled = false;
        }

        public virtual void Update()
        {
            StateTimer -= Time.deltaTime;
            XInput = Player.InputHandler.XInput;
            YInput = Player.InputHandler.YInput;
            Player.Anim.SetFloat("yVelocity", Player.Rb.velocity.y);
        }

        public virtual void Exit()
        {
            Player.Anim.SetBool(AnimBoolName, false);
        }

        public virtual void AnimationFinishTrigger()
        {
            TriggerCalled = true;
        }
    }
}
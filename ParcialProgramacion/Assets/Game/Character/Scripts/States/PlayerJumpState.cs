using Game.Character.Scripts.StateMachine;
using UnityEngine;

namespace Game.Character.Scripts.States
{
    /// <summary>
    /// Estado que se activa al realizar un salto.
    /// Aplica una fuerza vertical y cambia a aire cuando comienza a caer.
    /// </summary>
    public class PlayerJumpState : PlayerState
    {
        public PlayerJumpState(Player player,
            PlayerStateMachine stateMachine,
            string animBoolName) : base(player, stateMachine, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            ApplyJumpForce();
        }

        public override void Update()
        {
            base.Update();
            CheckForFallingTransition();
        }

        /// <summary>
        /// Aplica la fuerza de salto vertical y registra el salto realizado.
        /// </summary>
        private void ApplyJumpForce()
        {
            Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, Player.JumpForce);
            Player.JumpCount++;
        }

        /// <summary>
        /// Cambia al estado de aire si el jugador comienza a caer.
        /// </summary>
        private void CheckForFallingTransition()
        {
            if (Rigidbody2D.velocity.y < 0)
                StateMachine.ChangeState(Player.AirState);
        }
    }
}
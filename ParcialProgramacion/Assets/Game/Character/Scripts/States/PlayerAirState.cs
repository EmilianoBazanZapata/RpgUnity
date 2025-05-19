using Game.Character.Scripts.StateMachine;
using UnityEngine;

namespace Game.Character.Scripts.States
{
    /// <summary>
    /// Estado del jugador cuando está en el aire (caída o después de saltar).
    /// Permite movimiento lateral y salto extra si disponible.
    /// </summary>
    public class PlayerAirState : PlayerState
    {
        public PlayerAirState(Player player,
            PlayerStateMachine stateMachine,
            string animBoolName) : base(player, stateMachine, animBoolName)
        {
        }

        public override void Update()
        {
            base.Update();
            HandleTransitions();
            HandleAirMovement();
        }

        /// <summary>
        /// Cambia de estado dependiendo de si el jugador cae o salta otra vez.
        /// </summary>
        private void HandleTransitions()
        {
            if (Input.GetKeyDown(KeyCode.Space) &&
                !Player.IsGroundDetected() &&
                Player.JumpCount < Player.MaxJumpCount)
            {
                StateMachine.ChangeState(Player.JumpState);
                return;
            }

            if (Player.IsGroundDetected())
            {
                StateMachine.ChangeState(Player.IdleState);
            }
        }

        /// <summary>
        /// Permite al jugador moverse horizontalmente en el aire.
        /// </summary>
        private void HandleAirMovement()
        {
            if (XInput == 0) return;
            var horizontalSpeed = Player.MoveSpeed * 0.8f * XInput;
            Player.SetVelocity(horizontalSpeed, Rigidbody2D.velocity.y);
        }
    }
}
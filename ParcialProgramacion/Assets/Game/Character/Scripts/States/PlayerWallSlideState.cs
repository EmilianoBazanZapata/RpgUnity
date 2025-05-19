using Game.Character.Scripts.StateMachine;
using UnityEngine;

namespace Game.Character.Scripts.States
{
    /// <summary>
    /// Estado cuando el jugador se desliza por una pared.
    /// Permite transicionar a salto desde pared o a Idle si toca el suelo o se aleja de la pared.
    /// </summary>
    public class PlayerWallSlideState : PlayerState
    {
        public PlayerWallSlideState(Player player, 
            PlayerStateMachine stateMachine, 
            string animBoolName) : base(player, stateMachine, animBoolName)
        {
        }

        public override void Update()
        {
            base.Update();
            HandleStateTransitions();
            ApplySlideVelocity();
        }

        /// <summary>
        /// Controla los cambios de estado posibles mientras se desliza.
        /// </summary>
        private void HandleStateTransitions()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StateMachine.ChangeState(Player.WallJumpState);
                return;
            }

            // Si el jugador se aleja de la pared, salir del estado
            if (XInput != 0 && Player.FacingDir != XInput)
            {
                StateMachine.ChangeState(Player.IdleState);
                return;
            }

            if (Player.IsGroundDetected())
            {
                StateMachine.ChangeState(Player.IdleState);
            }
        }

        /// <summary>
        /// Aplica una reducción controlada en la caída mientras se desliza.
        /// </summary>
        private void ApplySlideVelocity()
        {
            float verticalSpeed = YInput < 0 ? Rigidbody2D.velocity.y : Rigidbody2D.velocity.y * 0.7f;
            Rigidbody2D.velocity = new Vector2(0, verticalSpeed);
        }
    }
}
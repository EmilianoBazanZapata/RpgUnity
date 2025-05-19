using Game.Character.Scripts.StateMachine;

namespace Game.Character.Scripts.States
{
    /// <summary>
    /// Estado que se activa cuando el jugador salta desde una pared.
    /// Aplica un impulso contrario y cambia a aire o idle según contacto.
    /// </summary>
    public class PlayerWallJumpState : PlayerState
    {
        public PlayerWallJumpState(Player player, 
            PlayerStateMachine stateMachine, 
            string animBoolName) : base(player, stateMachine, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            PerformWallJump();
        }

        public override void Update()
        {
            base.Update();
            HandleStateTransitions();
        }

        /// <summary>
        /// Aplica la velocidad inicial del salto desde la pared y reinicia el temporizador.
        /// </summary>
        private void PerformWallJump()
        {
            StateTimer = 1f;
            Player.SetVelocity(5 * -Player.FacingDir, Player.JumpForce);
        }

        /// <summary>
        /// Cambia de estado dependiendo del tiempo en el aire o si se toca el suelo.
        /// </summary>
        private void HandleStateTransitions()
        {
            if (StateTimer < 0)
            {
                StateMachine.ChangeState(Player.AirState);
                return;
            }

            if (Player.IsGroundDetected())
            {
                StateMachine.ChangeState(Player.IdleState);
            }
        }
    }
}
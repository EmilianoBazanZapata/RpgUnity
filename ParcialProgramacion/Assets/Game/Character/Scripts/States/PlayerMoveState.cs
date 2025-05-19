using Game.Character.Scripts.StateMachine;

namespace Game.Character.Scripts.States
{
    /// <summary>
    /// Estado cuando el jugador se está moviendo en el suelo.
    /// Se mantiene activo mientras haya input horizontal y no haya colisión con pared.
    /// </summary>
    public class PlayerMoveState : PlayerGroundedState
    {
        public PlayerMoveState(Player player,
            PlayerStateMachine stateMachine,
            string animBoolName) : base(player, stateMachine, animBoolName)
        {
        }

        public override void Update()
        {
            base.Update();
            Move();
            CheckForIdleTransition();
        }

        /// <summary>
        /// Aplica velocidad horizontal al jugador basada en el input.
        /// </summary>
        private void Move()
        {
            Player.SetVelocity(XInput * Player.MoveSpeed, Rigidbody2D.velocity.y);
        }

        /// <summary>
        /// Si no hay input o hay una pared en la dirección de movimiento, cambiar a Idle.
        /// </summary>
        private void CheckForIdleTransition()
        {
            if (XInput == 0 || Player.IsWallDetected())
                StateMachine.ChangeState(Player.IdleState);
        }
    }
}
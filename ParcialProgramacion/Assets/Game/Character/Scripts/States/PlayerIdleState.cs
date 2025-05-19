using Game.Character.Scripts.StateMachine;

namespace Game.Character.Scripts.States
{
    /// <summary>
    /// Estado del jugador cuando está quieto en el suelo.
    /// Cambia a movimiento si se presiona dirección.
    /// </summary>
    public class PlayerIdleState : PlayerGroundedState
    {
        public PlayerIdleState(Player player, 
            PlayerStateMachine stateMachine, 
            string animBoolName) : base(player, stateMachine, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Player.SetZeroVelocity();
        }

        public override void Update()
        {
            base.Update();
            CheckForMovementTransition();
        }

        /// <summary>
        /// Cambia al estado de movimiento si hay input y no está ocupado.
        /// </summary>
        private void CheckForMovementTransition()
        {
            if (Player.IsBusy)
                return;

            if (Player.IsWallDetected() && XInput == Player.FacingDir)
                return;

            if (XInput != 0)
                StateMachine.ChangeState(Player.MoveState);
        }
    }
}
using Game.Character.Scripts.StateMachine;

namespace Game.Character.Scripts.States
{
    /// <summary>
    /// Estado de dash del jugador. Aplica velocidad horizontal durante un tiempo definido.
    /// </summary>
    public class PlayerDashState : PlayerState
    {
        public PlayerDashState(Player player,
            PlayerStateMachine stateMachine,
            string animBoolName) : base(player, stateMachine, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            StartDash();
        }

        public override void Update()
        {
            base.Update();
            ContinueDash();
        }

        public override void Exit()
        {
            base.Exit();
            StopDash();
        }

        /// <summary>
        /// Inicializa la duración y comienza el dash.
        /// </summary>
        private void StartDash()
        {
            StateTimer = Player.DashDuration;
        }

        /// <summary>
        /// Aplica la velocidad del dash y verifica el final del estado.
        /// </summary>
        private void ContinueDash()
        {
            Player.SetVelocity(Player.DashSpeed * Player.DashDir, 0);

            if (StateTimer < 0)
                StateMachine.ChangeState(Player.IdleState);
        }

        /// <summary>
        /// Detiene el movimiento horizontal al salir del dash.
        /// </summary>
        private void StopDash()
        {
            Player.SetVelocity(0, Rigidbody2D.velocity.y);
        }
    }
}
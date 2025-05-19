using Game.Character.Scripts.StateMachine;
using Game.Managers;

namespace Game.Character.Scripts.States
{
    /// <summary>
    /// Estado de muerte del jugador. Detiene su movimiento y notifica al GameManager.
    /// </summary>
    public class PlayerDeadState : PlayerState
    {
        public PlayerDeadState(Player player,
            PlayerStateMachine stateMachine,
            string animBoolName) : base(player, stateMachine, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            HandleDeath();
        }
        
        /// <summary>
        /// Lógica de muerte: detiene el movimiento y notifica la derrota.
        /// </summary>
        private void HandleDeath()
        {
            Player.SetZeroVelocity();
            GameManager.Instance.LoseGame();
        }
    }
}
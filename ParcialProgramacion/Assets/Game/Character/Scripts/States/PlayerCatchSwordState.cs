using Game.Character.Scripts.StateMachine;
using UnityEngine;

namespace Game.Character.Scripts.States
{
    /// <summary>
    /// Estado en el que el jugador atrapa la espada.
    /// Ejecuta animación, se orienta y genera un impulso.
    /// </summary>
    public class PlayerCatchSwordState : PlayerState
    {
        private Transform _sword;

        public PlayerCatchSwordState(Player player,
            PlayerStateMachine stateMachine,
            string animBoolName) : base(player, stateMachine, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            if (Player.Sword == null) return;

            _sword = Player.Sword.transform;

            OrientPlayerTowardSword();
            ApplyCatchMomentum();
        }

        public override void Update()
        {
            base.Update();

            if (TriggerCalled)
                StateMachine.ChangeState(Player.IdleState);
        }

        public override void Exit()
        {
            base.Exit();
            Player.StartCoroutine(Player.BusyFor(0.1f));
        }

        /// <summary>
        /// Si el jugador está mirando en dirección opuesta a la espada, se voltea.
        /// </summary>
        private void OrientPlayerTowardSword()
        {
            var deltaX = Player.transform.position.x - _sword.position.x;
            if ((deltaX > 0 && Player.FacingDir == 1) || (deltaX < 0 && Player.FacingDir == -1))
            {
                Player.Flip();
            }
        }

        /// <summary>
        /// Aplica una fuerza de retroceso al atrapar la espada.
        /// </summary>
        private void ApplyCatchMomentum()
        {
            Rigidbody2D.velocity = new Vector2(
                Player.SwordReturnImpact * -Player.FacingDir,
                Rigidbody2D.velocity.y
            );
        }
    }
}
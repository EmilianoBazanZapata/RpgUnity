using Game.Character.Scripts.Controllers;
using Game.Character.Scripts.StateMachine;
using UnityEngine;

namespace Game.Character.Scripts.States
{
    /// <summary>
    /// Estado base para cuando el jugador está en el suelo.
    /// Desde aquí puede saltar, atacar o preparar el lanzamiento de la espada.
    /// </summary>
    public class PlayerGroundedState : PlayerState
    {
        public PlayerGroundedState(Player player,
            PlayerStateMachine stateMachine,
            string animBoolName) : base(player, stateMachine, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Player.JumpCount = 0;
        }

        public override void Update()
        {
            base.Update();
            HandleTransitions();
        }

        /// <summary>
        /// Evalúa las transiciones posibles desde el estado en tierra.
        /// </summary>
        private void HandleTransitions()
        {
            var isGrounded = Player.IsGroundDetected();

            if (!isGrounded)
            {
                StateMachine.ChangeState(Player.AirState);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                StateMachine.ChangeState(Player.JumpState);
                return;
            }

            if (Input.GetKey(KeyCode.Mouse0))
            {
                StateMachine.ChangeState(Player.PrimaryAttackState);
                return;
            }

            if (Input.GetKey(KeyCode.Mouse1) && TryReturnSwordIfHeld())
            {
                StateMachine.ChangeState(Player.AimSwordState);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                StateMachine.ChangeState(Player.CounterAttackState);
            }
        }

        /// <summary>
        /// Devuelve la espada si está presente y activa el estado de apuntado.
        /// </summary>
        private bool TryReturnSwordIfHeld()
        {
            if (Player.Sword == null)
                return true;

            if (!Player.Sword.TryGetComponent(out SwordSkillController controller)) return true;
            
            controller.ReturnSword();
            return false;

        }
    }
}

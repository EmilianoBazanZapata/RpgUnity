using Game.Character.Scripts.StateMachine;
using UnityEngine;

namespace Game.Character.Scripts.States
{
    /// <summary>
    /// Estado en el que el jugador apunta la dirección para lanzar la espada.
    /// Se activa al mantener presionado Mouse1 y muestra una trayectoria.
    /// </summary>
    public class PlayerAimSwordState : PlayerState
    {
        public PlayerAimSwordState(Player player,
            PlayerStateMachine stateMachine,
            string animBoolName) : base(player, stateMachine, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Player.Skill.SwordSkill.DotsActive(true);
        }

        public override void Update()
        {
            base.Update();

            Player.SetZeroVelocity();
            HandleExitInput();
            HandleAimingDirection();
        }

        public override void Exit()
        {
            base.Exit();
            Player.StartCoroutine(Player.BusyFor(0.2f));
        }

        /// <summary>
        /// Detecta si se soltó el botón de apuntado para salir del estado.
        /// </summary>
        private void HandleExitInput()
        {
            if (Input.GetKeyUp(KeyCode.Mouse1))
                StateMachine.ChangeState(Player.IdleState);
        }

        /// <summary>
        /// Invierte la dirección del jugador si el mouse apunta en dirección opuesta.
        /// </summary>
        private void HandleAimingDirection()
        {
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            var shouldFlipLeft = Player.transform.position.x > mouseWorldPos.x && Player.FacingDir == 1;
            var shouldFlipRight = Player.transform.position.x < mouseWorldPos.x && Player.FacingDir == -1;

            if (shouldFlipLeft || shouldFlipRight)
                Player.Flip();
        }
    }
}
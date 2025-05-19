using Game.Character.Scripts.StateMachine;
using Game.Managers;
using Game.Shared.Enums;
using UnityEngine;

namespace Game.Character.Scripts.States
{
    /// <summary>
    /// Estado del jugador al ejecutar un ataque primario.
    /// Maneja combos, animación y movimiento de impulso.
    /// </summary>
    public class PlayerPrimaryAttackState : PlayerState
    {
        public int ComboCounter;
        private float _lastTimeAttacked;
        private float _comboWindow = 2f;

        public PlayerPrimaryAttackState(Player player,
            PlayerStateMachine stateMachine,
            string animBoolName) : base(player, stateMachine, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            XInput = 0;

            HandleComboReset();
            SetAttackAnimation();
            ApplyAttackMovement();
            PlayAttackSound();

            StateTimer = 0.1f;
        }

        public override void Update()
        {
            base.Update();

            if (StateTimer < 0)
                Player.SetZeroVelocity();

            if (TriggerCalled)
                StateMachine.ChangeState(Player.IdleState);
        }

        public override void Exit()
        {
            base.Exit();
            Player.StartCoroutine(Player.BusyFor(0.15f));

            ComboCounter++;
            _lastTimeAttacked = Time.time;
        }

        /// <summary>
        /// Reinicia el combo si pasó demasiado tiempo desde el último ataque.
        /// </summary>
        private void HandleComboReset()
        {
            if (ComboCounter > 2 || Time.time >= _lastTimeAttacked + _comboWindow)
                ComboCounter = 0;
        }

        /// <summary>
        /// Asigna el valor actual del combo al animator.
        /// </summary>
        private void SetAttackAnimation()
        {
            Player.Anim.SetInteger("ComboCounter", ComboCounter);
        }

        /// <summary>
        /// Aplica la fuerza de impulso del ataque correspondiente al combo.
        /// </summary>
        private void ApplyAttackMovement()
        {
            float attackDir = Player.FacingDir;

            if (XInput != 0)
                attackDir = XInput;

            var movement = Player.AttackMovement[ComboCounter];
            Player.SetVelocity(movement.x * attackDir, movement.y);
        }

        /// <summary>
        /// Reproduce el sonido del ataque.
        /// </summary>
        private void PlayAttackSound()
        {
            SoundManager.Instance.PlaySound(SoundType.Attack);
        }
    }
}

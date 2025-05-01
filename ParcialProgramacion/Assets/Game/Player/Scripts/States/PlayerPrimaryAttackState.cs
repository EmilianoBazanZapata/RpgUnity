using Game.Managers;
using Game.Player.Scripts.StateMachine;
using Game.Shared.Enums;
using UnityEngine;

namespace Game.Player.Scripts.States
{
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

            XInput = 0; // we need this to fix bug on attack direction

            if (ComboCounter > 2 || Time.time >= _lastTimeAttacked + _comboWindow)
                ComboCounter = 0;

            Player.Anim.SetInteger("ComboCounter", ComboCounter);

            #region Choose attack direction

            float attackDir = Player.FacingDir;

            if (XInput != 0)
                attackDir = XInput;

            #endregion
            
            Player.SetVelocity(Player.AttackMovement[ComboCounter].x * attackDir,
                Player.AttackMovement[ComboCounter].y);

            StateTimer = .1f;
            SoundManager.Instance.PlaySound(SoundType.Attack);
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

            Player.StartCoroutine("BusyFor", .15f);

            ComboCounter++;
            _lastTimeAttacked = Time.time;
        }
    }
}
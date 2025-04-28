using Game.Player.Scripts.Controllers;
using Game.Player.Scripts.StateMachine;
using UnityEngine;

namespace Game.Player.Scripts.States
{
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
            ControlPlayerStateChanges();
        }

        private void ControlPlayerStateChanges()
        {
            if (!Player.IsGroundDetected())
                StateMachine.ChangeState(Player.AirState);

            if (Input.GetKeyDown(KeyCode.Space) && Player.IsGroundDetected())
                StateMachine.ChangeState(Player.JumpState);

            if (Input.GetKey(KeyCode.Mouse0))
                StateMachine.ChangeState(Player.PrimaryAttackState);
            
            if(Input.GetKey(KeyCode.Mouse1) && HasNoSword())
                StateMachine.ChangeState(Player.AimSwordState);
            
            if(Input.GetKeyDown(KeyCode.Q))
                StateMachine.ChangeState(Player.CounterAttackState);
        }
        
        private bool HasNoSword()
        {
            if (!Player.Sword)
                return true;
        
            Player.Sword.GetComponent<SwordSkillController>().ReturnSword();
            return false;
        }
    }
}
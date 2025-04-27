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
            
            if(Input.GetKey(KeyCode.Mouse0))
                StateMachine.ChangeState(Player.PrimaryAttackState);
        }
    }
}
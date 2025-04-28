using Game.Player.Scripts.StateMachine;
using UnityEngine;

namespace Game.Player.Scripts.States
{
    public class PlayerAirState : PlayerState
    {
        public PlayerAirState(Player player, 
                              PlayerStateMachine stateMachine, 
                              string animBoolName) : base(player, stateMachine, animBoolName)
        {
        }
        public override void Update()
        {
            base.Update();
            
                        
            if (Input.GetKeyDown(KeyCode.Space) && !Player.IsGroundDetected() && Player.JumpCount < Player.MaxJumpCount)
            {
                StateMachine.ChangeState(Player.JumpState);
                Player.JumpCount++;
            }

            // if (Player.IsWallDetected())
            //     StateMachine.ChangeState(Player.WallSlideState);
        
            if (Player.IsGroundDetected())
                StateMachine.ChangeState(Player.IdleState);

            if (XInput != 0)
                Player.SetVelocity(Player.MoveSpeed *.8f * XInput, Rigidbody2D.velocity.y);
        }
    }
}
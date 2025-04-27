using Game.Player.Scripts.StateMachine;
using UnityEngine;

namespace Game.Player.Scripts.States
{
    public class PlayerWallSlideState : PlayerState
    {
        public PlayerWallSlideState(Player player, 
                                    PlayerStateMachine stateMachine, 
                                    string animBoolName) : base(player, stateMachine, animBoolName)
        {
        }

        public override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                StateMachine.ChangeState(Player.WallJumpState);
                return;
            }
        
            if (XInput != 0 && Player.FacingDir != XInput)
                StateMachine.ChangeState(Player.IdleState);

            Rigidbody2D.velocity = YInput < 0 ? new Vector2(0, Rigidbody2D.velocity.y) : new Vector2(0, Rigidbody2D.velocity.y * .7f);

            if (Player.IsGroundDetected())
                StateMachine.ChangeState(Player.IdleState);
        }
    }
}
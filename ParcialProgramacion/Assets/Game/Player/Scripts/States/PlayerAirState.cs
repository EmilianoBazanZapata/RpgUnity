using Game.Player.Scripts.StateMachine;

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

            if (Player.IsWallDetected())
                StateMachine.ChangeState(Player.WallSlideState);
        
            if (Player.IsGroundDetected())
                StateMachine.ChangeState(Player.IdleState);

            if (XInput != 0)
                Player.SetVelocity(Player.MoveSpeed *.8f * XInput, Rigidbody2D.velocity.y);
        }
    }
}
using Game.Player.Scripts.StateMachine;

namespace Game.Player.Scripts.States
{
    public class PlayerWallJumpState : PlayerState
    {
        public PlayerWallJumpState(Player player, 
                                   PlayerStateMachine stateMachine, 
                                   string animBoolName) : base(player, stateMachine, animBoolName)
        {
        }
        
        public override void Enter()
        {
            base.Enter();

            StateTimer = 1f;
            Player.SetVelocity(5 * -Player.FacingDir, Player.JumpForce);
        }

        public override void Update()
        {
            base.Update();

            if (StateTimer < 0)
                StateMachine.ChangeState(Player.AirState);

            if (Player.IsGroundDetected())
                StateMachine.ChangeState(Player.IdleState);
        }

    }
}
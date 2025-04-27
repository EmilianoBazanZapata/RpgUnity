using Game.Player.Scripts.StateMachine;

namespace Game.Player.Scripts.States
{
    public class PlayerMoveState : PlayerGroundedState
    {
        public PlayerMoveState(Player player,
                               PlayerStateMachine stateMachine,
                               string animBoolName) : base(player, stateMachine, animBoolName)
        {
        }
        
        public override void Update()
        {
            base.Update();
            
            Player.SetVelocity(XInput * Player.MoveSpeed, Rigidbody2D.velocity.y);

            if (XInput == 0 || Player.IsWallDetected())
                StateMachine.ChangeState(Player.IdleState);
        }
    }
}
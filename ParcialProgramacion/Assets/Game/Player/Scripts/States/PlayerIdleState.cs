using Game.Player.Scripts.StateMachine;

namespace Game.Player.Scripts.States
{
    public class PlayerIdleState : PlayerGroundedState
    {
        public PlayerIdleState(Player player, 
                               PlayerStateMachine stateMachine, 
                               string animBoolName) : base(player, stateMachine, animBoolName)
        {
        }
        public override void Enter()
        {
            base.Enter();

            Player.SetZeroVelocity();
        }

        public override void Update()
        {
            base.Update();
        
            if(XInput == Player.FacingDir && Player.IsWallDetected())
                return;
        
            if(XInput != 0 && !Player.IsBusy)
                StateMachine.ChangeState(Player.MoveState);
        }
    }
}
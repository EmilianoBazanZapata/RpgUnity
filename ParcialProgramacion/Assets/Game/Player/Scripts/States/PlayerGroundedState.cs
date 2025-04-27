using Game.Player.Scripts.StateMachine;

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
        }
    }
}
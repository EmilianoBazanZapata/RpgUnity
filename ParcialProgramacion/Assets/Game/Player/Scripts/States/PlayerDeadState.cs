using Game.Managers;
using Game.Player.Scripts.StateMachine;

namespace Game.Player.Scripts.States
{
    public class PlayerDeadState : PlayerState
    {
        public PlayerDeadState(Player player,
            PlayerStateMachine stateMachine,
            string animBoolName) : base(player, stateMachine, animBoolName)
        {
        }

        public override void Update()
        {
            base.Update();

            Player.SetZeroVelocity();
            
            GameManager.Instance.LoseGame();
        }
    }
}
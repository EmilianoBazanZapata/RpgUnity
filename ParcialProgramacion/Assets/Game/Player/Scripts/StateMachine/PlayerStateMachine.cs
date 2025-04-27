using Game.Shared.StateMachines.Interfaces;

namespace Game.Player.Scripts.StateMachine
{
    public class PlayerStateMachine : IStateMachine<PlayerState>
    {
        public PlayerState CurrentState { get; private set; }
        
        public void Initialize(PlayerState startState)
        {
            CurrentState = startState;
            CurrentState.Enter();
        }

        public void ChangeState(PlayerState newState)
        {
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}
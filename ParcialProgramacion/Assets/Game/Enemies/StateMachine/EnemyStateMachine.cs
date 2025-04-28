using Game.Shared.StateMachines.Interfaces;

namespace Game.Enemies.StateMachine
{
    public class EnemyStateMachine : IStateMachine<EnemyState>
    {
        public EnemyState CurrentState { get; private set; }
        
        public void Initialize(EnemyState startState)
        {
            CurrentState = startState;
            CurrentState.Enter();
        }

        public void ChangeState(EnemyState newState)
        {
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}
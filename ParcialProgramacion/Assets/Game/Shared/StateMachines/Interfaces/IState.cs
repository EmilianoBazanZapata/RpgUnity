namespace Game.Shared.StateMachines.Interfaces
{
    public interface IState
    {
        void Enter();
        void Update();
        void Exit();
    }
}
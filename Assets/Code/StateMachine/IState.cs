
namespace Assets.Code.StateMachine
{
    public interface IState
    {
        void Enter();
        void Execute();
        void Exit();

        string Name { get; }
    }
}


namespace Assets.Code.StateMachine
{
    public interface IState
    {
        void Enter();
        void Execute();
        void Exit();
    }
}

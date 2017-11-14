
namespace Assets.Code.StateMachine
{
    public class BattleStateMachine
    {
        private IState _previousState;
        private IState _currentState;
        public string ActiveState { get { return _currentState.Name; }}

        public void ChangeState(IState newState)
        {
            if (_currentState != null)
            {
                _currentState.Exit();
            }
            _previousState = _currentState;

            _currentState = newState;

            _currentState.Enter();
        }

        public void ExecuteUpdate()
        {
            _currentState.Execute();
        }

        public void LoadPreviousState()
        {
            _currentState.Exit();
            _currentState = _previousState;
            _currentState.Enter();
        }

    }
}

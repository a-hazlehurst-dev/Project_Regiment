
using System.Collections.Generic;
using System.Linq;

namespace Assets.Code.StateMachine
{
    public class BattleStateMachine
    {
        private IState _previousState;
        private Dictionary<string,IState> _activeStates;
        public BattleStateMachine()
        {
            _activeStates = new Dictionary<string, IState>();
        }

     
        public void AddState(string type, IState newState)
        {
            if (_activeStates != null && _activeStates.ContainsKey(type))
            {
                _activeStates[type].Exit();
                _activeStates.Remove(type);
            }
            _activeStates.Add(type, newState);

            _activeStates[type].Enter();
        }


        public void ExecuteUpdate()
        {
            foreach (var state in _activeStates.Keys.ToArray())
            {
                _activeStates[state].Execute();
            }

        }

      

    }
}

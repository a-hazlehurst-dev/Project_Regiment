using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.StateMachine;
using NUnit.Framework.Internal.Builders;

namespace Assets.CodeV2.Item
{
    public class ItemStateMachine
    {
        public Dictionary<string, IState> _currentStates;

        public ItemStateMachine()
        {
            _currentStates = new Dictionary<string, IState>();
        }

        public void ChangeState(IState state)
        {
            if (_currentStates.ContainsKey(state.StateType))
            {
                _currentStates[state.StateType].Exit();
                _currentStates.Remove(state.StateType);

                _currentStates.Add(state.StateType, state);
                state.Enter();
            }
        }


        public void Execute()
        {
            foreach (var state in _currentStates)
            {
                state.Value.Execute();
            }
        }
    }
}

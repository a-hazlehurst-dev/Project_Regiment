
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

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

     
        public void AddState(IState newState)
        {
            if (_activeStates != null && _activeStates.ContainsKey(newState.StateType))
            {
                _activeStates[newState.StateType].Exit();
                _activeStates.Remove(newState.StateType);
            }
            _activeStates.Add(newState.StateType, newState);

            _activeStates[newState.StateType].Enter();
        }


        public void ExecuteUpdate()
        {
            

            foreach (var state in _activeStates.Keys.ToArray())
            {
                _activeStates[state].Execute();
            }

            
            
        }

        public string GetActiveStates()
        {
            var states = "";
            foreach (var state in _activeStates.Keys)
            {
                states += state + "{ " + _activeStates[state].Name + "}";
            }
            
            return states;
        }

      

    }
}

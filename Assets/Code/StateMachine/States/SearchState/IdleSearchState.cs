using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.StateMachine.States.SearchState
{
    public class IdleSearchState : IState
    {
        private readonly GameObject _self;
        public string Who { get { return _self.gameObject.name; } }

        public IdleSearchState(GameObject self)
        {
            _self = self;
        }
        public void Enter()
        {
            
        }

        public void Execute()
        {
        }

        public void Exit()
        {
        }

        public string StateType { get { return "search"; } }
        public string Name { get { return "Idle"; }}
    }
}

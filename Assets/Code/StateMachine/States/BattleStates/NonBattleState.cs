using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.StateMachine.States.BattleStates
{
    public class NonBattleState : IState
    {
        private readonly GameObject _self;
        public string Who { get { return _self.gameObject.name; } }
        public string StateType { get { return "battle"; } }
        public string Name { get { return "Non Battle"; } }

        public NonBattleState(GameObject self)
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

    }
}

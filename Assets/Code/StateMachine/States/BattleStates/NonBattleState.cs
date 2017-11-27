using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Code.StateMachine.States.BattleStates
{
    public class NonBattleState : IState
    {
        public void Enter()
        {
            
        }

        public void Execute()
        {
        }

        public void Exit()
        {
        }

        public string StateType { get { return "battle"; }}
        public string Name { get { return "Non Battle"; }}
    }
}

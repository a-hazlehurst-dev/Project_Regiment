using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Code.StateMachine.States
{
    public class NoState :IState
    {
        public string StateType {  get { return "none"; } }
        public void Enter()
        {
            
        }

        public void Execute()
        {
            
        }

        public void Exit()
        {
            
        }

        public string Name { get { return "NoState"; } }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.StateMachine;

namespace Assets.CodeV2.Item
{
    public class ItemOffState : IState
    {
        public string StateType {get { return "active"; }}
        public string Name { get { return "Off State"; } }
        public string Who { get { return ""; } }

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

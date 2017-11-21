using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Code.StateMachine.States
{
    public class NonState : IState
    {
        public string Name
        {
            get
            {
                return "Non State";
            }
        }

        public void Enter()
        {
         
        }

        public void Execute()
        {
           //Do Nothing
        }

        public void Exit()
        {
         
        }
    }
}

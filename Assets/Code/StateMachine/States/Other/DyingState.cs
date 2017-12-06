using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.StateMachine.States.Other
{
    public class DyingState : IState
    {
        private readonly GameObject _self;
        private bool AmILyingDown = false;
        private Action _cbOnDead;

        public string Name { get { return "Dead"; } }
        public string StateType { get { return "other"; } }
        public string Who { get { return _self.gameObject.name; } }
        public DyingState(GameObject self, Action cbOnDead)
        {
            _self = self;
            _cbOnDead += cbOnDead;
          
        }

        public void Enter()
        {
            
        }

        public void Execute()
        {
            
            var renderer = _self.GetComponentInChildren<SpriteRenderer>();
            renderer.color = Color.red;
            if (!AmILyingDown)
            {
                renderer.gameObject.transform.Rotate(0, 0, 90);
                AmILyingDown = true;
                Debug.Log("i am dead");
            }
            _cbOnDead();


        }

        public void Exit()
        {
        }

    }
}

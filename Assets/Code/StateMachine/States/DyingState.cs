using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.StateMachine.States
{
    public class DyingState : IState
    {
        private readonly GameObject _self;
        private bool AmILyingDown = false;

        public string Name { get { return "Dying"; } }
        public DyingState(GameObject self)
        {
            _self = self;
          
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
            }
            
        }

        public void Exit()
        {
        }

    }
}

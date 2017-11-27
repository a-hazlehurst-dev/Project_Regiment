using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.World;
using UnityEngine;

namespace Assets.Code.StateMachine.States.MovementState
{
    public class StayInRangeState : IState
    {
        private readonly GameObject _self;
        private readonly GameObject _target;
        private readonly BaseCharacter _character;
        private Action<GameObject> _cbOutOfRange;

        public StayInRangeState(GameObject self, GameObject target, BaseCharacter character, Action<GameObject> cbOutOfRange)
        {
            _self = self;
            _target = target;
            _character = character;
            _cbOutOfRange += cbOutOfRange;
        }
        public void Enter()
        {
            
        }

        public void Execute()
        {
            if (Vector3.Distance(_self.transform.position, _target.transform.position) >= _character.Reach)
            {
                _cbOutOfRange(_target);
            }
        }

        public void Exit()
        {
        }

        public string StateType { get { return "move"; }}
        public string Name { get { return "Stay In Range"; } }
    }
}

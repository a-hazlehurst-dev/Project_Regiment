using Assets.Code.Services.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.World;
using UnityEngine;

namespace Assets.Code.StateMachine
{
    public class MoveToState : IState
    {
        private readonly GameObject _self;
        private  GameObject _target;
        private readonly BaseCharacter _character;

        private FacingHelper _facingHelper;
        private Action OnTargetReached;
        public string Name { get { return "Move To"; } }
        public string StateType { get { return "move"; } }
        public string Who { get { return _self.gameObject.name; } }

        public MoveToState(GameObject self, GameObject target, BaseCharacter character, Action cbTargetReached )
        {
            this._self = self;
            this._target = target;
            _character = character;
            OnTargetReached += cbTargetReached;
          
        }


        public void Enter()
        {
            
        }

        public void Execute()
        {
            float step = _character.Speed * Time.deltaTime;

            _self.transform.position =  Vector3.MoveTowards(_self.transform.position, _target.transform.position, step);

            if (Vector3.Distance(_self.transform.position, _target.transform.position) <= _character.Reach)
            {
                OnTargetReached();
            }
        }

        public void Exit()
        {
            
        }


    }
}

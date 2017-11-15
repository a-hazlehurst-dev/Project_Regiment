using System;
using UnityEngine;

namespace Assets.Code.StateMachine.States
{
    public class FleeState : IState
    {
        private readonly GameObject _self;
        private readonly GameObject _target;
        private readonly float _speed;
        private readonly Action _cbOnExitReached;


        public FleeState(GameObject self, GameObject target, float speed, Action cbOnExitReached)
        {
            _self = self;
            _target = target;
            _speed = speed;
            _cbOnExitReached += cbOnExitReached;
        }
        public void Enter()
        {
            Debug.Log(_self.name +", is running away.");
        }

        public void Execute()   
        {
            if (_target != null)
            {
                float step = _speed * Time.deltaTime;
                _self.transform.position = Vector3.MoveTowards(_self.transform.position, _target.transform.position, step);
                var dist = Vector3.Distance(_self.transform.position, _target.transform.position);

                Debug.Log(_self + " distance to safety " + dist);

                if (dist <= 2)

                {
                    
                    GameObject.Destroy(_self);
                }
            }

        }

        public void Exit()
        {
        }

        public string Name
        {
            get { return "Flee"; }
        }
    }
}


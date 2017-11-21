using Assets.Code.Services.Helper;
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
        private string name;


        public FleeState(GameObject self, GameObject target, float speed, Action cbOnExitReached)
        {
            _self = self;
            _target = target;
            _speed = speed;
            _cbOnExitReached += cbOnExitReached;
            name = _self.gameObject.name;
        }
        public void Enter()
        {
            Debug.Log(_self.name +", is running away.");
            _self.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
        }

        public void Execute()   
        {
            Debug.Log(name + " is escaping to " );
            if (_target != null)
            {
                float step = _speed * Time.deltaTime;

                _self.transform.position = Vector3.MoveTowards(_self.transform.position, _target.transform.position, step);
                var dist = Vector3.Distance(_self.transform.position, _target.transform.position);
              


                if (dist <= .2f)
                {
                    _self.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.grey;
                    _cbOnExitReached();
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


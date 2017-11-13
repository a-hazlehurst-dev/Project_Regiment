﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.StateMachine
{
    public class EngageTarget : IState
    {
        private readonly GameObject _self;
        private  GameObject _target;
        private float _speed;

        private Action OnTargetReached;
        private readonly float _reach;

        public EngageTarget(GameObject self, GameObject target, float speed, Action cbTargetReached, float reach)
        {
       
            this._self = self;
            this._target = target;
            _speed = speed;
            OnTargetReached += cbTargetReached;
            this._reach = reach;
        }


        public void Enter()
        {
           
        }

        public void Execute()
        {
            float step = _speed * Time.deltaTime;
            _self.transform.position = Vector3.MoveTowards(_self.transform.position, _target.transform.position, step);

            if(Vector3.Distance(_self.transform.position, _target.transform.position) <= _reach)
            {
                OnTargetReached();
            }
        }

        public void Exit()
        {
            
        }


    }
}

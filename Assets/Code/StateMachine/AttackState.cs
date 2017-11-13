using System;
using UnityEngine;

namespace Assets.Code.StateMachine
{
    public class AttackState : IState
    {
        private  GameObject _target;

        public AttackState(GameObject target)
        {
            this._target = target;
        }

        public void Enter()
        {
        }

        public void Execute()
        {
            GameObject.Destroy(_target);
            _target = null;
        }

        public void Exit()
        {
        }
    }
}

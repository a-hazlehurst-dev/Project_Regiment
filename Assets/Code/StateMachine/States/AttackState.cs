using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Code.StateMachine
{
    public class AttackState : IState
    {
        private readonly GameObject _self;
        private  GameObject _target;
        private readonly float _attackSpeed;
        private float _cooldownTaken;
        private readonly Action _cbOnTargetDissapeared;
        private readonly Action<int> _cbOnHit;

        public AttackState(GameObject self, GameObject target,float attackSpeed, Action cbOnTargetDissapeared)
        {
            _self = self;
            this._target = target;
            _attackSpeed = attackSpeed;
            _cooldownTaken = attackSpeed;

            _cbOnTargetDissapeared += cbOnTargetDissapeared;
         
        }

        public void Enter()
        {
        }

        public void Execute()
        {
            _cooldownTaken -= Time.deltaTime;
            if (_cooldownTaken > 0)
            {
                return;
            }

            Brain brain = _target.GetComponentInChildren<Brain>();
            if (brain != null)
            {
                brain.OnHit(1);
                Debug.Log(_self.gameObject.name + "has hit " + _target.gameObject.name);
                _cbOnTargetDissapeared();
            }

            _cooldownTaken = _attackSpeed;

        }

        public void Exit()
        {
        }

        public string Name { get { return "Attack"; } }
    }
}

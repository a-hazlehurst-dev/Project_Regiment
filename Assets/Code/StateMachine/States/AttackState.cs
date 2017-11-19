using Assets.Code.Services.Helper;
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
        private readonly float _range;
        private float _cooldownTaken;
        private readonly Action _cbOnTargetDissapeared;
        private readonly Animator knifeAttack;
        private readonly Action<int> _cbOnHit;
        
        private FacingHelper _facingHelper;

        public AttackState(GameObject self, GameObject target,float attackSpeed, float range,Action cbOnTargetDissapeared, Animator knifeAttack, FacingHelper facingHelper)
        {
            _self = self;
            this._target = target;
            _attackSpeed = attackSpeed;
            _range = range;
            _cooldownTaken = attackSpeed;
            _facingHelper = facingHelper;
        
            _cbOnTargetDissapeared += cbOnTargetDissapeared;
            this.knifeAttack = knifeAttack;
        }

        public void Enter()
        {
          
            knifeAttack.SetBool("OnAttack", true);
        }

        public void Execute()
        {
            
            _cooldownTaken -= Time.deltaTime;

            if (_cooldownTaken > 0)
            {
                return;
            }
           
            if (Vector3.Distance(_self.transform.position, _target.transform.position) >= _range)
            {
                _cbOnTargetDissapeared();
                return;
            }

            Brain brain = _target.GetComponentInChildren<Brain>();

            if (brain != null)
            {
                brain.OnHit(1);
                Debug.Log(_self.gameObject.name + "has hit " + _target.gameObject.name);

                if (brain.Character.IsDead())
                {
                    _cbOnTargetDissapeared();

                }
                
           
            }

            _cooldownTaken = _attackSpeed;

        }

        public void Exit()
        {
            knifeAttack.SetBool("OnAttack", false);
        }

        public string Name { get { return "Attack"; } }
    }
}

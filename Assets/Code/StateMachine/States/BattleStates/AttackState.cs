using Assets.Code.Services.Helper;
using Assets.Code.World;
using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Code.StateMachine
{
    public class AttackState : IState
    {
        private readonly GameObject _self;
        private  GameObject _target;
        private readonly BaseCharacter _character;
        private readonly Brain _targetBrain;
        private Brain _myBrain;
        private float _cooldownTaken;
        private readonly Action _cbOnTargetDissapeared;
        private readonly Action _onRest;
        private readonly Animator knifeAttack;
        
        
        public string StateType { get { return "battle"; } }
        public string Name { get { return "Attack"; } }
        public string Who { get { return _self.gameObject.name; } }

        public AttackState(GameObject self, GameObject target, BaseCharacter character,Action cbOnTargetDissapeared,Action OnRest, Animator knifeAttack)
        {
            _self = self;
            this._target = target;
            _targetBrain = _target.GetComponentInChildren<Brain>();
            _myBrain = _self.GetComponentInChildren<Brain>();
            _character = character;
            _cooldownTaken = character.AttackSpeed;
        
            _cbOnTargetDissapeared += cbOnTargetDissapeared;
            _onRest += OnRest;
            this.knifeAttack = knifeAttack;
        }

        public void Enter()
        {
            

        }

        public void Execute()
        {
            _cooldownTaken -= Time.deltaTime;
           
            if (_character.Stamina <= 10)
            {
                Debug.Log("Stamina (Attacking): " + _character.Stamina);
                knifeAttack.SetBool("OnAttack", false);
                _onRest();
            }

            if (_cooldownTaken > 0)
            {
                return;
            }
           
            if (Vector3.Distance(_self.transform.position, _target.transform.position) >= _character.Reach)
            {
                _cbOnTargetDissapeared();
                return;
            }

            if (knifeAttack.GetBool("OnAttack") == true)
            {
                return;
            }
            if (_character.Stamina - 5 <0)
            {
                return;
            }

            if (_targetBrain != null)
            {
                if (_targetBrain.Character.IsActive())
                {
                    knifeAttack.SetBool("OnAttack", true);
                }
                
            }
            else
            {
                knifeAttack.SetBool("OnAttack", false);
                _cbOnTargetDissapeared();
            }
            _cooldownTaken = _character.AttackSpeed;
        }

        public void Exit()
        {
            
        }


    }
}

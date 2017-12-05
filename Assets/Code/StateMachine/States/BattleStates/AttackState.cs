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
        private Brain _targetBrain;
        private Brain _myBrain;
        private float _cooldownTaken;
        private readonly Action _cbOnTargetDissapeared;
        private readonly Animator knifeAttack;
        private readonly Action<int> _cbOnHit;
        
        public string StateType { get { return "battle"; } }
        public string Name { get { return "Attack"; } }
        public string Who { get { return _self.gameObject.name; } }
        public AttackState(GameObject self, GameObject target, BaseCharacter character,Action cbOnTargetDissapeared, Animator knifeAttack)
        {
            _self = self;
            this._target = target;
            _targetBrain = _target.GetComponentInChildren<Brain>();
            _myBrain = _self.GetComponentInChildren<Brain>();
            _character = character;
            _cooldownTaken = character.AttackSpeed;
        
            _cbOnTargetDissapeared += cbOnTargetDissapeared;
            this.knifeAttack = knifeAttack;
        }

        public void Enter()
        {
            

        }

        public void Execute()
        {
            _cooldownTaken -= Time.deltaTime;
            _targetBrain.OnBeingAttacked();

            if (_cooldownTaken > 0)
            {
                return;
            }
           
            if (Vector3.Distance(_self.transform.position, _target.transform.position) >= _character.Reach)
            {
                _cbOnTargetDissapeared();
                return;
            }

            if (_targetBrain != null)
            {
                knifeAttack.SetBool("OnAttack", true);
                _myBrain.Character.SetStamina(-5);
                _targetBrain.OnHit(_character.AttackPower);

                if (_targetBrain.IsDead|| _targetBrain.HasEscaped)
                {
                    _cbOnTargetDissapeared();
                }
            }

            _targetBrain.OnFinishedBeingAttacked();
            _myBrain.OnFinishedMyAttack();
            _cooldownTaken = _character.AttackSpeed;
            
        }

        public void Exit()
        {
            knifeAttack.SetBool("OnAttack", false);
        }


    }
}

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
        private float _cooldownTaken;
        private readonly Action _cbOnTargetDissapeared;
        private readonly Animator knifeAttack;
        private readonly Action<int> _cbOnHit;
        
        public string StateType { get { return "battle"; } }
        public string Who { get { return _self.gameObject.name; } }
        public AttackState(GameObject self, GameObject target, BaseCharacter character,Action cbOnTargetDissapeared, Animator knifeAttack)
        {
            _self = self;
            this._target = target;
            _character = character;
            _cooldownTaken = character.AttackSpeed;
        
            _cbOnTargetDissapeared += cbOnTargetDissapeared;
            this.knifeAttack = knifeAttack;
        }

        public void Enter()
        {
          
            knifeAttack.SetBool("OnAttack", true);
        }

        public void Execute()
        {
            Brain brain = _target.GetComponentInChildren<Brain>();
            _cooldownTaken -= Time.deltaTime;
            brain.OnBeingAttacked();

            if (_cooldownTaken > 0)
            {
                return;
            }
           
            if (Vector3.Distance(_self.transform.position, _target.transform.position) >= _character.Reach)
            {
                _cbOnTargetDissapeared();
                return;
            }

            

            if (brain != null)
            {
                brain.OnHit(_character.AttackPower);
                Debug.Log(_self.gameObject.name + "has hit " + _target.gameObject.name);

                if (brain.Character.IsDead())
                {
                    _cbOnTargetDissapeared();

                }
                
           
            }

            _cooldownTaken = _character.AttackSpeed;

        }

        public void Exit()
        {
            knifeAttack.SetBool("OnAttack", false);
        }

        public string Name { get { return "Attack"; } }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.StateMachine.States.BattleStates
{
    public class DefendState : IState
    {
        private readonly GameObject _self;
        private readonly Brain _myBrain;
        public string Who { get { return _self.gameObject.name; } }
        public Animator _knifeDefend;
        public string StateType { get { return "battle"; } }
        public string Name { get { return "Defend"; } }

        private bool _isAlreadyDefending;


        public DefendState(GameObject self, Animator knifeDefend)
        {
            _self = self;
            _knifeDefend = knifeDefend;
            _myBrain = _self.GetComponentInChildren<Brain>();

        }
        public void Enter()
        {
                _knifeDefend.SetBool("OnDefend", true);
        }

        public void Execute()
        {
            _self.GetComponentInChildren<Brain>().IsDefending = true;
            _myBrain.Character.SetStamina(-1);
        }

        public void Exit()
        {
            _self.GetComponentInChildren<Brain>().IsDefending = false;
            _knifeDefend.SetBool("OnDefend", false);
        }

     
    }
}

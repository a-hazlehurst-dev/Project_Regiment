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
        public string Who { get { return _self.gameObject.name; } }
        public Animator _knifeDefend;
        public string StateType { get { return "battle"; } }
        public string Name { get { return "Defend"; } }

        private bool _isAlreadyDefending;


        public DefendState(GameObject self, Animator knifeDefend, bool IsAlreadyDefending)
        {
            _self = self;
            _knifeDefend = knifeDefend;
            _isAlreadyDefending = IsAlreadyDefending;
        }
        public void Enter()
        {
            if (!_isAlreadyDefending)
            {
                _knifeDefend.SetBool("OnDefend", true);
            }
        }

        public void Execute()
        {
            _self.GetComponentInChildren<Brain>().IsDefending = true;
        }

        public void Exit()
        {
            _self.GetComponentInChildren<Brain>().IsDefending = false;
        }

     
    }
}

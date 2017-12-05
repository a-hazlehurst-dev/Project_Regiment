using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.StateMachine.States.BattleStates
{
    public class BattleRestState : IState
    {
        private readonly GameObject _self;
        private readonly Brain _myBrain;

        public BattleRestState(GameObject _self)
        {
            this._self = _self;
            _myBrain = _self.GetComponentInChildren<Brain>();
        }
        public void Enter()
        {
            
        }

        public void Execute()
        {
            if (_myBrain.Character.Strength >= _myBrain.Character.MaxStamina)
            {
                _myBrain.Character.Strength = _myBrain.Character.MaxStamina;
                return;
            }
            _myBrain.Character.SetStamina(1);
        }

        public void Exit()
        {
            
        }

        public string StateType { get{ return "Battle";} }
        public string Name { get { return "Resting."; } }
        public string Who { get; private set; }
    }
}

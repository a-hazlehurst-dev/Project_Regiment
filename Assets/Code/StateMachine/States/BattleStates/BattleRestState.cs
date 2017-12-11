﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.StateMachine.States.BattleStates
{
    public class BattleRestState : IState
    {
        private readonly GameObject _self;
        private readonly Action _onRestCompleted;
        private readonly Brain _myBrain;
        private float restingCooldown;
        private float count;
        public BattleRestState(GameObject self, Action onRestCompleted)
        {
            this._self = self;
            this._onRestCompleted += onRestCompleted;
            _myBrain = _self.GetComponentInChildren<Brain>();
           
        }
        public void Enter()
        {
            restingCooldown = 0.2f;
            count = 1;
        }

        public void Execute()
        {
           
            count -= Time.deltaTime;
            Debug.Log(_self.name + " cooldown: " + count + " Stamina: "+ _myBrain.Character.Stamina + "("+ _myBrain.Character.MaxStamina+")");

            if (_myBrain.Character.Stamina >= _myBrain.Character.MaxStamina)
            {
                Debug.Log("test");
                _myBrain.Character.Stamina = _myBrain.Character.MaxStamina;
                _onRestCompleted();
                return;
            }

            if(count <= 0)
            {
                Debug.Log("Test");
                _myBrain.Character.SetStamina(1);
                count = restingCooldown;
            }

           
        }

        public void Exit()
        {
            
        }

        public string StateType { get{ return "battle";} }
        public string Name { get { return "Resting"; } }
        public string Who { get; private set; }
    }
}

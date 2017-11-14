using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.StateMachine.States
{
    public class CelebrationState : IState
    {
        private readonly GameObject self;
        private float switchCooldown;
        private float cooldown;
   
        private Color switchColorTo;

        public string Name { get { return "Celebrating"; } }

        public CelebrationState(GameObject self)
        {
            this.self = self;
            switchCooldown = 1;
            cooldown = 1;
         
        }

        public void Enter()
        {
        }

        public void Execute()
        {
            cooldown = cooldown -  Time.deltaTime;
            if(cooldown<= 0)
            {
                if (switchColorTo == Color.blue)
                {
                    switchColorTo = Color.white;
                }
                else
                {
                    switchColorTo = Color.blue;
                }
                var renderer = self.GetComponentInChildren<SpriteRenderer>();
                renderer.color = switchColorTo;
                cooldown = switchCooldown;
            }
           
        }

        public void Exit()
        {
        }
    }
}

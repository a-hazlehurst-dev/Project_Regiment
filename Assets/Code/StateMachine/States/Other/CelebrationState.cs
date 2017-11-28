
using UnityEngine;

namespace Assets.Code.StateMachine.States.Other
{
    public class CelebrationState : IState
    {
        private readonly GameObject _self;
        private float switchCooldown;
        private float cooldown;
   
        private Color switchColorTo;
        public string Who { get { return _self.gameObject.name; } }
        public string Name { get { return "Celebrating"; } }
        public string StateType { get { return "other"; } }

    

        public CelebrationState(GameObject self)
        {
            _self = self;
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
                var renderer = _self.GetComponentInChildren<SpriteRenderer>();
                renderer.color = switchColorTo;
                cooldown = switchCooldown;
            }
           
        }

        public void Exit()
        {
        }
    }
}

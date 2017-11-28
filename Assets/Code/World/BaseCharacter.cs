
using System;
using System.Security.Policy;
using UnityEngine;

namespace Assets.Code.World
{
    public class BaseCharacter
    {

        public string Name { get; set; }
        public string Lastname { get; set; }
        public DateTime Dob { get; set; }
        public float Reach { get; set; }
        public float Speed { get { return Mathf.CeilToInt(1 + Agility/3); } }
        public float AttackSpeed { get { return Mathf.CeilToInt(1 + Agility/3); } }




        public float Strength { get; set; }
        public float Agility { get; set; }
        public float Endurance { get; set; }
        

        public int HitPoints { get; set; }
        public int MaxHitPoints { get; set; }
        public int Morale { get; set; }
        public int MaxMorale { get; set; }

        public float Stamina { get; set; }
        public float MaxStamina { get; set; }

        public int AttackPower {  get { return Mathf.CeilToInt(1 + Strength); } }

        public bool IsDead()
        {
            if(HitPoints<= 0)
            {
                return true;
            }
            return false;
        }
        public bool IsFleeing()
        {
            if(Morale<= 10)
            {
                return true;
            }
            return false;
        }
        public bool IsUnconsious()
        {
            if (HitPoints <= MaxHitPoints *.10)
            {
                return true;
            }
            return false;
        }
    }
}

﻿
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
        public float Speed { get { return Mathf.CeilToInt(1); } }
        public float AttackSpeed { get { return Mathf.CeilToInt(1); } }




        public float Strength { get; set; }
        public float Agility { get; set; }
        public float Endurance { get; set; }
        

        public int HitPoints { get; set; }
        public int MaxHitPoints { get; set; }
        public int Morale { get; set; }
        public int MaxMorale { get; set; }

        public int Stamina { get; set; }
        public int MaxStamina { get; set; }

        public int AttackPower {  get { return Mathf.CeilToInt(1 + Strength); } }

        public bool IsDead()
        {
            if(HitPoints<= 0)
            {
                return true;
            }
            return false;
        }

        public bool IsEscaped { get; set; }


        public bool IsActive()
        {
            if (IsDead() || IsEscaped)
            {
                return false;
            }
            return true;
        }

        public void SetStamina(int i)
        {
            if (Stamina +i <= 0)
            {
                Stamina = 0;
            }
            Stamina += i;
        }
    }
}

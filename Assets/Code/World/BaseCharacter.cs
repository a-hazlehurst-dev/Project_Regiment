using System;
using UnityEngine;

namespace Assets.Code.World
{
    public class BaseCharacter
    {
        public int Id { get; set; }
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
        public bool IsEscaped { get; set; }

        public int AttackPower {  get { return Mathf.CeilToInt(1 + Strength); } }

        public bool IsDead()
        {
            if(HitPoints<= 0)
            {
                return true;
            }
            return false;
        }

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
            Stamina += i;
        }

        public void TakeDamage(int attackPower)
        {
            HitPoints -= attackPower;

            if (HitPoints <= 0)
            {
                HitPoints = 0;
            }
        }
    }
}

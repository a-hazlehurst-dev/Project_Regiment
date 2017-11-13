
using System;

namespace Assets.Code.World
{
    public class BaseCharacter
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public DateTime Dob { get; set; }

        public int HitPoints { get; set; }
        public int MaxHitPoints { get; set; }
        public int Morale { get; set; }

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

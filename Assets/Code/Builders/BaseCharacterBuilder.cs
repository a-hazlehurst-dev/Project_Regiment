using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.World;

namespace Assets.Code.Builders
{
    public class BaseCharacterBuilder
    {

        public BaseCharacter Build()
        {

            var character = new BaseCharacter { Reach = UnityEngine.Random.value*2, Speed = UnityEngine.Random.value *3 };
            var rnd = UnityEngine.Random.value * 20;
            if (character.Speed < 1f)
            {
                character.Speed = 1;
            }
           
            if(rnd<1) { rnd += 1; }
            character.HitPoints = (int)rnd;
            character.MaxHitPoints = (int)rnd;
            character.AttackSpeed = UnityEngine.Random.value * 2;
            return character;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.World;
using UnityEngine;

namespace Assets.Code.Builders
{
    public class BaseCharacterBuilder
    {

        public BaseCharacter Build()
        {

            var character = new BaseCharacter { Reach = UnityEngine.Random.value*2, Speed = UnityEngine.Random.value *3 };
            character.Endurance = UnityEngine.Random.Range(2, 8);
            if(character.Reach <.07f) { character.Reach = 0.75f; }

            var rnd = 10;
            if (character.Speed < 1f)
            {
                character.Speed = 1;
            }

            character.MaxHitPoints = 10 + Mathf.CeilToInt(UnityEngine.Random.Range(4, 4+character.Endurance) * 3);
            character.HitPoints = character.MaxHitPoints;
            if(rnd<1) { rnd += 1; }
            character.AttackSpeed = UnityEngine.Random.value * 2;
            return character;
        }
    }
}

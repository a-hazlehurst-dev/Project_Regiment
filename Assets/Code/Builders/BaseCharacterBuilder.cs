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

            var character = new BaseCharacter { Reach = UnityEngine.Random.value*2  };
            character.Endurance = UnityEngine.Random.Range(2, 8);
            character.Strength = UnityEngine.Random.Range(2, 8);
            character.Agility = UnityEngine.Random.Range(2, 8);

            if (character.Reach <.5f) { character.Reach = 0.5f; }

            var rnd = 10;
            character.MaxStamina = 10 + Mathf.CeilToInt(UnityEngine.Random.Range(4, 4 + character.Endurance) * 3);
            character.MaxHitPoints = 10 + Mathf.CeilToInt(UnityEngine.Random.Range(4, 4+character.Endurance) * 3);
            
            character.Stamina = character.MaxStamina;
            Debug.Log("Stamina = " + character.Stamina);
            character.HitPoints = character.MaxHitPoints;
            if(rnd<1) { rnd += 1; }
            
            return character;
        }
    }
}

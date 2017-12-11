using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class BattleController : MonoBehaviour {


    public List<GameObject> Characters;
	// Use this for initialization
	void Start () {
        Characters = new List<GameObject>();
        var tags =  GameObject.FindGameObjectsWithTag("Character");
        Characters.AddRange(tags);

    }
   
	
	// Update is called once per frame
	void Update () {
        var remove = new List<GameObject>();
        foreach (var item in Characters)
        {
            var brain = item.GetComponentInChildren<Brain>();
            if (brain.Character.IsDead() )
            {
                remove.Add(item);
            }
        }

       foreach(var item in remove)
       {
            Characters.Remove(item);
       }

       if(Characters.Count == 1)
        {
            //Characters.First().GetComponentInChildren<Brain>().OnVictory();
        }
    }
}

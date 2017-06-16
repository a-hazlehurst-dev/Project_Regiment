using Assets.Code.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PeasantManager : MonoBehaviour {


    private List<Peasant> _peasants;
	// Use this for initialization
	void Awake()
    {
        _peasants = new List<Peasant>();
    }
	
    public void CreatePeasant()
    {
		if(_peasants ==null) { _peasants = new List<Peasant> ();}
        _peasants.Add(new Peasant { Id = 1, Name = "adam", Lastname = "Hazlehurst" });
    }

    public Peasant GetPeasant(int id)
    {
        return _peasants.SingleOrDefault(x => x.Id == id);
    }
}

﻿using System.Collections.Generic;
using UnityEngine;

public class FurniturePrototypes
{

    public Dictionary<string, Furniture> _furniturePrototypes;

    public FurniturePrototypes()
    {
        _furniturePrototypes = new Dictionary<string, Furniture>();
        InitPrototypes();
    }

    public void InitPrototypes()
    {
		//will be loaded from xml file or json file in the feature.
        _furniturePrototypes.Add("wall", new Furniture("wall", 0, 1, 1, true));
		_furniturePrototypes.Add("door", new Furniture("door", 2, 1, 1, true));

		_furniturePrototypes ["door"].furnParameters ["openess"] = 0;
		_furniturePrototypes ["door"].furnParameters ["is_opening"] = 0;
		_furniturePrototypes ["door"].updateActions += FurnitureActions.Door_UpdateAction;
		_furniturePrototypes ["door"].isEnterable = FurnitureActions.Door_IsEnterable;

    }

    public Furniture Get(string furnitureType)
    {
        if (!_furniturePrototypes.ContainsKey(furnitureType))
        {
            Debug.Log("FurniturePrototypes Warning: There are no furniture prototypes of type " + furnitureType);
            return null;
        }

        return _furniturePrototypes[furnitureType];

    }

}
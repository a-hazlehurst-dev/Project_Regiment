using System.Collections.Generic;
using UnityEngine;

public class FurniturePrototypes
{

    public Dictionary<string, Furniture> _furniturePrototypes;
    public Dictionary<string, Job> furnitureRequirements;

    public FurniturePrototypes()
    {
        _furniturePrototypes = new Dictionary<string, Furniture>();
        furnitureRequirements = new Dictionary<string, Job>();
        InitPrototypes();
    }

    public void InitPrototypes()
    {
		//will be loaded from xml file or json file in the feature.
        _furniturePrototypes.Add("wall", new Furniture("wall", 0, 1, 1, true, true));
        furnitureRequirements.Add("wall", new Job(null, "wall",FurnitureActions.JobComplete_FurnitureBuilding,1f,new Inventory[] { new Inventory( "clay", 5, 0)}));

		_furniturePrototypes.Add("door", new Furniture("door",1, 1, 1, false, true));


        _furniturePrototypes ["door"].SetParameter ("openness", 0);
		_furniturePrototypes ["door"].SetParameter("is_opening",0);
		_furniturePrototypes ["door"].RegisterUpdateAction (FurnitureActions.Door_UpdateAction);
		_furniturePrototypes ["door"].isEnterable = FurnitureActions.Door_IsEnterable;

        _furniturePrototypes.Add("stockpile", new Furniture("stockpile", 1, 1, 1, false, false));
        _furniturePrototypes["stockpile"].RegisterUpdateAction(FurnitureActions.Stockpile_Update_Action);

        furnitureRequirements.Add("stockpile", new Job(null, "stockpile", FurnitureActions.JobComplete_FurnitureBuilding, -1f,null));
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
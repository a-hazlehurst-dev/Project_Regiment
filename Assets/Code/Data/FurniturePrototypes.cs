using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;

public class FurniturePrototypes
{

    public Dictionary<string, Furniture> _furniturePrototypes;
    public Dictionary<string, Job> furnitureRequirements;

    public FurniturePrototypes()
    {
        _furniturePrototypes = new Dictionary<string, Furniture>();
        furnitureRequirements = new Dictionary<string, Job>();
        
    }

//    public void InitPrototypes()
//    {
//		//will be loaded from xml file or json file in the feature.
//        _furniturePrototypes.Add("FURN_WALL", new Furniture("FURN_WALL", 0, 1, 1, true, true));
//        furnitureRequirements.Add("FURN_WALL", new Job(null, "FURN_WALL", FurnitureActions.JobComplete_FurnitureBuilding,1f,new Inventory[] { new Inventory( "clay", 5, 0)}));
//        _furniturePrototypes["FURN_WALL"].Name = "Basic Wall";
//
//        _furniturePrototypes.Add("door", new Furniture("door",1, 1, 1, false, true));
//
//
//        _furniturePrototypes ["door"].SetParameter ("openness", 0);
//		_furniturePrototypes ["door"].SetParameter("is_opening",0);
//		_furniturePrototypes ["door"].RegisterUpdateAction (FurnitureActions.Door_UpdateAction);
//		_furniturePrototypes ["door"].isEnterable = FurnitureActions.Door_IsEnterable;
//
//        _furniturePrototypes.Add("stockpile", new Furniture("stockpile", 1, 1, 1, true, false));
//        _furniturePrototypes["stockpile"].RegisterUpdateAction(FurnitureActions.Stockpile_Update_Action);
//        _furniturePrototypes["stockpile"].Tint = new Color32(186, 30, 30, 255);
//
//        furnitureRequirements.Add("stockpile", new Job(null, "stockpile", FurnitureActions.JobComplete_FurnitureBuilding, -1f,null));
//
//        _furniturePrototypes.Add("smelter", new Furniture("smelter", 50, 2, 3, false, false));
//		_furniturePrototypes ["smelter"].jobSpotOffset = new Vector2 (1, 0);
//		_furniturePrototypes ["smelter"].RegisterUpdateAction (FurnitureActions.Smeltery_UpdateAction);
//
//
//    }

    public void RegisterJobFurniturePrototype(Job j, Furniture f)
    {
        furnitureRequirements[f.ObjectType] = j;
    }

	public void InitPrototypes(){
		//TODO: Read from xml files
		//In the future instead of using unity resources system, we'll read in from regular file or hd
		// hopefully get passed a data stream instead of hard coded paths.
		var furnText = Resources.Load<TextAsset> ("XMLData/Furniture");

		var xmlReader = XmlTextReader.Create(new StringReader(furnText.text));
		int furnProtoCount = 0;
		if (xmlReader.ReadToDescendant ("Furnitures")) {
			if (xmlReader.ReadToDescendant ("Furniture")) {
				do {
					furnProtoCount ++;

					Furniture furn = new Furniture();

					furn.ReadXmlPrototype(xmlReader);

					_furniturePrototypes[furn.ObjectType] = furn;

				} while(xmlReader.ReadToNextSibling ("Furniture"));
			} else {
				Debug.LogError ("no furniture elements in proto definition file.");
			}
		} else {
			Debug.LogError ("Did not find furnitures element in proto definition file.");
		}

		Debug.Log ("furn prototypes found in file: "+ furnProtoCount);

		//will come from lua file in future for now we need to run as hardcoded.
		//_furniturePrototypes ["FURN_BASIC_DOOR"].RegisterUpdateAction (FurnitureActions.Door_UpdateAction);
		//_furniturePrototypes ["FURN_BASIC_DOOR"].isEnterable = FurnitureActions.Door_IsEnterable;
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
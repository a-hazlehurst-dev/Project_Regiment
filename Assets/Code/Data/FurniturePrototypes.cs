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

    public void RegisterJobFurniturePrototype(Job j, Furniture f)
    {
        furnitureRequirements[f.ObjectType] = j;
    }

	void LoadFurnitureLua(){
		
		string filePath = System.IO.Path.Combine (Application.streamingAssetsPath, "LUA");
		filePath= System.IO.Path.Combine(filePath, "FurnitureActions.lua");
		//Debug.Log (filePath);
		string luaCode = File.ReadAllText (filePath);

		new FurnitureActions (luaCode);
	}

	public void InitPrototypes(){
		//TODO: Read from xml files
		LoadFurnitureLua();



		//loads furn xml data
		string filePath = System.IO.Path.Combine (Application.streamingAssetsPath, "Data");
		filePath= System.IO.Path.Combine(filePath, "Furniture.xml");
		string furnitureXmlText = File.ReadAllText (filePath);

		//Debug.Log (furnitureXmlText);

		var xmlReader = XmlTextReader.Create(new StringReader(furnitureXmlText));

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
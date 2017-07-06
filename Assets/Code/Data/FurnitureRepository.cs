using System;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureRepository
{
	//This will store the actual in built and installed furniture

	private List<Furniture> _furnitures;

	public FurnitureRepository ()
	{
		_furnitures = new List<Furniture> ();
	}

	public void Add(Furniture furn){
		_furnitures.Add (furn);
	}

	public List<Furniture> GetAll(){
		return _furnitures;
	}


}

public class FurnitureService{
	// this service will knit togehter functionality to control furniture.

	FurnitureBuilder builder;
	FurnitureRepository furnRepository;
	FurniturePrototypes furnPrototypes;

	Action<Furniture> cbOnCreated;

	public void Init()
	{
		
		furnPrototypes = new FurniturePrototypes ();
		furnRepository = new FurnitureRepository ();
		builder = new FurnitureBuilder (furnRepository,furnPrototypes);
	}

	public Furniture CreateFurniture(string type, Tile tile){
		var furniture = builder.CreateFurniture (type, tile);
        if(furniture == null) { return null; }
        if (cbOnCreated != null)
        {
            cbOnCreated(furniture);
        }
		return furniture;
	}

	public List<Furniture> FindAll(){
		return furnRepository.GetAll ();;
	}

	public bool IsValidPosition(string objectType, Tile tile){
		return furnPrototypes.Get (objectType).IsValidPosition (tile);
	}

	public void Register_OnFurniture_Created(Action<Furniture> cbCreatedFurnititure){
		cbOnCreated += cbCreatedFurnititure;
	}
	public void UnRegister_OnFurniture_Created(Action<Furniture> cbCreatedFurnititure){
		cbOnCreated -= cbCreatedFurnititure;
	}

}

public class FurnitureBuilder{

	private FurnitureRepository _furnitureRepository;
	private FurniturePrototypes _furniturePrototypes;

	public FurnitureBuilder(FurnitureRepository furnitureRepository, FurniturePrototypes furniturePrototypes){
		_furnitureRepository = furnitureRepository;
		_furniturePrototypes = furniturePrototypes;
	}

	public Furniture CreateFurniture(string type, Tile tileLocation){
		if (GameManager.Instance.TileDataGrid == null) {
			Debug.LogError ("Furniture Builder: Error, Cannot place furniture when World does not exist.");
			return null;
		}

		var furn = _furniturePrototypes.Get (type);
		var furnToPlace = Furniture.PlaceFurniture (furn, tileLocation);
        if(furnToPlace == null) { return null; }
		_furnitureRepository.Add (furnToPlace);

		return furnToPlace;
	}
}


public class FurniturePrototypes{

	public Dictionary<string, Furniture> _furniturePrototypes;

	public FurniturePrototypes(){
		_furniturePrototypes = new Dictionary<string, Furniture> ();
		InitPrototypes ();
	}

	public void InitPrototypes(){
		_furniturePrototypes.Add ("wall", Furniture.CreatePrototype ("wall", 0, 1, 1, true));
	}

	public Furniture Get(string furnitureType){
		if (!_furniturePrototypes.ContainsKey (furnitureType)) {
			Debug.Log("FurniturePrototypes Warning: There are no furniture prototypes of type " + furnitureType);
			return null;
		}

		return _furniturePrototypes [furnitureType];

	}

}


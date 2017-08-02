using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;

[MoonSharpUserData]
public class FurnitureService
{
    // this service will knit togehter functionality to control furniture.

    FurnitureBuilder builder;
    FurnitureRepository furnRepository;
    public FurniturePrototypes furnPrototypes;
   

    Action<Furniture> cbOnCreated;

    public void Init()
    {

        furnPrototypes = new FurniturePrototypes();
        
        furnRepository = new FurnitureRepository();
        builder = new FurnitureBuilder(furnRepository, furnPrototypes);
        furnPrototypes.InitPrototypes();
    }

    public Dictionary<string, Job> FindFurnitureRequirements()
    {
        return furnPrototypes.furnitureRequirements;
    }

   
    public Furniture CreateFurniture(string type, Tile tile, bool doRoomFloodFill = true)
    {
        var furniture = builder.CreateFurniture(type, tile);
        if (furniture == null) { return null; }

		furniture.RegisterOnRemovedCallback (OnFurnitureRemoved);

		//do we need to recalculate the rooms?
		if (doRoomFloodFill && furniture.RoomEnclosure) {
			Room.DoRoomFloodFill(tile);

        }

        if (cbOnCreated != null)
        {
            cbOnCreated(furniture);
			if(furniture.MovementCost !=1){
				//tiles return movement cost as a base * by furniture movement cost.
				//a furn movecost of 1, does not effect the pathfinding.
				GameManager.Instance.InvalidateTileGraph (); //reset pathfinding.
			}
        }
        return furniture;
    }

	public void Remove(Furniture furn){
		furnRepository.Remove (furn);
	}

    public List<Furniture> FindAll()
    {
        return furnRepository.GetAll(); ;
    }

    public bool IsValidPosition(string objectType, Tile tile)
    {
        return furnPrototypes.Get(objectType).DefaultIsPositionValid(tile);
    }

    public Dictionary<string,Furniture> FindPrototypes()
    {
        return furnPrototypes._furniturePrototypes;
    }

	public void OnFurnitureRemoved(Furniture furn){
		Remove (furn);
	}

    public void Register_OnFurniture_Created(Action<Furniture> cbCreatedFurnititure)
    {
        cbOnCreated += cbCreatedFurnititure;
    }
    public void UnRegister_OnFurniture_Created(Action<Furniture> cbCreatedFurnititure)
    {
        cbOnCreated -= cbCreatedFurnititure;
    }





}
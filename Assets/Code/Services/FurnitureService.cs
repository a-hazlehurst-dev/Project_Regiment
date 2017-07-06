using System;
using System.Collections.Generic;

public class FurnitureService
{
    // this service will knit togehter functionality to control furniture.

    FurnitureBuilder builder;
    FurnitureRepository furnRepository;
    FurniturePrototypes furnPrototypes;

    Action<Furniture> cbOnCreated;

    public void Init()
    {

        furnPrototypes = new FurniturePrototypes();
        furnRepository = new FurnitureRepository();
        builder = new FurnitureBuilder(furnRepository, furnPrototypes);
    }

    public Furniture CreateFurniture(string type, Tile tile)
    {
        var furniture = builder.CreateFurniture(type, tile);
        if (furniture == null) { return null; }
        if (cbOnCreated != null)
        {
            cbOnCreated(furniture);
        }
        return furniture;
    }

    public List<Furniture> FindAll()
    {
        return furnRepository.GetAll(); ;
    }

    public bool IsValidPosition(string objectType, Tile tile)
    {
        return furnPrototypes.Get(objectType).IsValidPosition(tile);
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
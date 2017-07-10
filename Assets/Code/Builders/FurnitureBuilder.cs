using UnityEngine;

public class FurnitureBuilder
{

    private FurnitureRepository _furnitureRepository;
    private FurniturePrototypes _furniturePrototypes;

    public FurnitureBuilder(FurnitureRepository furnitureRepository, FurniturePrototypes furniturePrototypes)
    {
        _furnitureRepository = furnitureRepository;
        _furniturePrototypes = furniturePrototypes;
    }

    public Furniture CreateFurniture(string type, Tile tileLocation)
    {
        if (GameManager.Instance.TileDataGrid == null)
        {
            Debug.LogError("Furniture Builder: Error, Cannot place furniture when World does not exist.");
            return null;
        }

        var furn = _furniturePrototypes.Get(type);
        var furnToPlace = Furniture.PlaceFurniture(furn, tileLocation);
        if (furnToPlace == null) { return null; }
        _furnitureRepository.Add(furnToPlace);


        return furnToPlace;
    }
}

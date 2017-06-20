using System;

public class Furniture  {

	public  Tile Tile { get; protected set; }					//base tile of object( what you place ) object can be bigger than 1 tile.
	public string ObjectType { get; protected set; }
	private float _movementCost = 1f;
	private int _width = 1;
	private int _height = 1;
    public bool LinksToNeighbour { get; protected set; }

	Action<Furniture> cbOnChanged;

	protected Furniture(){
	}

	static public Furniture CreatePrototype(string objectType, float movementCost = 1f, int width = 1, int height =1, bool linksToNeighbour = false){
		Furniture item = new Furniture ();

		item.ObjectType = objectType;
		item ._movementCost= movementCost;
		item._width = width;
		item._height = height;
        item.LinksToNeighbour = linksToNeighbour;

        return item;
	}

	static public Furniture PlaceFurniture(Furniture prototype,  Tile tile)
	{
		Furniture item = new Furniture ();

		item.ObjectType = prototype.ObjectType;
		item._movementCost = prototype._movementCost;
		item._width = prototype._width;
		item._height = prototype._height;

        item.LinksToNeighbour = prototype.LinksToNeighbour;

		item.Tile = tile;
		if (tile.PlaceObject (item)==false)  {
			//if we couldnt place the object.
			//it was likely already occupied
			//do NOT return the object;
			return null;
		}

        if (item.LinksToNeighbour)
        {
            //inform neighbours that they have a new tile        
            int x = tile.X;
            int y = tile.Y;

			var t = GameManager.Instance.TileDataGrid.GetTileAt(x, y + 1);
            if (t != null && t.InstalledFurniture != null && t.InstalledFurniture.ObjectType == item.ObjectType)
            {
                t.InstalledFurniture.cbOnChanged(t.InstalledFurniture); //we have northern neighbour with same object as us, so change it with callback;
            }

			t = GameManager.Instance.TileDataGrid.GetTileAt(x +1, y);
            if (t != null && t.InstalledFurniture != null && t.InstalledFurniture.ObjectType == item.ObjectType)
            {
                t.InstalledFurniture.cbOnChanged(t.InstalledFurniture);
            }

			t = GameManager.Instance.TileDataGrid.GetTileAt(x, y- 1);
            if (t != null && t.InstalledFurniture != null && t.InstalledFurniture.ObjectType == item.ObjectType)
            {
                t.InstalledFurniture.cbOnChanged(t.InstalledFurniture);
            }
			t = GameManager.Instance.TileDataGrid.GetTileAt(x-1, y );
            if (t != null && t.InstalledFurniture != null && t.InstalledFurniture.ObjectType == item.ObjectType)
            {
                t.InstalledFurniture.cbOnChanged(t.InstalledFurniture);
            }

        }

		return item;
	}
		
	public void RegisterOnChangedCallback(Action<Furniture> callBackFunc){
		cbOnChanged += callBackFunc;
	}
	public void UnRegisterOnChangedCallback(Action<Furniture> callBackFunc){
		cbOnChanged -= callBackFunc;
	}

}


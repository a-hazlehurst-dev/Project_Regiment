using System.Collections.Generic;

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

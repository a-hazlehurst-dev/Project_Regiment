using UnityEngine;

public class Inventory  {

	public string objectType = "clay";
	public int maxStackSize = 50;
	public int stackSize = 1;
	public Tile Tile;
	public Character Character;

	public Inventory(){

	}

	protected Inventory(Inventory other){
		objectType 		= other.objectType;
		maxStackSize 	= other.maxStackSize;
		stackSize 		= other.stackSize;
	}

	public virtual Inventory Clone(){
		return new Inventory (this);
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FurnitureItem  {

	public  Tile Tile { get; protected set; }					//base tile of object( what you place ) object can be bigger than 1 tile.
	private string _objectType;
	private float _movementCost = 1f;
	private int _width = 1;
	private int _height = 1;

	Action<FurnitureItem> cbOnChanged;

	protected FurnitureItem(){
	}

	static public FurnitureItem CreatePrototype(string objectType, float movementCost = 1f, int width = 1, int height =1){
		FurnitureItem item = new FurnitureItem ();

		item._objectType = objectType;
		item ._movementCost= movementCost;
		item._width = width;
		item._height = height;

		return item;
	}

	static public FurnitureItem PlaceFurniture(FurnitureItem prototype,  Tile tile)
	{
		FurnitureItem item = new FurnitureItem ();

		item._objectType = prototype._objectType;
		item._movementCost = prototype._movementCost;
		item._width = prototype._width;
		item._height = prototype._height;

		item.Tile = tile;
		if (tile.PlaceObject (item)==false)  {
			//if we couldnt place the object.
			//it was likely already occupied
			//do NOT return the object;
			return null;
		}

		return item;
	}
		
	public void RegisterOnChangedCallback(Action<FurnitureItem> callBackFunc){
		cbOnChanged += callBackFunc;
	}
	public void UnRegisterOnChangedCallback(Action<FurnitureItem> callBackFunc){
		cbOnChanged -= callBackFunc;
	}

}


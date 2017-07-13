using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryService
{

	/// <summary>
	/// list of all "live inventories", may also be organised by room in future.
	/// </summary>
	public Dictionary<string, List<Inventory>> _inventories;

	public Action<Inventory> cbInventoryCreated;
	public Action<Inventory> cbInventoryChanged;

	public void Init(){
		_inventories = new Dictionary<string, List<Inventory>> ();

	}


	// trying to place the inventory onto a tile.
	public bool PlaceInventory(Tile tile, Inventory inv){

		Debug.Log ("placing furniture");
		bool tileWasEmpty = tile.inventory == null;
		if (tile.PlaceInventory (inv) == false) {
 			//tile has not accepted the inventory
			Debug.Log("Failed to place Furniture");

			return false;
		}

		// "inv" maybe empty stack as it was merged with another stack.
		if (inv.stackSize == 0) {
			if (_inventories.ContainsKey (tile.inventory.objectType)) {
				_inventories [inv.objectType].Remove (inv);
			}
		}

		//also create a new stack if the tile was previously empty.
		if(tileWasEmpty){
			if (_inventories.ContainsKey (tile.inventory.objectType) == false) {
				_inventories [tile.inventory.objectType] = new List<Inventory> ();
			}
			_inventories[tile.inventory.objectType].Add(tile.inventory);
		}
		Debug.Log("placed returned true");
		return true;
	}


	public void Register_OnInventory_Created(Action<Inventory> cb){
		cbInventoryCreated += cb;
	}

	public void UnRegister_OnInventory_Created(Action<Inventory> cb){
		cbInventoryCreated -= cb;
	}

	public void Register_OnInventory_Changed(Action<Inventory> cb){
		cbInventoryChanged += cb;
	}

	public void UnRegister_OnInventory_Changed(Action<Inventory> cb){
		cbInventoryChanged -= cb;
	}

}


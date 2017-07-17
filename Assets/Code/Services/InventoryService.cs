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
		CleanUpInventory(inv);

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

	// trying to place the inventory onto a tile.
	public bool PlaceInventory(Job job, Inventory inv){

		if (job._inventoryRequirements.ContainsKey (inv.objectType) == false) {
			Debug.LogError ("Trying to add inventory to a job it doesnt want.");
			return false;
		}

		job._inventoryRequirements [inv.objectType].stackSize += inv.stackSize;
		if (job._inventoryRequirements [inv.objectType].maxStackSize < job._inventoryRequirements [inv.objectType].stackSize) {
			inv.stackSize = job._inventoryRequirements [inv.objectType].stackSize - job._inventoryRequirements [inv.objectType].maxStackSize;
			job._inventoryRequirements [inv.objectType].maxStackSize = job._inventoryRequirements [inv.objectType].maxStackSize;
		} else {
			inv.stackSize = 0;
		}

		// "inv" maybe empty stack as it was merged with another stack.
		CleanUpInventory(inv);


		return true;
	}

	// trying to place the inventory onto a tile.
	public bool PlaceInventory(Character character, Inventory inv){

		character.inventory.stackSize += inv.stackSize;

		if (character.inventory.maxStackSize <character.inventory.stackSize) {
			inv.stackSize = character.inventory.stackSize - character.inventory.maxStackSize; // set the inv
			character.inventory.maxStackSize = character.inventory.maxStackSize; // set the character
		} else {
			inv.stackSize = 0;
		}

		// "inv" maybe empty stack as it was merged with another stack.
		CleanUpInventory(inv);


		return true;
	}

	void CleanUpInventory(Inventory inv){
		if (inv.stackSize == 0) {
			if (_inventories.ContainsKey (inv.objectType)) {
				_inventories [inv.objectType].Remove (inv);
			}
			if (inv.Tile != null) {
				inv.Tile.inventory = null;
				inv.Tile = null;
			}
			if (inv.Character != null) {
				inv.Character.inventory = null;
				inv.Character = null;
			}
		}
	}

	public Inventory GetClosestInventoryOfType(string objectType, Tile t, int desiredAmount){
		//FIXME: we are lying about returning closest item.
		// No way to return item in optimal manner until the inventory db is more sophisticated.

		if (_inventories.ContainsKey (objectType) == false) {
			Debug.LogError ("GetClosestInventoryOfType: Trying to add inventory to a job it doesnt want.");
			return null;
		}

		foreach (var inv in _inventories[objectType]) {
			if (inv.Tile != null) {
				return inv;

			}
		}
		return null;

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


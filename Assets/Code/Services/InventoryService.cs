using System;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;


[MoonSharpUserData]
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

    #region constructors

    // trying to place the inventory onto a tile.
    public bool PlaceInventory(Tile tile, Inventory inv){

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
            //if(cbInventoryCreated!=null)
                cbInventoryCreated(tile.inventory);
		}

		return true;
	}

	// trying to place the inventory onto job.
	public bool PlaceInventory(Job job, Inventory inv){

		if (job._inventoryRequirements.ContainsKey (inv.objectType) == false) {
			Debug.LogError ("Trying to add inventory to a job it doesnt want.");
			return false;
		}

		job._inventoryRequirements [inv.objectType].StackSize += inv.StackSize;
		if (job._inventoryRequirements [inv.objectType].maxStackSize < job._inventoryRequirements [inv.objectType].StackSize) {
			inv.StackSize = job._inventoryRequirements [inv.objectType].StackSize - job._inventoryRequirements [inv.objectType].maxStackSize;
			job._inventoryRequirements [inv.objectType].maxStackSize = job._inventoryRequirements [inv.objectType].maxStackSize;
		} else {
			inv.StackSize = 0;
		}

		// "inv" maybe empty stack as it was merged with another stack.
		CleanUpInventory(inv);


		return true;
	}

	// trying to place the inventory onto a character
	public bool PlaceInventory(Character character, Inventory sourceInventory, int amount = -1){
        if(amount < 0)
        {
            amount = sourceInventory.StackSize;
        }
        else
        {
            amount = Mathf.Min(amount, sourceInventory.StackSize);
        }
        if(character.Inventory == null)
        {
            character.Inventory = sourceInventory.Clone();
            character.Inventory.StackSize = 0;
            _inventories[character.Inventory.objectType].Add(character.Inventory);
        }

		character.Inventory.StackSize += amount;

		if (character.Inventory.maxStackSize <character.Inventory.StackSize) {
			sourceInventory.StackSize = character.Inventory.StackSize - character.Inventory.maxStackSize; // set the inv
			character.Inventory.StackSize = character.Inventory.maxStackSize; // set the character
		} else {
			sourceInventory.StackSize -= amount;
		}

		// "inv" maybe empty stack as it was merged with another stack.
		CleanUpInventory(sourceInventory);


		return true;
	}

    #endregion

    void CleanUpInventory(Inventory inv){
		if (inv.StackSize == 0) {
			if (_inventories.ContainsKey (inv.objectType)) {
				_inventories [inv.objectType].Remove (inv);
			}
			if (inv.Tile != null) {
				inv.Tile.inventory = null;
				inv.Tile = null;
			}
			if (inv.Character != null) {
				inv.Character.Inventory = null;
				inv.Character = null;
			}
		}
	}

	public Inventory GetClosestInventoryOfType(string objectType, Tile t, int desiredAmount, bool canTakeFromStockpile){
		//FIXME: we are lying about returning closest item.
		// No way to return item in optimal manner until the inventory db is more sophisticated.

		if (_inventories.ContainsKey (objectType) == false) {
			Debug.LogError ("GetClosestInventoryOfType: Trying to add inventory to a job it doesnt want.");
			return null;
		}

		foreach (var inv in _inventories[objectType]) {
			if (inv.Tile != null &&( canTakeFromStockpile || inv.Tile.Furniture == null ||inv.Tile.Furniture.IsStockpile() == false)) {
				return inv;

			}
		}
		return null;

	}

    #region Callbacks


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
    #endregion
}


using System;

public  class CharacterDoJobAction
{
	public static  bool DoIHaveAJob(Character character){
		if (character.Job == null) {

			character.GetNewJob ();

			if (character.Job == null) {
				character.SetTileAsCurrent ();
				return false;
			}
		}

		return true;
	}

	public static bool DoesJobMeetRequirements(Character character){
		if (!character.Job.HasAllMaterial ()) {

			if (character.Inventory != null) {
				ProcessCarriedMaterial (character);
			} else {
				GetRequiredMaterial (character);
			}

			return false;
		}
		return true;
	}

	private void ProcessCarriedMaterial(Character character){
		//if im carrying the correct material
		if (character.Job.DesireInventoryType (character.Inventory)>0) {
			MoveToJobTileAndDropMaterialOff (character);
		} else {
			DumpUnwantedMaterials();
		}
	}
	private void MoveToJobTileAndDropMaterialOff(Character character){
		if (character.CurrentTile == character.Job.Tile) {
			//were already at job site so drop inventory.
			DropoffJobMaterials();

		} else {
			//still need to get to the site.
			character.DestinationTile = character.Job.Tile;
			return; // nothing to do.
		}
	}
	private void GetRequiredMaterial(Character character){

		if (character.CurrentTile.inventory != null 
			&& (character.Job.CanTakeFromStockpile ||character.CurrentTile.Furniture == null || character.CurrentTile.Furniture.IsStockpile() == false )
			&& character.Job.DesireInventoryType(character.CurrentTile.inventory) > 0) {
			//pick the stuff up.
			GameManager.Instance.InventoryService.PlaceInventory(this, character.CurrentTile.inventory, character.Job.DesireInventoryType(character.CurrentTile.inventory));

		}

		//FIXME : dum setup.
		//Find first inventory type we need from inventory.
		Inventory desired =  character.Job.GetFirstDesiredInventory();

		Inventory supplier = GameManager.Instance.InventoryService.GetClosestInventoryOfType (
			desired.objectType, 
			character.CurrentTile, 
			desired.maxStackSize - desired.StackSize, 
			character.Job.CanTakeFromStockpile
		);
		if (supplier !=null && supplier.Tile != null) {
			character.DestinationTile = supplier.Tile;
		}
		if (supplier == null) {
			
			character.AbandonJob ();
			character.DestinationTile = character.CurrentTile;
		}

		return;
	}
}



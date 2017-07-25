using System;
using UnityEngine;

public static class FurnitureActions
{
	public static void Door_UpdateAction(Furniture furn, float deltaTime){
		if (furn.GetParameter("is_opening") >= 1) {
			
			furn.ChangeParameter ("openness" , deltaTime * 2) ;
			if (furn.GetParameter ("openness") >= 1) {
				furn.SetParameter ("is_opening", 0);
			}
		} else {
			furn.ChangeParameter ("openness" , deltaTime * -2) ;
		}

		furn.SetParameter ("openness", Mathf.Clamp01 (furn.GetParameter ("openness")));

        if (furn.cbOnChanged != null)
        {
            furn.cbOnChanged(furn);
        }
	}

	public static Enterability Door_IsEnterable(Furniture furn){
		furn.SetParameter("is_opening", 1);

		if (furn.GetParameter("openness") >= 1) {
			return Enterability.Ok;
		} else {
			return Enterability.Wait;
		}
	}

    public static void JobComplete_FurnitureBuilding(Job theJob)
    {
        GameManager.Instance.FurnitureController.PlaceFurniture(theJob.JobObjectType, theJob.Tile);
        theJob.Tile.PendingFurnitureJob = null;

    }

    public static Inventory[] Stockpile_GetItemsFromFilter()
    {

        //TODO: should be reading from some ui for this stockpile.
        return new Inventory[1] { new Inventory("clay", 50, 0) };
    }

    public static void Stockpile_Update_Action(Furniture furn, float deltatime)
    {
        //we need a job on the queue either 


        //TODO: does not need to run on each update. 
        // lots of furniture, in a runnning game, will run more than required.
        // only needs to run, when a good gets delivered or
        //      - whenever it gets created,
        //      - good gets deliverd, (reset job)
        //      - good gets picked up (reset job)
        //      - ui, filter of allowed items get changed.
        if (furn.Tile.inventory != null){

        }
        
        if (furn.Tile.inventory!= null && furn.Tile.inventory.StackSize >= furn.Tile.inventory.maxStackSize)
        {
            //we are full!!
            furn.ClearJobs();
            return;
        }

        //maybe we already have a job queued.
        if (furn.JobCount() > 0)
        {
            return;
        }

        //Either we have some, or zero inventory.
        //or something is FUBAR

        Inventory[] itemsDesired;
        //(if we have stuff) then if we're still below max stacksize, then more of the same stuff plaese.
        if (furn.Tile.inventory == null)
        {
            
            itemsDesired = Stockpile_GetItemsFromFilter();
        
        }
        else
        {
         
            //tile already has inventory, ut its not full.
            //(if were empty) asking for ANY loose inventory to be brought to us.)
            Inventory desiredInv = furn.Tile.inventory.Clone();

            desiredInv.maxStackSize -= desiredInv.StackSize;
            desiredInv.StackSize = 0;

            itemsDesired = new Inventory[] { desiredInv };
        }
        //TODO: later on add stockpile priority so we can take from low priority and add to high priority.
        Job j = new Job(furn.Tile, null, null, 0, itemsDesired);
        j.CanTakeFromStockpile = false;
		j.furnitureToOperate = furn;
        j.RegisterJobWorkedCallback(Stockpile_JobWorked);
        furn.AddJob(j);

    }
     static void Stockpile_JobWorked(Job j)
    {
      
		j.furnitureToOperate.ClearJobs();
        //TODO; when stocipile logic is in place, 
        // 
        foreach(var inv in j._inventoryRequirements.Values)
        {
            if(    inv.StackSize > 0)
            {
              
                GameManager.Instance._inventoryService.PlaceInventory(j.Tile, inv);

                return; // should never end up with more than 1 inventory requirement with stacksize >0
            }
        }
    }

	public static void Smeltery_UpdateAction(Furniture furn, float deltaTime){

		//TODO: change, gas contribution, based on room volume.
		Debug.Log ("smelter updateaction.");
		furn.Tile.Room.ChangeEnvironment ("temperature", 0.1f * deltaTime); //replace hardcoded value;

		//add a job if one does not already exist.
		if (furn.JobCount() > 0) {
			return;
		}

		Tile jobSpotInventory = furn.GetJobSpotTile ();

		if (jobSpotInventory.inventory != null && (jobSpotInventory.inventory.StackSize >= jobSpotInventory.inventory.maxStackSize)) {
			//stack is full dont create a job.
			return;
		}

		Job j = new Job (
			jobSpotInventory,
			null,
			Smeltery_JobCompleted,
			1f,
			null
		);
		j.furnitureToOperate = furn;
		furn.AddJob (j);
	}

	public static void Smeltery_JobCompleted(Job job){

		GameManager.Instance._inventoryService.PlaceInventory (job.Tile, new Inventory ("clay", 50, 2));

		job.furnitureToOperate.RemoveJob(job);

	}

}


using System;
using UnityEngine;
using MoonSharp.Interpreter;


public  class FurnitureActions
{

	static FurnitureActions _Instance;

	Script _lua;

	public FurnitureActions(string rawLuaCode){
		//tell the lua interpreter to load all classes that are markted as moonsharpuserdata.

		UserData.RegisterAssembly ();
		_Instance = this;
		_lua = new Script ();
		_lua.DoString (rawLuaCode);
	}

	public static void CallFunctionsWithFurniture(string[] functionNames, Furniture furn, float deltaTime){
		foreach (var fn in functionNames) {
			var func =_Instance._lua.Globals [fn];

			if(func == null){
				Debug.LogError("'"+fn+"'"+ " is not a lua function");
			}

			Debug.Log ("Calling+ " + fn);
			var result = _Instance._lua.Call (func, furn ,deltaTime);
			Debug.Log ("found:  " + result);

		}

	}

	public static DynValue CallFunction(string functionName, params object[] args){

		var func =_Instance._lua.Globals [functionName];

		var result = _Instance._lua.Call (func, args);

		return result;

	}


	public static void JobComplete_FurnitureBuilding(Job theJob)
	    {
	        GameManager.Instance.FurnitureController.PlaceFurniture(theJob.JobObjectType, theJob.Tile);
	        theJob.Tile.PendingFurnitureJob = null;
	
	    }

}



//
//	public static void Door_UpdateAction(Furniture furn, float deltaTime){
//		if (furn.GetParameter("is_opening") >= 1) {
//			
//			furn.ChangeParameter ("openness" , deltaTime * 2) ;
//			if (furn.GetParameter ("openness") >= 1) {
//				furn.SetParameter ("is_opening", 0);
//			}
//		} else {
//			furn.ChangeParameter ("openness" , deltaTime * -2) ;
//		}
//
//		furn.SetParameter ("openness", Mathf.Clamp01 (furn.GetParameter ("openness")));
//
//        if (furn.cbOnChanged != null)
//        {
//            furn.cbOnChanged(furn);
//        }
//	}
//
//	public static Enterability Door_IsEnterable(Furniture furn){
//		furn.SetParameter("is_opening", 1);
//
//		if (furn.GetParameter("openness") >= 1) {
//			return Enterability.Ok;
//		} else {
//			return Enterability.Wait;
//		}
//	}
//
//   
//
//    public static Inventory[] Stockpile_GetItemsFromFilter()
//    {
//
//        //TODO: should be reading from some ui for this stockpile.
//        return new Inventory[1] { new Inventory("clay", 50, 0) };
//    }
//
//    public static void Stockpile_Update_Action(Furniture furn, float deltatime)
//    {
//        //we need a job on the queue either 
//
//
//        //TODO: does not need to run on each update. 
//        // lots of furniture, in a runnning game, will run more than required.
//        // only needs to run, when a good gets delivered or
//        //      - whenever it gets created,
//        //      - good gets deliverd, (reset job)
//        //      - good gets picked up (reset job)
//        //      - ui, filter of allowed items get changed.
//        if (furn.Tile.inventory != null){
//
//        }
//        
//        if (furn.Tile.inventory!= null && furn.Tile.inventory.StackSize >= furn.Tile.inventory.maxStackSize)
//        {
//            //we are full!!
//			furn.CancelJobs();
//            return;
//        }
//
//        //maybe we already have a job queued.
//        if (furn.JobCount() > 0)
//        {
//            return;
//        }
//
//        //Either we have some, or zero inventory.
//        //or something is FUBAR
//
//        Inventory[] itemsDesired;
//        //(if we have stuff) then if we're still below max stacksize, then more of the same stuff plaese.
//        if (furn.Tile.inventory == null)
//        {
//            
//            itemsDesired = Stockpile_GetItemsFromFilter();
//        
//        }
//        else
//        {
//         
//            //tile already has inventory, ut its not full.
//            //(if were empty) asking for ANY loose inventory to be brought to us.)
//            Inventory desiredInv = furn.Tile.inventory.Clone();
//
//            desiredInv.maxStackSize -= desiredInv.StackSize;
//            desiredInv.StackSize = 0;
//
//            itemsDesired = new Inventory[] { desiredInv };
//        }
//        //TODO: later on add stockpile priority so we can take from low priority and add to high priority.
//        Job j = new Job(furn.Tile, null, null, 0, itemsDesired);
//        j.CanTakeFromStockpile = false;
//		j.furnitureToOperate = furn;
//		j.Register_JobWorked_Callback(Stockpile_JobWorked);
//        furn.AddJob(j);
//
//    }
//     static void Stockpile_JobWorked(Job j)
//    {
//      
//		j.CancelJob ();
//
//        //TODO; when stocipile logic is in place, 
//        foreach(var inv in j._inventoryRequirements.Values)
//        {
//            if(    inv.StackSize > 0)
//            {
//              
//                GameManager.Instance.InventoryService.PlaceInventory(j.Tile, inv);
//
//                return; // should never end up with more than 1 inventory requirement with stacksize >0
//            }
//        }
//    }
//
//	public static void Smeltery_UpdateAction(Furniture furn, float deltaTime){
//
//		furn.Tile.Room.ChangeEnvironment ("temperature", 0.1f * deltaTime); //replace hardcoded value;
//		var spawnSpot = furn.GetSpawnSpotTile ();
//
//		if (furn.JobCount () > 0) {
//			//add a job if one does not already exist.
//		
//				if (spawnSpot.inventory !=null && (spawnSpot.inventory.StackSize >= spawnSpot.inventory.maxStackSize))
//				{
//					furn.CancelJobs ();
//				}
//				return;
//			
//		}
//		//if we get here then there is no current job. see if spawn spot is full.
//		if (spawnSpot.inventory !=null && (spawnSpot.inventory.StackSize >= spawnSpot.inventory.maxStackSize)) {
//			//we're full dont create a job
//			return;
//		}
//		//if we get here we need to create a new job.
//
//		Tile jobSpotInventory = furn.GetJobSpotTile ();
//
//		if (jobSpotInventory.inventory != null && (jobSpotInventory.inventory.StackSize >= jobSpotInventory.inventory.maxStackSize)) {
//			//stack is full dont create a job.
//			return;
//		}
//
//		Job j = new Job (
//			jobSpotInventory,
//			null,
//			Smeltery_JobCompleted,
//			1f,
//			null, 
//			true 	// repeats until destination tile is full.
//		);
//		j.furnitureToOperate = furn;
//	
//		furn.AddJob (j);
//	}
//
//	public static void Smeltery_JobCompleted(Job job){
//		GameManager.Instance.InventoryService.PlaceInventory (job.furnitureToOperate.GetSpawnSpotTile(), new Inventory ("clay", 50, 10));
//	}
//
//



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

        _lua.Globals["Inventory"] = typeof(Inventory);
        _lua.Globals["Job"] = typeof(Job);

        _lua.Globals["GameManager"] = typeof(GameManager);

		_lua.DoString (rawLuaCode);
	}

	public static void CallFunctionsWithFurniture(string[] functionNames, Furniture furn, float deltaTime){
		foreach (var fn in functionNames) {
			var func =_Instance._lua.Globals [fn];

			if(func == null){
				Debug.LogError("'"+fn+"'"+ " is not a lua function");
			}

			//Debug.Log ("Calling: "+ fn);
			var result = _Instance._lua.Call (func, furn ,deltaTime);
		}

	}

	public static DynValue CallFunction(string functionName, params object[] args){

		var func =_Instance._lua.Globals [functionName];
		Debug.Log ("Calling: "+ functionName);
		var result = _Instance._lua.Call (func, args);
		Debug.Log (result.String);

		return result;

	}


	public static void JobComplete_FurnitureBuilding(Job theJob)
	    {
	        GameManager.Instance.FurnitureSpriteRenderer.PlaceFurniture(theJob.JobObjectType, theJob.Tile);
	        theJob.Tile.PendingFurnitureJob = null;
	
	    }

}





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



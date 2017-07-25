using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Character {

	public float X { 
		get{ 
			if(nextTile == null){
				return CurrentTile.X;
			}

			return Mathf.Lerp (CurrentTile.X, nextTile.X, movementPercentage); }
	}
	public float Y { 
		get{ 
			if(nextTile == null){
				return CurrentTile.Y;
			}
			return Mathf.Lerp (CurrentTile.Y, nextTile.Y, movementPercentage); }
	}


	Job myJob;
	public Inventory inventory; // what im carrying. (not hear / or equipment)

	Action<Character> cbCharacterChanged;

	public Tile CurrentTile { get; protected set; }

	private Tile _destTile;

	Tile destTile 
	{ 
		get { return _destTile; }
		set
		{ 
				if (_destTile != value) {
					_destTile = value;
					pathAStar = null; // new destination will always invalidate path.
			}
		}
	}
	Tile nextTile;  // next tile in path finding sequence;
	PathAStar pathAStar;
	float speed = 5; //tiles per second.
	float movementPercentage;// goes from 0 to 1, when moveing to dest tile. 1 being destination


	public Character(Tile currentTile){

		CurrentTile = destTile = nextTile = currentTile; 
	}


	void Update_DoJob(float deltaTime){
		
		if (myJob == null) {
			GetNewJob ();

			if (myJob == null) {
				destTile = CurrentTile;
				return;
			}
		}
        //we have a job!! and we can get to it.
        //STEP1: doe the job have all materials it needs.

		if (myJob.HasAllMaterial () == false) {
			//no we are missing something.
			//Step2, are we carry anything that is required.
			if (inventory != null) {
				if (myJob.DesireInventoryType (inventory)>0) {
					//if so deliver them, walk to tile and drop them off.
					if (CurrentTile == myJob.Tile) {
						//were already at job site so drop inventory.
						GameManager.Instance._inventoryService.PlaceInventory (myJob, inventory);
                        myJob.DoWork(0);

						if (inventory.StackSize == 0) {
							inventory = null;
						} else {
							Debug.Log ("Character is still carrying inventory, which wshould not be.");
							inventory = null;
						}

					} else {
						//still need to get to the site.
						destTile = myJob.Tile;
						return; // nothing to do.
					}


				} else {
					//carrying something that the job doesnt want.
					//dump at feet. (or werever is closest);
					//TODO; go to nearest empty tile and dump it.
					if (GameManager.Instance._inventoryService.PlaceInventory (CurrentTile, inventory) == false) {
						Debug.LogError ("Tried to dump invemntory into invalid tile " + CurrentTile.X + ", " + CurrentTile.Y);
						//FIXME: this will loose inventory perminantly.
						inventory = null;
					}
				}
			} else {
                //job still wants materials but we arnt carrying any.


                //are we standing in a tile where theere are goods for oour desired job
              

                if (CurrentTile.inventory != null 
                    && (myJob.CanTakeFromStockpile ||CurrentTile.Furniture == null || CurrentTile.Furniture.IsStockpile() == false )
                    && myJob.DesireInventoryType(CurrentTile.inventory) > 0) {
                    //pick the stuff up.
                    GameManager.Instance._inventoryService.PlaceInventory(this, CurrentTile.inventory, myJob.DesireInventoryType(CurrentTile.inventory));

				}

				//FIXME : dum setup.
				//Find first inventory type we need from inventory.
				Inventory desired =  myJob.GetFirstDesiredInventory();

				Inventory supplier = GameManager.Instance._inventoryService.GetClosestInventoryOfType (
					desired.objectType, 
					CurrentTile, 
					desired.maxStackSize - desired.StackSize, 
                    myJob.CanTakeFromStockpile
				);
				if (supplier !=null && supplier.Tile != null) {
					destTile = supplier.Tile;
				}
                if (supplier == null) {
					Debug.Log ("No tile contains objects of type: "+ desired.objectType + " to satisfy desired amount.");
					AbandonJob ();
                    destTile = CurrentTile;
				}
				
				return;
			}
			// if not got to a tile to collect goods.
			// if already on tile with materials, then pick some up.
			return; // cant continue until all mats are present.
		}

		//make sure out destinationtile is the job stie.

		destTile = myJob.Tile;


		//are we there yet
		if (CurrentTile == destTile ) {
			myJob.DoWork (deltaTime);
		}

	}


	void GetNewJob(){
		myJob = GameManager.Instance.JobQueue.DeQueue ();
		if (myJob == null) {
			return;
		}
		destTile = myJob.Tile;
		myJob.RegisterJobCompletedCallback (OnJobEnded);
		myJob.RegisterJobCancelledCallback (OnJobEnded);

		//immediately check if job tile is reachable.

		pathAStar = new PathAStar (GameManager.Instance.TileDataGrid, CurrentTile, destTile);
		if (pathAStar.Length () == 0) {
			Debug.LogError ("PathASTAR returned no path to target job tile..");
			myJob.CancelJob();
			AbandonJob ();
			destTile = CurrentTile;
		}
	}
	public void AbandonJob(){
		nextTile = destTile = CurrentTile;
		GameManager.Instance.JobQueue.Enqueue (myJob);
		myJob = null;

	}

	void Update_DoMovement(float deltaTime){

		if (CurrentTile == destTile) 
		{
			pathAStar = null;
			return;
		} //already at destination.
       

		if (nextTile == null || nextTile == CurrentTile) {
			//get next tile from path finder.
			if (pathAStar == null || pathAStar.Length()==0) {
				pathAStar = new PathAStar (GameManager.Instance.TileDataGrid, CurrentTile, destTile);
				if (pathAStar.Length () == 0) {
					
					Debug.LogError ("PathASTAR returned no path to destination.");
					//FIXME: job should be reenqueued.
					myJob.CancelJob();
					AbandonJob ();
					return;
				
				}
				//ignore the first tile.
				nextTile = pathAStar.Dequeue();
			}
	
			//grab the tuke were about to enter.
			nextTile = pathAStar.Dequeue();

			if (nextTile == CurrentTile) {
				Debug.LogError ("UpdateMovement:+ next tile is currTile?");
			}
		}

		//should have valid next tile.

		float distToTravel = Mathf.Sqrt(
			Mathf.Pow (CurrentTile.X - nextTile.X, 2) + 
			Mathf.Pow (CurrentTile.Y - nextTile.Y, 2));

		if (nextTile.IsEnterable() == Enterability.Never) {
			// did a wall get built? reset pathing. invalidate the path.
			Debug.Log ("FIXME: character, pathed through 0 movemnt tile. (Unwalkable)");
			nextTile = null;
			pathAStar = null;
			return;
		} else if(nextTile.IsEnterable() == Enterability.Wait){
            //cant enter now but can soon enter, could be entering a door.
            // dont bail on the path, but slow down movement.
            return;
		}


		//distance travelled this update;
		float distThisFrame = (speed/ nextTile.MovementCost) * deltaTime;

		//percentage distance to destination.
		float percThisFrame = distThisFrame / distToTravel ;

		//increment that to movement percentage
		movementPercentage += percThisFrame;

		if (movementPercentage >= 1) {
			CurrentTile = nextTile;
			movementPercentage = 0;

			//FIXME? retain any overshot movement?;
		}
	}

	public void Update(float deltaTime){

		Update_DoJob (deltaTime);

		Update_DoMovement (deltaTime);


		if (cbCharacterChanged != null) {
			cbCharacterChanged (this);
		}
	}


	public void RegisterOnCharacterChangedCallback(Action<Character> cb){

		cbCharacterChanged += cb;
	}

	public void UnRegisterOnCharacterChangedCallback(Action<Character> cb){
		cbCharacterChanged -= cb;
	}


	public void OnJobEnded(Job j){
		// job was completed or was cancelled.
		if (j != myJob) {
			Debug.LogError ("Character being told about job thats not his. you forgot to unregister old job");
		}
        j.UnRegisterJobCancelledCallback(OnJobEnded);
        j.UnRegisterJobCompletedCallback(OnJobEnded);

		myJob = null;
	}
}

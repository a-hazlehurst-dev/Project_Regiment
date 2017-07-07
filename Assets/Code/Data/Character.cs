using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Character {

	public float X { 
		get{ return Mathf.Lerp (CurrentTile.X, nextTile.X, movementPercentage); }
	}
	public float Y { 
		get{ return Mathf.Lerp (CurrentTile.Y, nextTile.Y, movementPercentage); }
	}


	Job myJob;

	Action<Character> cbCharacterChanged;

	public Tile CurrentTile { get; protected set; }
	Tile destTile; // final destination
	Tile nextTile;  // next tile in path finding sequence;
	PathAStar pathAStar;
	float speed = 2; //tiles per second.
	float movementPercentage;// goes from 0 to 1, when moveing to dest tile. 1 being destination


	public Character(Tile currentTile){

		CurrentTile = destTile = nextTile = currentTile; 
	}


	void Update_DoJob(float deltaTime){
		
		if (myJob == null) {
			myJob = GameManager.Instance.JobQueue.DeQueue ();
			if (myJob != null) {
				//SetDestination (myJob.Tile);
				destTile = myJob.Tile;
				myJob.RegisterJobCompletedCallback (OnJobEnded);
				myJob.RegisterJobCancelledCallback (OnJobEnded);
	
			}
		}


		//are we there yet
		if (CurrentTile == destTile ) {
		//if(pathAStar != null & pathAStar.Length() ==1)
			
			if (myJob != null) {
				myJob.DoWork (deltaTime);
			}

		}

	}

	public void AbandonJob(){
		nextTile = destTile = CurrentTile;
		pathAStar = null;
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
					pathAStar = null;
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

	public void SetDestination(Tile destinationTile){
		if (!CurrentTile.IsNeighbour (destTile, true)) {
			Debug.Log ("Character: SetDestination : Our destination tile is not a neightbour");
		}
		destTile = destinationTile;
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

		myJob = null;
	}
}

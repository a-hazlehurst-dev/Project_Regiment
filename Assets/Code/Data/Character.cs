﻿using System.Collections;
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
	Tile destTile;
	Tile nextTile;  // next tile in path finding sequence;
	PathAStar pathAStar;
	float speed = 5; //tiles per second.
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
			}
			//grab next waypoint from the pathing system.
			nextTile = pathAStar.Dequeue();
			if (nextTile == CurrentTile) {
				Debug.LogError ("UpdateMovement:+ next tile is currTile?");
			}
		}

		//should have valid next tile.

		float distToTravel = Mathf.Sqrt(
			Mathf.Pow (CurrentTile.X - nextTile.X, 2) + 
			Mathf.Pow (CurrentTile.Y - nextTile.Y, 2));

		//distance travelled this update;
		float distThisFrame = speed * deltaTime;

		//percentage distance to destination.
		float percThisFrame = distThisFrame / distToTravel ;

		//increment that to movement percentage
		movementPercentage += percThisFrame;

		Debug.Log (movementPercentage);

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

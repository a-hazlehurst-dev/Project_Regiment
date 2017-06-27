﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Character {

	public float X { 
		get{ return Mathf.Lerp (CurrentTile.X, destTile.X, movementPercentage); }
	}
	public float Y { 
		get{ return Mathf.Lerp (CurrentTile.Y, destTile.Y, movementPercentage); }
	}


	Job myJob;

	Action<Character> cbCharacterChanged;

	public Tile CurrentTile { get; protected set; }
	Tile destTile;
	float speed = 50; //tiles per second.
	float movementPercentage;// goes from 0 to 1, when moveing to dest tile. 1 being destination


	public Character(Tile currentTile){

		CurrentTile = destTile =  currentTile; 
	}

	public void Update(float deltaTime){

		if (myJob == null) {
			myJob = GameManager.Instance.JobQueue.DeQueue ();
			if (myJob != null) {
				SetDestination (myJob.Tile);
				myJob.RegisterJobCancelledCallback (OnJobEnded);
				myJob.RegisterJobCompletedCallback (OnJobEnded);
			}
		}


		//are we there yet
		if (CurrentTile == destTile ) {
			if (myJob != null) {
				myJob.DoWork (deltaTime);
			}
			return;
		}

		//total distance from a to b
		float distToTravel = Mathf.Pow (CurrentTile.X - destTile.X, 2) + Mathf.Pow (CurrentTile.Y - destTile.Y, 2);

		//distance travelled this update;
		float distThisFrame = speed * deltaTime;

		//percentage distance to destination.
		float percThisFrame = distThisFrame / distToTravel ;

		//increment that to movement percentage
		movementPercentage += percThisFrame;

		if (movementPercentage >= 1) {
			CurrentTile = destTile;
			movementPercentage = 0;

			//FIXME? retain any overshot movement?;
		}
	
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

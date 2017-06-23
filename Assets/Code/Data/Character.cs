using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character {

	public float X { 
		get{ return Mathf.Lerp (CurrentTile.X, destTile.X, movementPercentage); }
	}
	public float Y { 
		get{ return Mathf.Lerp (CurrentTile.Y, destTile.Y, movementPercentage); }
	}

	public Tile CurrentTile { get; protected set; }
	Tile destTile;
	float speed = 2f; //tiles per second.
	float movementPercentage;// goes from 0 to 1, when moveing to dest tile. 1 being destination


	public Character(Tile currentTile){

		CurrentTile = destTile =  currentTile; 
	}

	public void Update(float deltaTime){

		//are we there yet
		if (CurrentTile == destTile) {
			return;
		}

		//total distance from a to b
		float distToTravel = Mathf.Pow (CurrentTile.X - destTile.X, 2) + Mathf.Pow (CurrentTile.Y - destTile.Y, 2);

		//distance travelled this update;
		float distThisFrame = speed * deltaTime;

		//percentage distance to destination.
		float percThisFrame = distToTravel / distThisFrame;

		//increment that to movement percentage
		movementPercentage += percThisFrame;

		if (movementPercentage >= 1) {
			CurrentTile = destTile;
			movementPercentage = 0;

			//FIXME? retain any overshot movement?;
		}
	}

	public void SetDestination(Tile destinationTile){
		if (!CurrentTile.IsNeighbour (destTile, true)) {
			Debug.Log ("Character: SetDestination : Our destination tile is not a neightbour");
		}

		destTile = destinationTile;
	}
}

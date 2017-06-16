using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	public int horizontalSpeed =1;
	public int verticalSpeed = 1;

	public GameObject cursorPointer;


	void Update(){

		Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		currentMousePosition.z = 0;
		Tile tileUnderMouse = GetTileAtWorldCoordinate (currentMousePosition);

		Vector3 cursorPosition = new Vector3 (tileUnderMouse.X, tileUnderMouse.Y, 0);

		cursorPointer.transform.position = cursorPosition;

		if (Input.GetMouseButton(2))
		{
			var h = verticalSpeed * Input.GetAxis ("Mouse Y") *-1;
			var v = horizontalSpeed * Input.GetAxis ("Mouse X") *-1;
			transform.Translate (v,h, 0);
		}

	}

	Tile GetTileAtWorldCoordinate(Vector3 coordinate){

		int x = Mathf.FloorToInt (coordinate.x);
		int y = Mathf.FloorToInt (coordinate.y);

		return GameManager.Instance.GridManager.GetTileAt (x, y);
	}
		

			
}

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
		Tile tileUnderMouse = GameManager.Instance.GetTileAtWorldCoordinate (currentMousePosition);
        if (tileUnderMouse == null) return;

		Vector3 cursorPosition = new Vector3 (tileUnderMouse.X, tileUnderMouse.Y, 0);

		cursorPointer.transform.position = cursorPosition;


        if (Input.GetMouseButton(0))
        {
            var drawMode = GameManager.Instance.GetDrawMode();
            var tile = GameManager.Instance.GetTileAtWorldCoordinate(currentMousePosition);
            tile.Floor = drawMode;
        }

        if (Input.GetMouseButton(1))
        {
            var tile = GameManager.Instance.GetTileAtWorldCoordinate(currentMousePosition);
            tile.Floor = Tile.FloorType.Grass;
        }

        if (Input.GetMouseButton(2))
		{
			var h = verticalSpeed * Input.GetAxis ("Mouse Y") *-1;
			var v = horizontalSpeed * Input.GetAxis ("Mouse X") *-1;
			transform.Translate (v,h, 0);
		}

        Camera.main.orthographicSize -= (Camera.main.orthographicSize/2) * Input.GetAxis("Mouse ScrollWheel") *1.5f;

	}

	

			
}

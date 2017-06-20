using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	public int horizontalSpeed =1;
	public int verticalSpeed = 1;
	private int _drawMode = 0;
	private string _drawObjectMode;

	public GameObject cursorPointer;



	void Update(){

		Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		currentMousePosition.z = 0;
		Tile tileUnderMouse = GameManager.Instance.GetTileAt (currentMousePosition);
        if (tileUnderMouse == null) return;

		Vector3 cursorPosition = new Vector3 (tileUnderMouse.X, tileUnderMouse.Y, 0);

		cursorPointer.transform.position = cursorPosition;


        if (Input.GetMouseButton(0))
        {
			_drawMode= GameManager.Instance.GetDrawMode();
			_drawObjectMode= GameManager.Instance.GetDrawObjectMode();
            if (_drawObjectMode== null) { return; }

			var tile = GameManager.Instance.GetTileAt(currentMousePosition);

			Tile.FloorType floorMode = Tile.FloorType.Grass;//default draw mode

			if (_drawMode == 1) {
				Debug.Log (_drawObjectMode);
				if(_drawObjectMode.Equals("grass")){
					floorMode =  Tile.FloorType.Grass;
				}
				else if(_drawObjectMode.Equals("mud")){
					floorMode = Tile.FloorType.Mud;
				}
				tile.Floor = floorMode;

			} else if (_drawMode == 2) {
				if(_drawObjectMode.Equals("wall")){
					GameManager.Instance.FurnitureManager.PlaceFurniture ("wall", tile);
				}
				else if(_drawObjectMode.Equals("path")){
					GameManager.Instance.FurnitureManager.PlaceFurniture ("path", tile);
				}	

			}


            
        }

        if (Input.GetMouseButton(1))
        {
			var tile = GameManager.Instance.GetTileAt(currentMousePosition);
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

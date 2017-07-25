using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileInspectorViewModel : MonoBehaviour {

    CameraScript camScript;
    Text[] txtTileType;

    void Start()
    {
        txtTileType = GetComponentsInChildren<Text>();
        camScript = Camera.main.GetComponent<CameraScript>();

        if(txtTileType == null)
        {
            Debug.LogError("No tile type text component");
            this.enabled = false;
            return;
        }
    }
	// Update is called once per frame
	void Update () {
        Tile t = camScript.GetMouseOverTile();
        if(t == null) { return; }
        if(t.Room == null) { return; }
        txtTileType[0].text = "Tile type: " + t.Floor.ToString() + "(" + t.X + ","+t.Y +")";
		txtTileType[1].text = "Room " + GameManager.Instance.FindRooms().IndexOf(t.Room).ToString();
		txtTileType [2].text = "Details: ";
		foreach (var environment in t.Room.GetEnvironmentNames()) {
			var temp = "";
			if (environment == "temperature") {
				temp = "C";
			}
			txtTileType [2].text += t.Room.GetEnviromenntAmount(environment).ToString("0.0") + temp ;
		}

        if (t.Furniture != null)
        {
            txtTileType[3].text = "Furniture: " + t.Furniture.ObjectType;
        }
        else { txtTileType[3].text = "Furniture: none"; }
        
    }
}

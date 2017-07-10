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
        txtTileType[1].text = "Room type: " + t.Room.Name;
        if (t.InstalledFurniture != null)
        {
            txtTileType[2].text = "Furniture: " + t.InstalledFurniture.ObjectType;
        }
        else { txtTileType[2].text = "Furniture: none"; }
        
    }
}

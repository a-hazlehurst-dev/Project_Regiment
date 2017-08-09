using UnityEngine;
using UnityEngine.UI;

public class TileInspectorViewModel : MonoBehaviour {

    private CameraScript camScript;
    private Text[] txtTileType;

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
        txtTileType[0].text = "Tile type: " + t.Floor.ToString() + "(" + t.X + ","+t.Y +")";

        string roomId = "N/A";
        if (t.Room != null)
        {
            roomId = "Room " + t.Room.Id.ToString();
        }
        txtTileType[1].text = roomId;


		txtTileType [2].text = "Details: ";
		if (t.Room == null) {
			return;
		}
		foreach (var environment in t.Room.GetEnvironmentNames()) {
			var temp = "";
			if (environment == "temperature") {
				temp = "C";
			}
			txtTileType [2].text += t.Room.GetEnviromenntAmount(environment).ToString("0.0") + temp ;
		}

        if (t.Furniture != null)
        {
            txtTileType[3].text = "Furniture: " + t.Furniture.Name;
        }
        else { txtTileType[3].text = "Furniture: none"; }
        
    }
}

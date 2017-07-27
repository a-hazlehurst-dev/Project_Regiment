using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureBuildMenu : MonoBehaviour {

	public GameObject BuildFurniturePrefab;
	// Use this for initialization
	void Start () {
		GameDrawMode gdm = GameObject.FindObjectOfType<GameDrawMode> ();

		//foreach 
		foreach(var key in GameManager.Instance.FurnitureService.FindPrototypes().Keys){
			GameObject go = (GameObject)Instantiate (BuildFurniturePrefab);
			go.transform.SetParent (this.transform);
            string objectId = key;
            string objectName = GameManager.Instance.FurnitureService.FindPrototypes()[key].Name;

            go.name = "btn build " + key;
			go.GetComponentInChildren<Text> ().text = "Build "+ objectName;

			

			Button b = go.GetComponent<Button> ();
			b.onClick.AddListener (delegate { gdm.SetupMode_BuildFurniture(objectId);});

		}

	}
	

}

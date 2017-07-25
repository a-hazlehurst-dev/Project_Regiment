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
		foreach(var key in GameManager.Instance._furnitureService.FindPrototypes().Keys){
			GameObject go = (GameObject)Instantiate (BuildFurniturePrefab);
			go.transform.SetParent (this.transform);

			go.name = "btn build " + key;
			go.GetComponentInChildren<Text> ().text = "Build "+ key;

			string objectId = key;

			Button b = go.GetComponent<Button> ();
			b.onClick.AddListener (delegate { gdm.SetupMode_BuildFurniture(objectId);});

		}

	}
	

}

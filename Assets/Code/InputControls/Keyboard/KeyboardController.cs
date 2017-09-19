using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour {

    public GameDrawMode GameDrawMode;

    // Use this for initialization
    void Start () {
        GameDrawMode = GameObject.FindObjectOfType<GameDrawMode>();
    }
	
	// Update is called once per frame
	void Update () {


        if (Input.GetKeyUp(KeyCode.Escape))
        {
            GameDrawMode.GameBuildMode = BuildMode.None;
        }
        if (Input.GetKeyUp(KeyCode.F1))
        {
            GameDrawMode.GameBuildMode = BuildMode.Select;
        }
        if (Input.GetKeyUp(KeyCode.F2))
        {
            GameDrawMode.GameBuildMode = BuildMode.Furniture;
        }
    }
}

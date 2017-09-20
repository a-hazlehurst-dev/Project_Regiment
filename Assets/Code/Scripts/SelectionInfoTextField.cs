using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionInfoTextField : MonoBehaviour {

    CameraScript _cameraScript;
    GameDrawMode GameDrawMode;
    Text txt;
    public CanvasGroup CanvasGroup;
	// Use this for initialization
	void Start () {
        _cameraScript = FindObjectOfType<CameraScript>();
        GameDrawMode = GameManager.Instance.GameDrawMode;
    }
	
	// Update is called once per frame
	void Update () {

        if (GameDrawMode.GameBuildMode != BuildMode.Select)
        {

            CanvasGroup.alpha = 0f;
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
            return;
        }
        if (_cameraScript.MouseDrawing.SelectionInfo== null) { return; }

        CanvasGroup.alpha =1f;
        CanvasGroup.interactable = true;
        CanvasGroup.blocksRaycasts = false;
        
        ISelectableItem result = _cameraScript.MouseDrawing.SelectionInfo.Content[_cameraScript.MouseDrawing.SelectionInfo.SubSelect];

        GetComponent<Text>().text = result.Getname() + " " + result.GetDescription() + " " + result.GetHitPointsToString();
	}
}

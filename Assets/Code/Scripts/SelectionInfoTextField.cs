using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionInfoTextField : MonoBehaviour {

    CameraScript _cameraScript;
    Text txt;
    public CanvasGroup CanvasGroup;
	// Use this for initialization
	void Start () {
        _cameraScript = FindObjectOfType<CameraScript>();
    }
	
	// Update is called once per frame
	void Update () {

        if (_cameraScript.SelectionInfo == null)
        {

            CanvasGroup.alpha = 0f;
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
            return;
        }

        CanvasGroup.alpha =1f;
        CanvasGroup.interactable = true;
        CanvasGroup.blocksRaycasts = false;
        
        if (_cameraScript.SelectionInfo.Content == null) { return; }
        Debug.Log(_cameraScript.SelectionInfo.Content);
        ISelectableItem result = _cameraScript.SelectionInfo.Content[_cameraScript.SelectionInfo.SubSelect];
        

        GetComponent<Text>().text = result.Getname() + " " + result.GetDescription() + " " + result.GetHitPointsToString();
	}
}

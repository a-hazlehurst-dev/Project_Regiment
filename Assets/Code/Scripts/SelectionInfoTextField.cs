using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class SelectionInfoTextField : MonoBehaviour {

    CameraScript _cameraScript;
    GameDrawMode GameDrawMode;
    public GameObject ButtonPanel;
    Text txt;
    public CanvasGroup CanvasGroup;
    private int subSelect = -1;
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

        if (subSelect != _cameraScript.MouseDrawing.SelectionInfo.SubSelect)
        {
            List<GameObject> list = new List<GameObject>();
            for (int x = 0; x < ButtonPanel.transform.childCount; x++)
            {
                var child = ButtonPanel.transform.GetChild(x);
              
                    list.Add(child.gameObject);
               
            }

            foreach (var item in list.ToArray())
            {
                Destroy(item);
            }

            subSelect = _cameraScript.MouseDrawing.SelectionInfo.SubSelect;
            ISelectableItem result = _cameraScript.MouseDrawing.SelectionInfo.Content[_cameraScript.MouseDrawing.SelectionInfo.SubSelect];

            GetComponent<Text>().text = result.Getname() + " " + result.GetDescription() + " " + result.GetHitPointsToString();

            if (result.Buttons().Any())
            {
                foreach (var buttonName in result.Buttons())
                {
                    GameObject button = new GameObject();
                    button.name = buttonName; 
                    button.transform.SetParent(ButtonPanel.transform);
                    button.AddComponent<RectTransform>();
                    button.AddComponent<CanvasRenderer>();
                    button.AddComponent<Image>();
                    button.AddComponent<Button>();

                    GameObject text = new GameObject();
                   var textComponent =  text.AddComponent<Text>();
                    textComponent.text = buttonName;
                    textComponent.font = new Font("Arial");
                    textComponent.color = new Color(0, 0, 0);
                    text.transform.SetParent(button.transform);
                    text.AddComponent<CanvasRenderer>();
                }


                // button.GetComponent<Button>().onClick.AddListener(method);
            }
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class SelectionInfoTextField : MonoBehaviour {

    CameraScript _cameraScript;
    GameDrawMode GameDrawMode;
    public GameObject ButtonPanel;
    public GameObject PlaceholderPanel;
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
            MakeVisible(false);
            return;
        }
        
        if (!IsSomethingSelected()) { return; }

        MakeVisible(true);

        if (HasSelectedItemChanged())
        {
            subSelect = _cameraScript.MouseDrawing.SelectionInfo.SubSelect;

            ClearPanel(ButtonPanel);
            ClearPanel(PlaceholderPanel);

            ISelectableItem result = _cameraScript.MouseDrawing.SelectionInfo.Content[_cameraScript.MouseDrawing.SelectionInfo.SubSelect];

            GetComponentInChildren<Text>().text = result.Getname();

            if (result.Buttons().Any())
            {
                foreach (var buttonName in result.Buttons())
                {
                    CreateButton(buttonName);
                }

            }
        }   


    }


    private void CreateButton(string buttonName)
    {
        var btn = GameManager.Instance.PrefabManager.Buttons["btnMini"];

        GameObject instance = (GameObject)Instantiate(btn);
        instance.transform.SetParent(ButtonPanel.transform);

        instance.name = "btn" + buttonName;
        instance.GetComponentInChildren<Text>().text = buttonName;

        Button t = instance.GetComponent<Button>();

        t.onClick.AddListener(delegate { BuildInfoPanel(buttonName); });
    }


    #region callbacks
    private void BuildInfoPanel(string pnlName)
    {
        ClearPanel(PlaceholderPanel);
        var pnl = "pnl" + pnlName;
        Debug.Log(pnl);
        var btn = GameManager.Instance.PrefabManager.Panels[pnl];
        GameObject instance = (GameObject)Instantiate(btn, new Vector3(0,0,0),Quaternion.identity );

        instance.transform.SetParent(PlaceholderPanel.transform);

    }
    #endregion


    #region "Private helpers"
    bool HasSelectedItemChanged()
    {
        return subSelect != _cameraScript.MouseDrawing.SelectionInfo.SubSelect ? true : false;
    }

    bool IsSomethingSelected()
    {
        return _cameraScript.MouseDrawing.SelectionInfo != null ? true : false;
    }

    private void MakeVisible(bool isVisable)
    {
        if (isVisable)
        {
            CanvasGroup.alpha = 1f;
            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;
        }
        else
        {

            CanvasGroup.alpha = 0f;
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
        }
    }

    private void ClearPanel(GameObject panelToClear)
    {
        List<GameObject> list = new List<GameObject>();
        for (int x = 0; x < panelToClear.transform.childCount; x++)
        {
            var child = panelToClear.transform.GetChild(x);

            list.Add(child.gameObject);

        }

        foreach (var item in list.ToArray())
        {
            Destroy(item);
        }
    }
   
    #endregion
}





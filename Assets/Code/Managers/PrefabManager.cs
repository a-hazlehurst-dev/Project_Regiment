using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{

    public Dictionary<string, GameObject> Buttons;
    public Dictionary<string, GameObject> Panels;

    public void Init()
    {
        Buttons = new Dictionary<string, GameObject>();
        Panels = new Dictionary<string, GameObject>();
        LoadPrefabs();
    }

    private void LoadPrefabs()
    {

        LoadButtons();
        LoadPanels();


    }

    private void LoadButtons()
    {
        var ui = Resources.LoadAll<GameObject>("Prefabs/UI/Buttons");
        foreach (var item in ui)
        {
            Buttons.Add(item.name, item);
        }
    }

    private void LoadPanels()
    {
        var ui = Resources.LoadAll<GameObject>("Prefabs/UI/Panels");

        foreach (var item in ui)
        {
            Panels.Add(item.name, item);
        }
    }
}


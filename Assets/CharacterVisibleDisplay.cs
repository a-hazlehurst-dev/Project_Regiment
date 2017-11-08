using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterVisibleDisplay : MonoBehaviour {

    public Brain brain;
    public Text txtThingsHeard;
    public Text txtThingsSeen;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        txtThingsHeard.text = "Things Heard : " + brain.myMemoryOfHeard.Count.ToString();
        txtThingsSeen.text = "Things Seen:" + brain.myMemoryOfSeen.Count.ToString();

    }
}

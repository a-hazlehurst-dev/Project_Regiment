using Assets.Code.Models;
using UnityEngine;
using UnityEngine.UI;

public class PeasentModel : MonoBehaviour {

    public Peasant data;
    public Text txtName;
	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
        if(!txtName.text.Equals(data.Name + " " + data.Lastname))
        {
            txtName.text = data.Name + " " + data.Lastname;
        }
       
	}
}

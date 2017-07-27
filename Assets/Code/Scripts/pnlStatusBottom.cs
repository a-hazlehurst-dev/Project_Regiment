using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pnlStatusBottom : MonoBehaviour {

	public Text jobs;
	// Use this for initialization
	void Start () {
		GameManager.Instance.JobService.Register_Job_Created (OnJobCreated);
	}
	
	// Update is called once per frame

	public void OnJobCreated(Job j){
		jobs.text = "Jobs: "+ GameManager.Instance.JobService.FindAll ().Count;
	}
}

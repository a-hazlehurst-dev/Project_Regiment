using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobSpriteManager : MonoBehaviour {

	//this bare bones controller is going to barrow from furniture manager.

	FurnitureController _furnitureManager;
    
	private  Transform jobHolder;
	Dictionary<Job, GameObject>  _jobToGameObjectMap;
	// Use this for initialization
	void Start () {
		_jobToGameObjectMap = new Dictionary<Job, GameObject> ();
		_furnitureManager = GameObject.FindObjectOfType<FurnitureController> ();
		jobHolder = new GameObject ("Jobs").transform;
		GameManager.Instance.JobQueue.RegisterJobCreatedCallBack (OnJobCreated);

	}

	void OnJobCreated(Job job)
	{
		GameObject job_go = new GameObject();


		if (_jobToGameObjectMap.ContainsKey (job)) {
			Debug.Log ("ON Job Created: attempting to create a job graphic, where graphic already exists (probable, re queue not job created!");
			return;
		}

		_jobToGameObjectMap.Add(job, job_go);

		job_go.name = "Job: " + job.JobObjectType + ": x: " + job.Tile.X + ", y" + job.Tile.Y;
		job_go.transform.position = new Vector3(job.Tile.X, job.Tile.Y, 0);
		job_go.transform.SetParent (jobHolder, true);

		Debug.Log ("job created: "+ job + ", (" + job_go+")");
		SpriteRenderer sr = job_go.AddComponent<SpriteRenderer> ();
		sr.sortingLayerName = "Job";
		sr.sprite = _furnitureManager.GetSpriteForFurniture(job.JobObjectType); 
		sr.color = new Color (.2f, .2f, .2f, 0.5f); //transparent



		job.RegisterJobCompletedCallback (OnJobCompleted);
		job.RegisterJobCancelledCallback (OnJobCompleted);
	}

	void OnJobCompleted (Job job){

		GameObject job_go = _jobToGameObjectMap [job];

		job.UnRegisterJobCancelledCallback (OnJobCompleted);
		job.UnRegisterJobCompletedCallback (OnJobCompleted);

		Destroy (job_go);
	}
}

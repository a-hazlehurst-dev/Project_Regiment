using UnityEngine;
using UnityEngine.UI;

public class jobView : MonoBehaviour {

    public GameObject ui_text;
    private JobService _jobService;
	void Start () {


        GameManager.Instance.JobService.Register_Job_Created(OnCreated);
        _jobService = GameManager.Instance.JobService;
    }

    void Update()
    {

    }

    void OnCreated(Job j)
    {

        var jobs = transform.GetComponentsInChildren<Text>();
        for (int i = 0; i < jobs.Length; i++)
        {
            Destroy(jobs[i]);
        }

        var result = _jobService.FindAll();
   
        if (result.Count > 0)
        { 
        
            
            foreach (var job in result)
            {
                job.RegisterJobCompletedCallback(OnJobComplete);
                var temp = job;

                GameObject go = (GameObject)Instantiate(ui_text);

                go.GetComponentInChildren<Text>().text = job.JobObjectType;
                go.transform.SetParent(this.transform);
            }
        }
    }


    void OnJobComplete(Job j)
    {
        var jobs = transform.GetComponentsInChildren<Text>();
        for (int i = 0; i < jobs.Length; i++)
        {
            Destroy(jobs[i]);
        }

        var result = _jobService.FindAll();

        if (result.Count > 0)
        {
            foreach (var job in result)
            {
                
                var temp = job;

                GameObject go = (GameObject)Instantiate(ui_text);

                go.GetComponentInChildren<Text>().text = job.JobObjectType;
                go.transform.SetParent(this.transform);
            }
        }

    }
	
	
}

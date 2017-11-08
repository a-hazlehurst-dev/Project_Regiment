using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour {

    public Sight SightScript;
    public Sound SoundScript;
    public Rigidbody2D rigidBody;

    public Dictionary<string, GameObject> myMemoryOfSeen;
    public Dictionary<string, GameObject> myMemoryOfHeard;
    // Use this for initialization

    public GameObject target;
    void Start() {

        rigidBody =gameObject.transform.parent.gameObject.GetComponent<Rigidbody2D>();
        myMemoryOfSeen = new Dictionary<string, GameObject>();
        myMemoryOfHeard = new Dictionary<string, GameObject>();

        SightScript.Register_SeenSomething(OnSeenSomething);
        SightScript.Register_OnLostSight(OnLostSight);
        SoundScript.Register_HeardSomething(OnHeardSomething);
        SoundScript.Register_HeardingExit(OnHeardExit);

    }

    // Update is called once per frame
    void Update() {
        if(target!=null)
            rigidBody.transform.position = Vector3.MoveTowards(rigidBody.transform.position, target.transform.position, 0.1f);
    }

    public void OnSeenSomething(GameObject go)
    {
        Debug.Log("Seen " + go.gameObject.name);
        if (!myMemoryOfSeen.ContainsKey(go.name)) {
            myMemoryOfSeen.Add(go.name, go);
        }
        target = go;
    }
    public void OnLostSight(GameObject go)
    {
        Debug.Log("Lost sight of " + go.gameObject.name);
        if (myMemoryOfSeen.ContainsKey(go.name))
        {
            myMemoryOfSeen.Remove(go.name);
        }
    }

    public void OnHeardSomething(GameObject go)
    {
        Debug.Log("heard " + go.gameObject.name);
        if (!myMemoryOfHeard.ContainsKey(go.name))
        {
            myMemoryOfHeard.Add(go.name, go);
        }
        target = go;
    }

    public void OnHeardExit(GameObject go)
    {
        Debug.Log("cannot hear " + go.gameObject.name);
        if (myMemoryOfHeard.ContainsKey(go.name))
        {
            myMemoryOfHeard.Remove(go.name);
        }
    }
}

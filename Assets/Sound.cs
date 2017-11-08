using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour {

    private Action<GameObject> On_HeardSomething;
    private Action<GameObject> On_HearingExit;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        On_HeardSomething(collision.gameObject);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        On_HearingExit(collision.gameObject);
    }

    public void Register_HeardSomething(Action<GameObject> detected)
    {
        On_HeardSomething += detected;
    }
    public void Register_HeardingExit(Action<GameObject> detected)
    {
        On_HearingExit += detected;
    }
}

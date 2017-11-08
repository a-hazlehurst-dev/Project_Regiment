using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
    public Action<GameObject> On_SeenSomething;
    public Action<GameObject> On_LostSight;

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
        On_SeenSomething(collision.gameObject);
    }

  

    private void OnTriggerExit2D(Collider2D collision)
    {
        On_LostSight(collision.gameObject);
    }
    public void Register_SeenSomething(Action<GameObject> detected)
    {
        On_SeenSomething += detected;
    }

    public void Register_OnLostSight(Action<GameObject> detected)
    {
        On_LostSight += detected;
    }

}

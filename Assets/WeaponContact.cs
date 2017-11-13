using System;
using UnityEngine;

public class WeaponContact : MonoBehaviour {


    public Action<GameObject> On_HitSomething;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        On_HitSomething(collision.gameObject);
    }

    public void Register_HitSomething(Action<GameObject> detected)
    {
        On_HitSomething += detected;
    }
}

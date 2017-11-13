using System;
using UnityEngine;

public class OnWeaponHitMeTrigger : MonoBehaviour {


    private Action<GameObject> On_WeaponHitMe;

	private void OnTriggerEnter2D(Collider2D collision)
    {
        //TODO: if was hit by weapon, then raise weapon hit me 
        if (collision.gameObject.tag == "Weapon")
        {
            On_WeaponHitMe(collision.gameObject);
        }
    }


    public void Register_OnWeaponHitMe(Action<GameObject> cb)
    {
        On_WeaponHitMe += cb;
    }
}

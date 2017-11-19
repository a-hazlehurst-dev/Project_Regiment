using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarState : MonoBehaviour {


    public Slider slider;
    private int percentWidth;
    public Brain Brain;
    private int max;
    private int currentHP;
    private int pixelsPerHP;
    private float percent;
    // Use this for initialization

    private void OnEnable()
    {

    }
    void Start() {

      }
	
	// Update is called once per frame
	void Update () {

        max = Brain.Character.MaxHitPoints;
        currentHP = Brain.Character.HitPoints;

        var maxPerc = (max * 1.0f);

        slider.maxValue = max;

        var cr = Brain.Character.HitPoints * 1.0f; ;

        var val = cr/ maxPerc  ;
        Debug.Log(Brain.root.name + " HP:" + Brain.Character.HitPoints + "( "+val+")");
        
        slider.value = Brain.Character.HitPoints;    


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour {

    private static bool moving;
	// Use this for initialization
	void Start () {
        moving = false;
        Time.timeScale = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TriggerAnimation(){
        if(!moving){
            Time.timeScale = 1;
            moving = true;
        } else {
            Time.timeScale = 0;
            moving = false;
        }
    }

    private void OnMouseUpAsButton()
    {
        TriggerAnimation();
    }
}

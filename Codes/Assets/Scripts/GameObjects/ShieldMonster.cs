using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldMonster : Monster {
    public bool debug;
	
	// Update is called once per frame
	void Update () {
        MonsterMove();
        DestroyIfOutOfBounds(false);
        if(health == 0) {
            if (debug) Debug.Log("You killed a shield");
            GameManager.shielded = true;
            gameObject.SetActive(false);
        }
		
	}
}

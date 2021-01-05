using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplierMonster2 : Monster {

    // Use this for initialization
    public int multiplier;
	// Update is called once per frame
	void Update () {
        MonsterMove();
        DestroyIfOutOfBounds(false);
        if (health == 0) {
            GameManager.multiplierFromBlob += multiplier;
            DestroySelfAndGetPoints();
        }
    }
}

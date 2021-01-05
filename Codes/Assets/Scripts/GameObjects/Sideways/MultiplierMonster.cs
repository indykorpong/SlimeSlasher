using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplierMonster : GameItem {
    public int myMultiplier;

    private void Start() {
        gameObject.name = "Multiplier";
    }

    void Update () {
        ItemMove();
        //DestroyIfOutOfBounds();
        if (!isAlive) {
            Debug.Log("Multiplier up!");
            GameManager.multiplierFromBlob += myMultiplier;
            GameManager.UpdateScore();
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }
}

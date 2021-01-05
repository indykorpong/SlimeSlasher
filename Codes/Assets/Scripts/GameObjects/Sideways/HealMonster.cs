using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealMonster : GameItem {
    public int lifeHealed;
	// Use this for initialization
	void Start () {
        this.name = "Healblob";
	}

    // Update is called once per frame
    void Update() {
        ItemMove();
        //DestroyIfOutOfBounds();
        if (!isAlive) {
            Debug.Log("Welcome to heal!");
            if (GameManager.lives < GameManager.maxLives) {
                GameManager.lives += lifeHealed;
                if (GameManager.lives > GameManager.maxLives) GameManager.lives = GameManager.maxLives;
            }
            //GameManager.UpdateLives();
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }
}

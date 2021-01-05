using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealMonster2 : Monster {
    public int amountHealed;

	// Update is called once per frame
	void Update () {
        MonsterMove();
        DestroyIfOutOfBounds(false);
        if (health == 0) {
            Debug.Log("Evil vanquished.");
            GameManager.IncrementScore(points);
            if (GameManager.lives < GameManager.maxLives) {
                GameManager.lives += amountHealed;
                if (GameManager.lives > GameManager.maxLives) GameManager.lives = GameManager.maxLives;
            }
            //GameManager.UpdateLives();
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }
}

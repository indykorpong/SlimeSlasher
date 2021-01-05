using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeMonster2 : Monster {
    AudioSource explodeSound;

    // Update is called once per frame

    private void Start() {
        explodeSound = GameObject.Find("Bomb").GetComponent<AudioSource>();
    }
    void Update () {
        MonsterMove();
        DestroyIfOutOfBounds(false);
        if (health == 0) {
            Debug.Log("KABOOM");
            explodeSound.Play();
            GameManager.KillAllMonsters(true);
        }
    }
}

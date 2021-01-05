using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeMonster : GameItem {
    float sfxVolume;
    AudioSource explodeSound;
    // Use this for initialization
    void Start() {
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 1);
        explodeSound = GameObject.Find("Bomb").GetComponent<AudioSource>();
        explodeSound.volume = sfxVolume;
        gameObject.name = "Exploder";
    }

    // Update is called once per frame
    void Update() {
        ItemMove();
        //DestroyIfOutOfBounds();
        if (!isAlive) {
            Debug.Log("KABOOM");
            explodeSound.Play();
            GameManager.KillAllMonsters();
            //Destroy(gameObject);
        }
    }
}

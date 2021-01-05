using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeChangeMonster : GameItem {
    public bool goesToHell;
    public float sineFreq;
    public float sineAmp;
    public bool variedSin;
    public float freqVariance;
    public float ampVariance;
    // Use this for initialization
    AudioSource sparkle;
    private void Awake() {
        sparkle = GameObject.Find("Sparkle").GetComponent<AudioSource>();
    }

    void Start() {
        if (goesToHell) {
            this.name = "Hellblob";
        } else {
            this.name = "Heavenblob";
            
        }
    }

    new void OnEnable() {
        base.OnEnable();
        if(GameManager.hellcourse && !goesToHell) sparkle.Play();
        if (variedSin) {
            sineFreq = Random.Range(sineFreq - freqVariance, sineFreq + freqVariance);
            sineAmp = Random.Range(sineAmp - ampVariance, sineAmp + ampVariance);
        }
    }

    // Update is called once per frame
    void Update() {
        ItemMove();
        //DestroyIfOutOfBounds();
        if (!isAlive) {
            if (goesToHell) {
                Debug.Log("WELCOME HELL");
                GameManager.SetHell(true);
            } else {
                Debug.Log("GOODBYE HELL");
                GameManager.SetHell(false);
            }
            Instantiate(myExplosion, transform.position, Quaternion.identity);
            GameManager.KillAllMonsters();
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }

    public new void ItemMove() {
        //float speedMultiplier = Mathf.Pow(1.01f, GameManager.kills);
        //if (spawnPoint.x > 0) {
        //    transform.position += (velocity * Time.deltaTime * speedMultiplier);
        //} else {
        //    transform.position -= (velocity * Time.deltaTime * speedMultiplier);
        //}
        float speedMultiplier = Mathf.Pow(1.01f, GameManager.kills);
        velocity.x = Mathf.Sin(Time.time * sineFreq) * sineAmp;
        transform.position += (velocity * Time.deltaTime * speedMultiplier);
    }
}

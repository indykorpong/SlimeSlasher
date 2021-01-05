using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EraseHighscore : EventTrigger {

    public BestScore bestScore;
    public Coroutine eraser;
    private bool hasDeleted;

    private void Start() {
        hasDeleted = false;
        bestScore = FindObjectOfType<BestScore>();
    }

    public override void OnPointerDown(PointerEventData p) {
        if (!hasDeleted) {
            eraser = StartCoroutine(EraseCountDown());
            GameObject.Find("Click").GetComponent<AudioSource>().Play();
        }
    }

    public override void OnPointerUp(PointerEventData p) {
        if (!hasDeleted) {
            StopCoroutine(eraser);
            GetComponentInChildren<Text>().text = "Hold for 5 seconds to erase.";
        }
    }

    public IEnumerator EraseCountDown() {
        yield return new WaitForSeconds(1f);
        this.GetComponentInChildren<Text>().text = "Hold for 4 seconds to erase.";
        yield return new WaitForSeconds(1f);
        this.GetComponentInChildren<Text>().text = "Hold for 3 seconds to erase.";
        yield return new WaitForSeconds(1f);
        this.GetComponentInChildren<Text>().text = "Hold for 2 seconds to erase.";
        yield return new WaitForSeconds(1f);
        this.GetComponentInChildren<Text>().text = "Hold for 1 second to erase.";
        yield return new WaitForSeconds(1f);
        for(int i = 0; i < DataController.scoreCount; i++) {
            PlayerPrefs.DeleteKey("Score" + i);
        }
        this.GetComponent<Button>().interactable = false;
        this.GetComponentInChildren<Text>().text = "Highscore data deleted.";
        GameObject.Find("Bomb").GetComponent<AudioSource>().Play();
        hasDeleted = true;
        bestScore.UpdateScore();
        bestScore.SetScoreTextZero();
    }
}

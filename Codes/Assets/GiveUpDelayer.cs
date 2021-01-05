using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiveUpDelayer : MonoBehaviour {
    Button myButton;
    Text myText;
    public float delayInSeconds;
    // Use this for initialization
    private void Awake() {
        myButton = gameObject.GetComponent<Button>();
        myText = gameObject.GetComponentInChildren<Text>();
    }
    private void OnEnable() {
        StartCoroutine(DelayGiveup());
    }

    IEnumerator DelayGiveup() {
        myButton.interactable = false;
        myText.enabled = false;
        yield return new WaitForSeconds(delayInSeconds);
        myButton.interactable = true;
        myText.enabled = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour {

    public Text timerText;
    public Text instructionText;

    private void OnEnable() {
        Pause.IsGamePaused += Pause_IsGamePaused;
        GameManager.OnTapStateChange.AddListener(TapStateChange);
        GameManager.OnTapperClock.AddListener(TimerCountdown);
    }

    private void OnDisable() {
        GameManager.OnTapStateChange.RemoveListener(TapStateChange);
        GameManager.OnTapperClock.RemoveListener(TimerCountdown);
        Pause.IsGamePaused -= Pause_IsGamePaused;
    }

    private void TapStateChange(bool tapState) {
        timerText.enabled = tapState;
        instructionText.enabled = tapState;
    }

    private void TimerCountdown(float theTime) {
        if (theTime < 0) theTime = 0;
        string timeToString = string.Format("{0:0.00}", theTime);
        timerText.text = timeToString;
    }

    private void Pause_IsGamePaused(bool isPaused) {
        timerText.enabled = !isPaused && GameManager.tapEnabled;
        instructionText.enabled = !isPaused && GameManager.tapEnabled;
    }

}

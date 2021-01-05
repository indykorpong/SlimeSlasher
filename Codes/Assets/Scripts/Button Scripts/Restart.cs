using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Restart : MonoBehaviour {
    public static Pause pause;
    public Button restartButton;

    // Use this for initialization
    void Start()
    {
        pause = Pause.instance;
        restartButton.onClick.AddListener(ClickedRestart);

    }
    // Update is called once per frame
    private void Update()
    {
        //Debug.Log("From restart button, gamePaused = " + Pause.gamePaused);
        if(Pause.gamePaused){
            restartButton.interactable = true;
            restartButton.image.enabled = true;
        }else{
            restartButton.interactable = false;
            restartButton.image.enabled = false;
        }

    }
    public void ClickedRestart()
    {
        restartButton.interactable = false;
        restartButton.image.enabled = false;
        pause.pauseButton.gameObject.SetActive(true);
        pause.pauseButton.image.overrideSprite = pause.pauseImage;
        Pause.pauseScreen.SetActive(false);

        Time.timeScale = 1f;
        Pause.gamePaused = false;

        GameManager.KillAllMonsters();
        //GameManager.gameIsRunning = false;
        GameManager.PlayAgain();
    }
}

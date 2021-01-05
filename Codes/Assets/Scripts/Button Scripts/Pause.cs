using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Pause : MonoBehaviour {
    private static Pause _instance = null;


    public static bool gamePaused { get { return _gamePaused; } set { _gamePaused = value; IsGamePaused(value); } }
    private static bool _gamePaused;
    public delegate void gameIsPaused(bool isPaused);
    public static event gameIsPaused IsGamePaused;
    
    
    //private static Button pauseButton;
    public static GameObject pauseScreen;

    public Sprite pauseImage;
    public Sprite playImage;

    public Button pauseButton;

    private static bool gameRunning;
    public static Pause instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(Pause)) as Pause;
            }

            if (_instance == null)
            {
                GameObject obj = new GameObject("Pause");
                _instance = obj.AddComponent(typeof(Pause)) as Pause;
                Debug.Log("Created a Pause.");
            }
            return _instance;
        }
    }
	
    private void Start () {
        gamePaused = false;
        pauseScreen = GameObject.Find("PauseScreen");
        pauseScreen.SetActive(false);

        pauseButton.image.overrideSprite = pauseImage;
        pauseButton.onClick.AddListener(TogglePause);

        //Debug.Log("GamePaused value is " + gamePaused);
	}

    private void OnEnable()
    {
        GameManager.IsGameRunning += IsGameRunning;
    }

    private void OnDisable()
    {
        GameManager.IsGameRunning -= IsGameRunning;
    }

    private void IsGameRunning(bool gameRunning)
    {
        //gameRunning = GameManager.gameIsRunning;
        pauseButton.enabled = gameRunning;
        pauseButton.interactable = gameRunning;
        pauseButton.image.enabled = gameRunning;

        CheckIfGamePaused();
    }

    //private void Update()
    //{
    //    gameRunning = GameManager.gameIsRunning;
    //    pauseButton.enabled = gameRunning;
    //    pauseButton.interactable = gameRunning;
    //    pauseButton.image.enabled = gameRunning;

    //    CheckIfGamePaused();
    //}

    private void OnApplicationFocus(bool focus) {
        if (!focus && GameManager.gameIsRunning) SetPause(true);
    }

    /*private void OnApplicationPause(bool pause) {
        if (!pause) SetPause(true);
    }*/

    public void SetPause(bool pause) {
        if (pause) {
            gamePaused = true;
            pauseButton.image.overrideSprite = playImage;
            gameObject.SetActive(false);
        } else {
            gamePaused = false;
            pauseButton.image.overrideSprite = pauseImage;
            gameObject.SetActive(true);
        }
        pauseScreen.SetActive(gamePaused);
    }

    public void TogglePause() {
        //Debug.Log("You pressed the pause button!");
        //Debug.Log("GamePaused value is " + gamePaused);
        if (!gamePaused) {
            //Time.timeScale = 0f;
            SetPause(true);
        } else {
            /*
            if (GameManager.hellcourse) {
                Time.timeScale = 1.5f;
            } else {
                Time.timeScale = 1f;
            }
            */
            SetPause(false);
        } 
        
       // Debug.Log("GamePaused value is " + gamePaused);
    }

    private void CheckIfGamePaused(){
        //Debug.Log("GamePaused value is " + gamePaused);
        if (gamePaused) {
            Time.timeScale = 0f;
        } else {
            if (GameManager.hellcourse)
            {
                Time.timeScale = 1.5f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }
}

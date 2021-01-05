using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Home : MonoBehaviour {
    private Button homeButton;
    public string myScene;
    // Use this for initialization
    void Start()
    {
        homeButton = gameObject.GetComponent<Button>();
        homeButton.onClick.AddListener(ButtonClicked);
    }
    private void Update()
    {
        //if (Pause.gamePaused)
        //{
        //    homeButton.interactable = true;
        //    homeButton.image.enabled = true;
        //}
        //else
        //{
        //    homeButton.interactable = false;
        //    homeButton.image.enabled = false;
        //}
    }
    void ButtonClicked()
    {
        //Debug.Log(homeButton.GetComponentInChildren<Text>().text);
        GameManager.KillAllMonsters();
        //Pause.gamePaused = false;
        //homeButton.interactable = false;
        //homeButton.image.enabled = false;
        //GameManager.gameIsRunning = false;
        SceneManager.LoadScene(myScene);
    }
}

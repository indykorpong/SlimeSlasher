using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeHolderUI : MonoBehaviour
{
    /*
     * This Class is used to set game background
     * when game is over not life count
     */

    private DrawingManager dm;
    private Image myHolder;

    private void Awake()
    {
        //dm = FindObjectOfType<DrawingManager>();
        myHolder = gameObject.GetComponent<Image>();
    }

    private void OnEnable()
    {
        GameManager.CurrentGameState += SetBackground;
        GameManager.OnTapStateChange.AddListener(TapStateChange);
    }

    private void OnDisable()
    {
        GameManager.CurrentGameState -= SetBackground;
        GameManager.OnTapStateChange.RemoveListener(TapStateChange);
    }

    private void SetBackground(GameManager.GameState g)
    {
//        Debug.Log("Current game state: " + GameManager.gameIsRunning.ToString());
        Debug.Log("Current game state: " + (GameManager.GameState.gameStarted == g).ToString());
        myHolder.enabled = (GameManager.GameState.gameStarted == g) && GameManager.gameIsRunning;
    }
    private void TapStateChange(bool tapState) {
        Debug.Log("Current game state in tap: " + (!tapState).ToString());
        myHolder.enabled = !tapState;
    }


    // Update is called once per frame
    //void Update () {
    //       if (GameManager.gameState == GameManager.GameState.gameStarted) {
    //           myHolder.sprite = dm.lifePoint1;
    //       } else {
    //           myHolder.sprite = dm.lifePoint2;
    //       }
    //}
}

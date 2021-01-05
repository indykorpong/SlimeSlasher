using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour {
    private Button myButton;
    // Use this for initialization
    void Start() {
        myButton = gameObject.GetComponent<Button>();
        //myButton.onClick.AddListener(ButtonClicked);
    }
    void ButtonClicked() {
        GameManager.PlayAgain();
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {
    private Button myButton;
    public string myScene;
	// Use this for initialization
	void Start() {
        myButton = gameObject.GetComponent<Button>();
        myButton.onClick.AddListener(ButtonClicked);
    }
    void ButtonClicked() {
        //Debug.Log(myButton.GetComponentInChildren<Text>().text);
        SceneManager.LoadScene(myScene);
    }
}

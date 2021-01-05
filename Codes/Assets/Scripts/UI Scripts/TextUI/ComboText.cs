using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboText : MonoBehaviour {

    public Text textUI;
    public string preText = "";

    private void OnEnable() {
        GameManager.OnChainChange.AddListener(ComboTextUpdate);
    }

    private void OnDisable() {
        GameManager.OnChainChange.RemoveListener(ComboTextUpdate);
    }

    private void ComboTextUpdate(int chain) {
        textUI.text = preText + chain.ToString();
    }

}

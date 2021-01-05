using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldUI : MonoBehaviour {

    private Image myShield;

    private void Awake() {
        myShield = gameObject.GetComponent<Image>();
    }

    private void OnEnable()
    {
        GameManager.IsUserShielded += IsShielded;
        GameManager.OnTapStateChange.AddListener(TapStateChange);
    }

    private void OnDisable()
    {
        GameManager.IsUserShielded -= IsShielded;
        GameManager.OnTapStateChange.RemoveListener(TapStateChange);
    }

    private void IsShielded(bool isShield)
    {
        myShield.enabled = isShield;
    }
    private void TapStateChange(bool tapState) {
        myShield.enabled = !tapState && GameManager.shielded;
    }

}

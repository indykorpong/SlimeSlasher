using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeSelection : MonoBehaviour {

    public Text statusTextUI;

    public GameObject addNewGesture;
    public GameObject recognizeGesture;

    private Dictionary<int, GameObject> modeDict = new Dictionary<int, GameObject>();
    private void Awake()
    {
        modeDict.Add(1, addNewGesture);
        modeDict.Add(2, recognizeGesture);
    }

    public void SetModeTo(int modeNum)
    {
        GameObject currentMode = null;
        foreach (int mode in modeDict.Keys)
        {
            modeDict.TryGetValue(mode, out currentMode);
            if (currentMode != null) currentMode.SetActive(false);
        }

        modeDict.TryGetValue(modeNum, out currentMode);
        if (currentMode == null)
        {
            string msg = "No mode with value!";
            statusTextUI.text = msg;
            Debug.Log(msg);

            return;
        }

        currentMode.SetActive(true);
    }

}

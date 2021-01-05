using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecognizerScript : ClassifierScript
{
    public Text statusUI;
    public Toggle internalToggle;
    public Toggle externalToggle;

    private void Start()
    {
        LoadAllGestureAssets();
        internalToggle.isOn = loadInternalGesture;
        externalToggle.isOn = loadExternalGesture;
        currentClassifier = ClassifierClass.PointCloudPlus;
    }
}

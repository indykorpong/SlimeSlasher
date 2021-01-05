using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSpawner : MonoBehaviour
{

    public PoolingAddon poolGrid;
    public GameObject lineToSpawn;

    public PoolingAddon poolRecognizer;
    public GameObject recognizerSpawn;

    private void Awake()
    {
        poolGrid.prefab = lineToSpawn;
        poolRecognizer.prefab = recognizerSpawn;
    }

    public enum DetectionMode
    {
        Grid, Recognizer
    }
    public DetectionMode currentDetectionMethod;

    public ClassifierScript.ClassifierClass currentFamily = ClassifierScript.ClassifierClass.QPointCloud;
    public bool saveRawGesture = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && GameManager.gameIsRunning && !Pause.gamePaused && !GameManager.tapEnabled)
        {
            switch (currentDetectionMethod)
            {
                case DetectionMode.Grid:
                    poolGrid.RequestGameObject().SetActive(true);
                    break;
                case DetectionMode.Recognizer:
                    ClassifierScript.currentClassifier = currentFamily;
                    RecognizerSpawn.savePositions = saveRawGesture;
                    poolRecognizer.RequestGameObject().SetActive(true);

                    break;
                default:
                    throw new System.Exception("No such mode!");
            }
        }
    }
}

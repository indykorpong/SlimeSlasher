using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecognizerSpawn : MonoBehaviour
{

    // Detection System - Recognizer

    public LineRenderer lineRender;
    private List<Vector3> positionList = new List<Vector3>();

    public static bool savePositions = false;
    public static MonsterJudge mj;

    private void Awake()
    {
        if (mj == null) mj = FindObjectOfType<MonsterJudge>();
    }

    private void OnEnable()
    {
        Pause.IsGamePaused += DestroyOnPause;
    }

    private void OnDisable()
    {
        Pause.IsGamePaused -= DestroyOnPause;
    }

    private void Start()
    {
    }

    private void DestroyOnPause(bool isPaused)
    {
        if (isPaused)
        {
            gameObject.SetActive(false);
            ClearEverything();
        }
    }

    private Vector3 _tmp_mousePos;
    private Vector3 _tmp_worldPos;
    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            _tmp_mousePos = Input.mousePosition;
            _tmp_mousePos.z = 1.0f;
            _tmp_worldPos = Camera.main.ScreenToWorldPoint(_tmp_mousePos);

            positionList.Add(_tmp_worldPos);
            lineRender.positionCount = positionList.Count;
            lineRender.SetPosition(positionList.Count - 1, _tmp_worldPos);
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && !Pause.gamePaused)
        {
            if (savePositions) SavePositions.SaveRawGesture(positionList);
            mj.RecognizerJudge(positionList);
            ClearEverything();
            gameObject.SetActive(false);
        }
    }

    private void ClearEverything()
    {
        positionList.Clear();
        lineRender.positionCount = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddGestureScript : MonoBehaviour
{

    public Text statusText;

    public InputField classNameInput;
    public InputField fileNameInput;

    public LineRenderer gestureRenderer;
    public List<Vector3> gesturePoints = new List<Vector3>();

    public Collider2D startArea;

    void Start()
    {

    }

    private bool isValidStart = false;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isValidStart = false;

            RaycastHit2D[] raycastHit2D = null;
            raycastHit2D = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            foreach (RaycastHit2D ray2D in raycastHit2D)
            {
                if (ray2D == startArea)
                {
                    isValidStart = true;
                    break;
                }
            }

            if (isValidStart)
            {
                ClearScreen();
            }
        }

        if (Input.GetMouseButton(0) && isValidStart)
        {
            Vector3 _point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _point.z = 0f;

            gesturePoints.Add(_point);

            gestureRenderer.positionCount = gesturePoints.Count;
            gestureRenderer.SetPosition(gesturePoints.Count - 1, _point);
        }
    }

    public void ClearScreen()
    {
        gestureRenderer.positionCount = 0;
        gesturePoints.Clear();
    }

    [Tooltip("Stimulate saving and NOT actually saves")]
    public bool drySave = true;
    public void AddGesture()
    {
        string msg = "";
        if (classNameInput.text.Length == 0 && currentSelection.Length == 0)
        {
            msg += "No class name provided! ";
        }
        if (fileNameInput.text.Length == 0)
        {
            msg += "No file name provided! ";
        }
        if (msg.Length > 0)
        {
            statusText.text = msg;
            Debug.Log(msg);
            return;
        }

        //string className = classNameInput.text;
        string className = currentSelection;
        string fileName = fileNameInput.text;

        Point[] pointArr = ClassifierScript.Vector3ListToPointArray(gesturePoints);
        Gesture newGest = new Gesture(pointArr, className);

        string pathSaved = ClassifierScript.SaveGestures(newGest, fileName, drySave);

        statusText.text = string.Format("Class: {0}; Filename: {1};", className, fileName);
        Debug.Log("saved to: " + pathSaved);
    }
    public string currentSelection;
    public void OnDropdownClassChanges(int option)
    {
        currentSelection = ((DrawingManager.LineType)option).ToString();
    }

    public void TryRecognize()
    {
        PassStuff ps = ClassifierScript.ClassifyScore(gesturePoints);
        if (ps == null)
        {
            string msg = "Does not detect anything";
            Debug.Log(msg);
            statusText.text = msg;
        }
        else
        {
            string msg = "Detected: " + ps.templateName;
            Debug.Log(msg);
            statusText.text = msg;
        }
    }

}

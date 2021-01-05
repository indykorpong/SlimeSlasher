using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecognizeGestureScript : MonoBehaviour
{

    public Text statusText;

    public LineRenderer gestureRenderer;
    public List<Vector3> gesturePoints = new List<Vector3>();

    public Collider2D startArea;

    private void Start()
    {
        distanceUI.text = ClassifierScript.maxDistance.ToString();
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

        if (Input.GetMouseButtonUp(0))
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

    public void ClearScreen()
    {
        gestureRenderer.positionCount = 0;
        gesturePoints.Clear();
    }

    public Text distanceUI;
    public void AddValueBy(float val)
    {
        ClassifierScript.maxDistance += val;
        if (ClassifierScript.maxDistance < 0) ClassifierScript.maxDistance = 0;
        distanceUI.text = ClassifierScript.maxDistance.ToString();
    }
}

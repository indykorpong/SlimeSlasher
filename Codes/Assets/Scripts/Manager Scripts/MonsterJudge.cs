using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Recognize line type of the current line
public class MonsterJudge : MonoBehaviour
{


    private static SoundManager soundManager;
    //private static DrawingManager drawingManager;
    public GameObject grid;
    public bool showDebugMsg = false;


    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    private List<Vector3> PositiveQ(List<Vector3> rawVector)
    {
        List<Vector3> positiveVec = new List<Vector3>();
        float maxX = float.MinValue;
        float maxY = float.MinValue;
        float minX = float.MaxValue;
        float minY = float.MaxValue;
        foreach (Vector3 vec in rawVector)
        {
            if (maxX < vec.x) maxX = vec.x;
            if (maxY < vec.y) maxY = vec.y;
            if (minX > vec.x) minX = vec.x;
            if (minY > vec.y) minY = vec.y;
        }

        foreach (Vector3 vec in rawVector)
        {
            positiveVec.Add(new Vector3(minX + vec.x, minY + vec.y, 0f));
        }

        return positiveVec;
    }

    public bool makePositive = false;
    private PassStuff ps;
    public void RecognizerJudge(List<Vector3> vecList)
    {
        // Shift all points to positive quadrant
        if (makePositive) vecList = PositiveQ(vecList);

        ps = ClassifierScript.ClassifyScore(vecList);

        if (ps == null)
        {
            if (showDebugMsg) Debug.Log("Recognizer does not detect");
            CheckMonsters2(DrawingManager.LineType.nothing.ToString());
        }
        else
        {
            if (showDebugMsg) Debug.Log("Recognizer detected: " + ps.templateName);
            CheckMonsters2(ps.templateName);
        }
    }

    public IEnumerator GridJudge(Vector3 minPoint, Vector3 maxPoint, List<Vector3> VectorList)
    {

        GameObject tempGrid = Instantiate(grid);
        //change size of grid according to line size
        //Debug.Log("maxPoint = " + maxPoint + " , minPoint = " + minPoint);
        Vector3 imageScale = (Camera.main.ScreenToWorldPoint(maxPoint) - Camera.main.ScreenToWorldPoint(minPoint)) / 4;
        //Debug.Log(imageScale);
        if (imageScale.x < 0.25) imageScale.x = 0.25f;
        if (imageScale.y < 0.25) imageScale.y = 0.25f;
        //imageScale = new Vector3(2, 2, 1);
        tempGrid.transform.localScale = imageScale;

        //change position of grid according to starting point
        Vector3 reposition = Camera.main.ScreenToWorldPoint(new Vector3(minPoint.x, minPoint.y, 1.0f));
        reposition.x += imageScale.x * 2;
        reposition.y += imageScale.y * 2;
        reposition.z = 1.0f;
        tempGrid.transform.localPosition = reposition;

        //compare VectorList and see what squares in the detector grid the vectors go through
        List<string> hitlist = new List<string>();
        string tempHit = "nothing";
        foreach (Vector3 point in VectorList)
        {
            foreach (Transform t in tempGrid.GetComponentInChildren<Transform>())
            {
                Rect r = new Rect(t.position.x, t.position.y, imageScale.x, imageScale.y)
                {
                    center = new Vector2(t.position.x, t.position.y)
                }; //I need to move position (which starts at the center of a shape) to the top left (where Rect starts)
                if (r.Contains(point) && !t.name.Equals(tempHit))
                {
                    hitlist.Add(t.name);
                    tempHit = t.name;
                    break;
                }
            }
        }

        //return the result as string
        string result = "";
        foreach (string s in hitlist)
        {
            result += s;
        }
        //Debug.Log(result);
        CheckMonsters(result);

        //imageOfGrid.enabled = true;
        //yield return new WaitForSeconds(0.1f);
        
        hitlist.Clear();
        Destroy(tempGrid);
        yield return null;
    }

    private void Start()
    {
        soundManager.incorrectSound = GameObject.Find("Incorrect").GetComponent<AudioSource>();
        //dm = FindObjectOfType<DrawingManager>();
    }

    public void CheckMonsters2(string lineName)
    {
        bool monsterGotDamaged = false;
        bool monsterDied = false;
        int tempScore = GameManager.score;

        Monster[] MonsterObjectList = FindObjectsOfType<Monster>();
        foreach (Monster m in MonsterObjectList)
        {
            if (lineName == (m.myLine).ToString())
            {
               
                m.health--;
                monsterGotDamaged = true;
                DrawingManager.LineType oldLine = m.myLine;
                if (m.health > 0)
                {
                    while (oldLine == m.myLine)
                    {
                        m.SetMyLine(DrawingManager.RandomLineType(), false);
                    }
                }else if(m.health <= 0) {
                    monsterDied = true;
                }
            }
        }
        if (monsterGotDamaged) soundManager.RandomHurtSound().Play();
        if (monsterDied) {
            int intensity;
            int pointsGained = GameManager.score - tempScore;
            if(pointsGained >= 25) {
                intensity = 2;
            }else if(pointsGained >= 10) {
                intensity = 1;
            } else {
                intensity = 0;
            }
            soundManager.RandomDeathSound(intensity).Play();
        }
        if (!monsterGotDamaged && !GameManager.hitModeChangeBlob)
        {
            //Debug.Log("You draw it wrong!");
            soundManager.incorrectSound.Play();
            GameManager.hitModeChangeBlob = false;
        }
        else if (GameManager.hitModeChangeBlob)
        {
            GameManager.hitModeChangeBlob = false;
        }
    }

    public void CheckMonsters(string lineCoords)
    {
        bool monsterGotDamaged = false;
        //Debug.Log("The closest shape: " + closest + ", " + lineCoords);
        //GameObject[] MonsterObjectList = GameObject.FindGameObjectsWithTag("Monsters");

        DrawingManager.LineType closest = DrawingManager.ClosestLineType(lineCoords);

        DrawingManager.LineType detectedType = DrawingManager.LineType.nothing;
        bool detectedALineType = false;

        Monster[] MonsterObjectList = FindObjectsOfType<Monster>();
        foreach (Monster m in MonsterObjectList)
        {
            if (closest == m.myLine)
            {
                detectedType = closest;
                detectedALineType = true;
                soundManager.RandomDeathSound().Play();
                m.health--;
                monsterGotDamaged = true;
                DrawingManager.LineType oldLine = m.myLine;
                if (m.health > 0)
                {
                    while (oldLine == m.myLine)
                    {
                        m.SetMyLine(DrawingManager.RandomLineType(), false);
                    }
                }
            }
        }

        if (detectedALineType)
        {
            Debug.Log("Grid detected: " + detectedType.ToString());
        }
        else
        {
            Debug.Log("Grid does not detect");
        }

        if (!monsterGotDamaged && !GameManager.hitModeChangeBlob)
        {
            //Debug.Log("You draw it wrong!");
            soundManager.incorrectSound.Play();
            GameManager.hitModeChangeBlob = false;
        }
        else if (GameManager.hitModeChangeBlob)
        {
            GameManager.hitModeChangeBlob = false;
        }
    }

}

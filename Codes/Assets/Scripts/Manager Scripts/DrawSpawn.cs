using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Create a line with a shape detector grid
public class DrawSpawn : MonoBehaviour
{
    // 5x5 grid without space draw detector
    public LineRenderer lineRender;

    //public Texture tex;
    public GameObject grid;
    //private SpriteRenderer imageOfGrid;
    private Vector3 minPoint;
    private Vector3 maxPoint;
    private Vector3 imageScale;
    private List<Vector3> VectorList = new List<Vector3>();

    public static MonsterJudge mj;

    private void Awake()
    {
        if (mj == null) mj = FindObjectOfType<MonsterJudge>();
    }

    private void OnEnable()
    {
        minPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f);
        maxPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f);
        Pause.IsGamePaused += ActiveFalseOnPause;
    }

    private void OnDisable()
    {
        Pause.IsGamePaused -= ActiveFalseOnPause;
    }

    private void ActiveFalseOnPause(bool isPaused)
    {
        if (isPaused){
            gameObject.SetActive(false);
            ClearEverything();
        }
    }

    private IEnumerator DrawGridRef;
    private Vector3 _tmp_worldPos;
    private Vector3 _tmp_mousePos;
    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {

            if (Input.mousePosition.x > maxPoint.x) maxPoint.x = Input.mousePosition.x;
            if (Input.mousePosition.y > maxPoint.y) maxPoint.y = Input.mousePosition.y;
            if (Input.mousePosition.x < minPoint.x) minPoint.x = Input.mousePosition.x;
            if (Input.mousePosition.y < minPoint.y) minPoint.y = Input.mousePosition.y;

            _tmp_mousePos = Input.mousePosition;
            _tmp_mousePos.z = 1.0f;

            _tmp_worldPos = Camera.main.ScreenToWorldPoint(_tmp_mousePos);
            VectorList.Add(_tmp_worldPos);

            lineRender.positionCount = VectorList.Count;
            lineRender.SetPosition(VectorList.Count - 1, _tmp_worldPos);
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && !Pause.gamePaused)
        {
            DrawGridRef = DrawGrid2();
            StartCoroutine(DrawGridRef);
        }
    }

    private IEnumerator DrawGrid2()
    {
        yield return null;
        // Do calculate after the frame is updated

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
        Debug.Log(result);

        //imageOfGrid.enabled = true;
        hitlist.Clear();
        //yield return new WaitForSeconds(0.1f);

        mj.CheckMonsters(result);

        //yield return new WaitForSeconds(0.5f);
        Destroy(tempGrid);

        //imageOfGrid.enabled = false;
        lineRender.positionCount = 0;
        VectorList.Clear();

        gameObject.SetActive(false);
    }

    private void ClearEverything()
    {
        VectorList.Clear();
        lineRender.positionCount = 0;
    }
}

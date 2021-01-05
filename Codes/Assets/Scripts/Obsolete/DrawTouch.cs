using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTouch : MonoBehaviour {
	// 5x5 grid without space draw detector
	public LineRenderer lineRender;
	public Texture tex;
	private int numberOfPoints = 0;
	private GameObject grid;
	//private SpriteRenderer imageOfGrid;
	private Vector2 minPoint;
	private Vector2 maxPoint;
	private Vector2 imageScale;
	private List<Vector2> VectorList;
	// Use this for initialization
	void Start() {
		grid = GameObject.Find("DetectorGrid");
		//imageOfGrid = grid.GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update() {
		Touch touch = Input.GetTouch (0);
		if (touch.phase == TouchPhase.Began) {
			minPoint = new Vector2 (touch.position.x, touch.position.y);
			maxPoint = new Vector2 (touch.position.x, touch.position.y);
			VectorList = new List<Vector2> ();
		} else if (touch.phase == TouchPhase.Moved) {
			numberOfPoints++;
			lineRender.positionCount = numberOfPoints;
			Vector2 touchPos = new Vector2 (0, 0);
			if (touch.position.x > maxPoint.x)
				maxPoint.x = touch.position.x;
			if (touch.position.y > maxPoint.y)
				maxPoint.y = touch.position.y;
			if (touch.position.x < minPoint.x)
				minPoint.x = touch.position.x;
			if (touch.position.y < minPoint.y)
				minPoint.y = touch.position.y;
			touchPos = touch.position;
			Vector2 worldPos = Camera.main.ScreenToWorldPoint (touchPos);
			VectorList.Add (worldPos);
			lineRender.SetPosition (numberOfPoints - 1, worldPos);
		} else if (touch.phase == TouchPhase.Ended) {
			StartCoroutine (DrawGrid ());
		}
	}
	/*private void OnGUI() {
        GUI.color = Color.green;
        foreach (Transform t in grid.GetComponentInChildren<Transform>()) {
            Rect r = new Rect(t.position.x, t.position.y, 0.5f * imageScale.x, 0.5f * imageScale.y);
            r.y = Screen.height - r.y;
            GUI.DrawTexture(r, tex);
        }
    }*/
	IEnumerator DrawGrid() {
		//change size of grid according to line size
		imageScale = (maxPoint - minPoint) / 540;
		//Debug.Log(imageScale);
		if (imageScale.x < 0.25) imageScale.x = 0.25f;
		if (imageScale.y < 0.25) imageScale.y = 0.25f;
		//imageScale = new Vector3(2, 2, 1);
		grid.transform.localScale = imageScale;

		//change position of grid according to starting point
		Vector2 reposition = Camera.main.ScreenToWorldPoint(new Vector2(minPoint.x, minPoint.y));
		reposition.x += imageScale.x * 2;
		reposition.y += imageScale.y * 2;
		grid.transform.localPosition = reposition;

		//compare VectorList and see what squares in the detector grid the vectors go through
		List<string> hitlist = new List<string>();
		string tempHit = "nothing";
		foreach (Vector2 point in VectorList) {
			foreach (Transform t in grid.GetComponentInChildren<Transform>()) {
				Rect r = new Rect(t.position.x, t.position.y, imageScale.x, imageScale.y); //I need to move position (which starts at the center of a shape) to the top left (where Rect starts)
				r.center = new Vector2(t.position.x, t.position.y);
				if (r.Contains(point) && !t.name.Equals(tempHit)) {
					hitlist.Add(t.name);
					tempHit = t.name;
					break;
				}
			}
		}

		//return the result as string
		string result = "";
		foreach (string s in hitlist) {
			result += s;
		}
		Debug.Log(result);
		//DrawJudge.CheckLine(result);


		//imageOfGrid.enabled = true;
		yield return new WaitForSeconds(1);
		//imageOfGrid.enabled = false;

		numberOfPoints = 0;
		lineRender.positionCount = 0;
		VectorList.Clear();
		hitlist.Clear();
		yield return null;
	}
}

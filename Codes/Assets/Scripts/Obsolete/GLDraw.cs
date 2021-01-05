using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLDraw : MonoBehaviour {
    public Color baseColor;
    public Material material;
    public Transform origin;

    public Vector3[] points;
    public Color[] colors;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnDrawGizmos() {
        RenderLines(points, colors);
    }
    void OnPostRender() {
        RenderLines(points, colors);
    }

    void RenderLines(Vector3[] points, Color[] colors) {
        if (!ValidateInput(points, colors)) return;
        GL.Begin(GL.LINES);
        material.SetPass(0);
        for (int i = 0; i<points.Length; i++) {
            GL.Color(baseColor);
            GL.Vertex(origin.position);
            GL.Color(colors[i]);
            GL.Vertex(points[i]);
        }


    }

    private bool ValidateInput(Vector3[] points, Color[] colors) {
        return points.Length == colors.Length;
    }
}

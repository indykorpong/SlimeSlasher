using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GestureJSON : MonoBehaviour
{
    [System.Serializable]
    public class GestureJsonString
    {
        public string gestureClass;

        public float[] X;
        public float[] Y;
        public int[] StrokeID;
        public int[] intX;
        public int[] intY;

        public int pointLength;

        public int[] LUTdimension;
        public int[] LUTflat;
        //public int[][] LUT;
    }


    public static int[][] ArrayUnroll(int[][] arr)
    {
        int xLength = arr.Length;
        int yLength = arr[0].Length;
        int[] dimension = { xLength, yLength };

        List<int> values = new List<int>();
        for (int x = 0; x < xLength; x++)
        {
            for (int y = 0; y < yLength; y++)
            {
                values.Add(arr[x][y]);
            }
        }

        return new int[][] { dimension, values.ToArray() };
    }

    public static int[][] ArrayRollup(int[] dimensions, int[] flatArray)
    {
        int xLength = dimensions[0];
        int yLength = dimensions[1];

        int[][] values = new int[xLength][];
        for (int i = 0; i < xLength; i++)
            values[i] = new int[yLength];

        int xCounter = 0;
        int yCounter = 0;
        foreach (int val in flatArray)
        {
            int xPos = xCounter % xLength;
            int yPos = yCounter % yLength;

            values[xPos][yPos] = val;

            yCounter++;
            if (yCounter % yLength == 0)
            {
                xCounter++;
            }
        }

        return values;
    }


    public static string ToJsonString(Gesture g)
    {
        int[][] values = ArrayUnroll(g.LUT);
        GestureJsonString gestString = new GestureJsonString
        {
            X = new float[g.Points.Length],
            Y = new float[g.Points.Length],
            StrokeID = new int[g.Points.Length],
            intX = new int[g.Points.Length],
            intY = new int[g.Points.Length],

            gestureClass = g.gestureClass,
            pointLength = g.Points.Length,

            //LUT = g.LUT
            LUTdimension = values[0],
            LUTflat = values[1]
        };

        for (int i = 0; i < g.Points.Length; i++)
        {
            gestString.X[i] = g.Points[i].X;
            gestString.Y[i] = g.Points[i].Y;
            gestString.StrokeID[i] = g.Points[i].StrokeID;
            gestString.intX[i] = g.Points[i].intX;
            gestString.intY[i] = g.Points[i].intY;
        }

        string jstring = JsonUtility.ToJson(gestString);

        return jstring;
    }
    
    public static Gesture LoadGestureJson(string strJson)
    {
        GestureJsonString gj = JsonUtility.FromJson<GestureJsonString>(strJson);

        Point[] pointArr = new Point[gj.pointLength];
        for (int i = 0; i < pointArr.Length; i++)
        {
            Point p = new Point(0, 0, 0)
            {
                X = gj.X[i],
                Y = gj.Y[i],
                StrokeID = gj.StrokeID[i],
                intX = gj.intX[i],
                intY = gj.intY[i]
            };

            pointArr[i] = p;
        }

        Gesture loadedGesture = new Gesture();
        loadedGesture.gestureClass = gj.gestureClass;
        loadedGesture.Points = pointArr;
        loadedGesture.LUT = ArrayRollup(gj.LUTdimension, gj.LUTflat);
        //loadedGesture.LUT = gj.LUT;
        
        return loadedGesture;
    }
}


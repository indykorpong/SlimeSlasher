using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SavePositions : MonoBehaviour
{

    private static readonly string folderName = "rawGestures";

    [System.Serializable]
    public class RawGesture
    {
        public float[] X;
        public float[] Y;

        public int pointLength;
    }

    public static void SaveRawGesture(List<Vector3> vecList)
    {
        int pointCount = vecList.Count;

        List<float> xList = new List<float>();
        List<float> yList = new List<float>();

        foreach (Vector3 vec in vecList)
        {
            xList.Add(vec.x);
            yList.Add(vec.y);
        }

        RawGesture rg = new RawGesture();
        rg.X = xList.ToArray();
        rg.Y = yList.ToArray();
        rg.pointLength = pointCount;

        Guid guid = Guid.NewGuid();

        string gestureFolder = Path.Combine(Application.persistentDataPath, folderName);
        string txtFile = string.Format("{0}.json", guid);
        string gestureFileLoc = Path.Combine(gestureFolder, txtFile);

        if (!Directory.Exists(gestureFolder))
        {
            Directory.CreateDirectory(gestureFolder);
        }

        string gestureString = JsonUtility.ToJson(rg);
        File.WriteAllText(gestureFileLoc, gestureString);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ClassifierScript : MonoBehaviour
{
    //public Text statusUI;

    //public Toggle internalToggle;
    //public Toggle externelToggle;

    public static Gesture[] gestureSet; // For performance reason
    //private void Awake()
    //{
    //    if (cs == null) cs = this;
    //    else Destroy(this);
    //}
    //private void Start()
    //{
    //    LoadAllGestureAssets();
    //    //internalToggle.isOn = loadInternalGesture;
    //    //externelToggle.isOn = loadExternalGesture;
    //}

    public static bool showDebugMsg = false;

    public static bool isGestureLoaded = false;

    public static float maxDistance = 10;
    public static int minPoint = 5;

    private static List<Gesture> gestureInternalList; // Used internally 
    public bool loadInternalGesture = true;
    public void SetLoadInternalGestures(bool shouldLoad)
    {
        loadInternalGesture = shouldLoad;
    }
    public TextAsset[] gestureTextAssets;
    public void LoadInternalGestureAssets()
    {
        gestureInternalList = new List<Gesture>();

        if (!loadInternalGesture)
        {
            Debug.Log("Does not load internal gesture due to setting");
            return;
        }

        Gesture g = null;
        foreach (TextAsset textAsset in gestureTextAssets)
        {
            if (textAsset == null) continue;
            //  Debug.Log("Trying to load: " + textAsset.name);
            try
            {
                g = GestureJSON.LoadGestureJson(textAsset.text);
            }
            catch (System.Exception e)
            {
                Debug.Log("Failed to load: " + textAsset.name + "; reason: " + e.Message);
            }
            if (g == null) continue;
            gestureInternalList.Add(g);
        }

        Debug.Log(string.Format("Loaded {0} of {1} internal gestures", gestureInternalList.Count, gestureTextAssets.Length));
    }

    private static List<Gesture> gestureExternalList; // Custom added gestures
    public bool loadExternalGesture = false;
    public void SetLoadExternalGestures(bool shouldLoad)
    {
        loadExternalGesture = shouldLoad;
    }
    private static readonly string gestureFolderName = "Gestures";
    public void LoadExternalGestureAssets()
    {
        gestureExternalList = new List<Gesture>();

        if (!loadExternalGesture)
        {
            Debug.Log("Does not load External gesture due to setting");
            return;
        }

        string gestureFolder = Path.Combine(Application.persistentDataPath, gestureFolderName);

        if (!Directory.Exists(gestureFolder))
        {
            //if it doesn't, create it
            Directory.CreateDirectory(gestureFolder);
        }

        Gesture externalGest = null;
        DirectoryInfo info = new DirectoryInfo(gestureFolder);
        FileInfo[] fileInfo = info.GetFiles();

        foreach (FileInfo file in fileInfo)
        {
            string gestString = null;
            try
            {
                gestString = File.ReadAllText(Path.Combine(gestureFolder, file.Name));
                externalGest = GestureJSON.LoadGestureJson(gestString);
            }
            catch (System.Exception e)
            {
                Debug.Log("Failed to load: " + file.Name + "; reason: " + e.Message);
            }
            if (externalGest == null) continue;
            gestureExternalList.Add(externalGest);
        }

        Debug.Log(string.Format("Loaded {0} of {1} from external files", gestureExternalList.Count, fileInfo.Length));
    }

    public void LoadAllGestureAssets()
    {
        LoadInternalGestureAssets();
        LoadExternalGestureAssets();

        gestureSet = new Gesture[gestureInternalList.Count + gestureExternalList.Count];

        int counter = 0;
        foreach (Gesture gg in gestureInternalList)
        {
            gestureSet[counter] = gg;
            counter++;
        }
        foreach (Gesture gg in gestureExternalList)
        {
            gestureSet[counter] = gg;
            counter++;
        }

        string dbMessage = string.Format("Result - EX: {0}; IN: {1}", gestureExternalList.Count, gestureInternalList.Count);
        if (showDebugMsg) Debug.Log(dbMessage);

        //statusUI.text = dbMessage;
        isGestureLoaded = true;
    }

    public void RemoveAllGestures()
    {
        gestureSet = new Gesture[0];
    }

    public static string SaveGestures(Gesture ng, string fileName, bool drySave = false)
    {
        string gestureFolder = Path.Combine(Application.persistentDataPath, gestureFolderName);
        string txtFile = string.Format("{0}.txt", fileName);
        string gestureFileLoc = Path.Combine(gestureFolder, txtFile);

        if (!Directory.Exists(gestureFolder))
        {
            Directory.CreateDirectory(gestureFolder);
            if (showDebugMsg) Debug.Log("Directory not found, making one...");
        }

        string gestureString = GestureJSON.ToJsonString(ng);

        if (drySave)
        {
            if (showDebugMsg) Debug.Log("Gesture! (Stimulate Only)");
        }
        else
        {
            File.WriteAllText(gestureFileLoc, gestureString);
            if (showDebugMsg) Debug.Log("Saved gesture!");
        }

        return gestureFileLoc;
    }

    public enum ClassifierClass
    {
        PointCloud,
        PointCloudPlus,
        QPointCloud
    }
    public static ClassifierClass currentClassifier;
    public void SetClassiferClass(ClassifierClass c)
    {
        currentClassifier = c;
    }

    private static PassStuff ClassifyScore(Gesture candidate, float maxDistance)
    {
        PassStuff ps = null;
        switch (currentClassifier)
        {
            case ClassifierClass.PointCloud:
                if (showDebugMsg) Debug.Log("Point cloud");
                ps = PointCloudRecognizer.ClassifyScore(candidate, gestureSet);
                break;
            case ClassifierClass.PointCloudPlus:
                if (showDebugMsg) Debug.Log("Point cloud PLUS");
                ps = PointCloudRecognizerPlus.ClassifyScore(candidate, gestureSet);
                break;
            case ClassifierClass.QPointCloud:
                if (showDebugMsg) Debug.Log("Quick cloud");
                ps = QPointCloudRecognizer.ClassifyScore(candidate, gestureSet);
                break;
            default:
                throw new System.Exception("No such class!");
        }

        if (ps.score > maxDistance) return null;

        return ps;
    }

    public static bool preProcess = true;
    public static PassStuff ClassifyScore(List<Vector3> inputPoints)
    {
        if (!isGestureLoaded)
        {
            if (showDebugMsg) Debug.Log("Nothing loaded, exiting method");
            return null;
        }
        if (gestureSet.Length < 1)
        {
            if (showDebugMsg) Debug.Log("No gesture templates!, exiting method");
            return null;
        }
        if (minPoint > inputPoints.Count)
        {
            if (showDebugMsg) Debug.Log("Not enough points! exiting method");
            return null;
        }

        List<Vector3> positionList = new List<Vector3>();

        if (preProcess)
        {
            positionList = Preprocess_FilterConsecutiveDuplicates(inputPoints);
        }
        else
        {
            positionList = inputPoints;
        }

        //List<Vector3> validPositions = new List<Vector3>();
        //int lastValid = 0;
        //validPositions.Add(inputPoints[lastValid]);
        //for (int i = 1; i < inputPoints.Count; i++)
        //{
        //    // Point filtering
        //    // Remove duplicate points
        //    // WARN: May make points count too low
        //    if (inputPoints[lastValid].x == inputPoints[i].x && inputPoints[lastValid].y == inputPoints[i].y) continue;
        //    lastValid = i;

        //    validPositions.Add(inputPoints[lastValid]);
        //}

        //string msg = "[";

        //List<Point> pointPosition = new List<Point>();
        //int counter = 0;
        //foreach (Vector3 tmp in validPositions)
        ////foreach (Vector3 tmp in inputPoints)
        //{
        //    msg += string.Format("({0}, {1})", tmp.x, tmp.y);
        //    pointPosition.Add(new Point(tmp.x, tmp.y, 0));
        //    counter++;
        //    msg += ", ";
        //}
        //msg += "]; Point count: " + pointPosition.Count.ToString();

        Point[] pointPosition = Vector3ListToPointArray(positionList);
        Gesture g = null;
        try
        {
            g = new Gesture(pointPosition);
        }
        catch (DistanceTooSmallException e)
        {
            if (!showDebugMsg) Debug.Log(e.Message);
        }

        if (g == null)
        {
            if (!showDebugMsg) Debug.Log("Does not detect");
            return null;
        }

        return ClassifyScore(g, maxDistance);
    }

    public static Point[] Vector3ListToPointArray(List<Vector3> vec3List)
    {

        List<Point> pointPosition = new List<Point>();
        foreach (Vector3 tmp in vec3List)
        {
            pointPosition.Add(new Point(tmp.x, tmp.y, 0));
        }
        return pointPosition.ToArray();
    }

    public static List<Vector3> Preprocess_FilterConsecutiveDuplicates(List<Vector3> vec3List)
    {

        // Point filtering
        // Remove duplicate points
        // WARN: May make points count too low

        List<Vector3> validPositions = new List<Vector3>();
        int lastValid = 0;

        validPositions.Add(vec3List[lastValid]);
        for (int i = 1; i < vec3List.Count; i++)
        {
            if (vec3List[lastValid].x == vec3List[i].x && vec3List[lastValid].y == vec3List[i].y) continue;
            lastValid = i;

            validPositions.Add(vec3List[lastValid]);
        }

        return validPositions;
    }




    //private void LoadAllGestureAssets_OLD()
    //{
    //    gestureInternalList = new List<Gesture>();
    //    gestureSet = null;

    //    int successCounter = 0;
    //    int failCounter = 0;
    //    Gesture g = null;
    //    foreach (TextAsset textAsset in gestureTextAssets)
    //    {
    //        //  Debug.Log("Trying to load: " + textAsset.name);
    //        try
    //        {
    //            g = GestureJSON.LoadGestureJson(textAsset.text);
    //            successCounter++;
    //        }
    //        catch (System.Exception e)
    //        {
    //            Debug.Log("Failed to load: " + textAsset.name + "; reason: " + e.Message);
    //            failCounter++;
    //        }
    //        if (g == null) continue;
    //        gestureInternalList.Add(g);
    //    }

    //    Debug.Log(string.Format("Loaded {0} of {1} gestures", gestureInternalList.Count, gestureTextAssets.Length));
    //    gestureSet = gestureInternalList.ToArray();
    //    // Debug.Log(string.Format("To array of {0}", gestureSet.Length));

    //    isGestureLoaded = true;
    //}

    //private void LoadAllGestures()
    //{
    //    string gestureFolder = Path.Combine(Application.persistentDataPath, gestureFolderName);

    //    DirectoryInfo info = new DirectoryInfo(gestureFolder);
    //    FileInfo[] fileInfo = info.GetFiles();

    //    int totalGestures = fileInfo.Length;

    //    int successCounter = 0;
    //    int failCounter = 0;
    //    Gesture g = null;
    //    foreach (FileInfo file in fileInfo)
    //    {
    //        Debug.Log("Trying to load: " + file.Name);
    //        string filePath = Path.Combine(gestureFolder, file.Name);
    //        try
    //        {
    //            string x = File.ReadAllText(filePath);
    //            g = GestureJSON.LoadGestureJson(x);
    //            successCounter++;
    //        }
    //        catch (System.Exception e)
    //        {
    //            Debug.Log("Failed to load: " + file.Name + "; reason: " + e.Message);
    //            failCounter++;
    //        }
    //        if (g == null) continue;
    //        gestureList.Add(g);
    //    }
    //    //Debug.Log(string.Format("Loaded {0} of {1} gestures", gestureList.Count, totalGestures));
    //    gestureSet = gestureList.ToArray();
    //    // Debug.Log(string.Format("To array of {0}", gestureSet.Length));

    //    isGestureLoaded = true;
    //}

}

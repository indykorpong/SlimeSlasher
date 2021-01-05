using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Get randomed line type and its properties
public class DrawingManager : MonoBehaviour {
    private static DrawingManager s_instance = null;
    public static List<string> lineMapping = new List<string> { "aflqv", "vwxyz", "vrnie", "agntz", "aflqvwxyz", "vwxyzupke", "vqlfabcde", "abcdekpuz", "agmsxsoie", "vrmhchotz", "aghoposrv", "eihmlmstz", "agntzyxwvrnie", /*"edcbaflqvwxyz", "vqlfabcdkpuyxw",*/ "mnokdcbflqwxyz", "nouyxwqlfbcde", "vqlmgbgmnsxsnoidiopuz", "vqlfagntzupke", "bflqwxyupkd", "edcbfmnouyxwv", "aflmrwrmnhchnotytopke", "", "v" };
    public static List<int> mistakesAllowed = new List<int> { 5, 5, 5, 5, 6, 6, 6, 6, 10, 10, 10, 10, 10, /*9, 10,*/ 12, 12, 15, 10, 12, 12, 15, 0, 0 };
    public static List<int> symbolDifficulty = new List<int> { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, /*2, 2,*/ 2, 3, 3, 3, 3, 2, 3, 3, 9999, 9999};
    public List<Sprite> possibleDrawings;
    public Sprite fullHeart;
    public Sprite deadHeart;
    public Sprite lifePoint1;
    public Sprite lifePoint2;
    public enum LineType {
        straightV, //Vertical line
        straightH, //Horizontal line
        straightD, //Up+Right Diagonal line
        straightD2, //Down+Right Diagonal line
        downLeftCorner,
        downRightCorner,
        upLeftCorner,
        upRightCorner,
        vShape,
        caret,
        greaterThan,
        lesserThan,
        xShape,
        //C,
        //D,
        E,
        G,
        M,
        N,
        O,
        S,
        W,
        nothing,
        dot
    }

    public static DrawingManager instance {
        get {
            if(s_instance == null) {
                s_instance = FindObjectOfType(typeof(DrawingManager)) as DrawingManager;
            }

            if(s_instance == null) {
                GameObject obj = new GameObject("DrawingManager");
                s_instance = obj.AddComponent(typeof(DrawingManager)) as DrawingManager;
                Debug.Log("I created a DrawingManager on my own because I couldn't find any.");
            }
            return s_instance;
        }
    }
    private void OnApplicationQuit() {
        s_instance = null;
    }

    public List<Sprite> getPossibleDrawings() {
        return possibleDrawings;
    }

    public static LineType RandomLineType() {
        return (LineType) UnityEngine.Random.Range(0, (int)LineType.nothing);
    }

    public static LineType ClosestLineType(string lineCoords) {
        int minMistake = 9999;
        LineType closest = LineType.nothing;
        foreach (LineType lt in Enum.GetValues(typeof(LineType))) {
            string thisMapping = lineMapping[(int)lt];
            if (ShapeProximity(thisMapping, lineCoords) <= DrawingManager.mistakesAllowed[(int)lt] && ShapeProximity(thisMapping, lineCoords) < minMistake) {
                closest = lt;
                minMistake = ShapeProximity(thisMapping, lineCoords);
            }
        }
        return closest;
    }

    private static int Min(int a, int b) {
        if (a < b)
            return a;
        return b;
    }

    public static int CompareDistance(string s, string t) {
        int ls = s.Length;
        int lt = t.Length;
        int[,] d = new int[ls + 1, lt + 1];
        for (int i = 1; i <= ls; i++) {
            d[i, 0] = i;
        }
        for (int j = 1; j <= lt; j++) {
            d[0, j] = j;
        }
        int cost;
        for (int j = 1; j <= lt; j++) {
            for (int i = 1; i <= ls; i++) {
                if (s[i - 1].Equals(t[j - 1])) {
                    cost = 0;
                } else {
                    cost = 1;
                }
                d[i, j] = Min(Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
            }
        }
        return d[ls, lt];
    }
    public static int ShapeProximity(string s, string t) {
        return Min(CompareDistance(s, t), CompareDistance(s, Reverse(t)));
    }

    public static string Reverse(string s) {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
}

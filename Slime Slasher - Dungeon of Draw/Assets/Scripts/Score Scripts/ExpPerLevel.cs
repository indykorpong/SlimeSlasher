using UnityEngine;
using System.Collections;

public class ExpPerLevel: MonoBehaviour
{
    public static ExpPerLevel _instance = null;
    private static ulong[] expArray = new ulong[201];

    public static ExpPerLevel instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(ExpPerLevel)) as ExpPerLevel;
            }

            if (_instance == null)
            {
                GameObject obj = new GameObject("ExpPerLevel");
                _instance = obj.AddComponent(typeof(ExpPerLevel)) as ExpPerLevel;
                Debug.Log("Created an ExpPerLevel.");
            }
            return _instance;
        }
    }

    private static void CalculateExpArray(){
        ulong exp = 0;
        ulong killsPerGame = 100;

        expArray[0] = 0;
        for (ulong i = 1; i <= 200; i++)
        {
            exp += (ulong)(i * killsPerGame * GamesToLevelUp(i));
            expArray[i] = exp;
        }
    }

    private static ulong GamesToLevelUp(ulong k)
    {
        ulong result = 1;
        ulong a;
        double ke = (double)k;
        a = (ulong)System.Math.Ceiling(ke / 5.0);
        for (ulong i = 0; i < a; i++)
        {
            result *= 2;
        }
        //Debug.Log("2^" + a + " = " + result);
        return result;
    }

    public ulong[] GetExpArray(){
        CalculateExpArray();
        Debug.Log("Returned an expArray");
        return expArray;
    }
}

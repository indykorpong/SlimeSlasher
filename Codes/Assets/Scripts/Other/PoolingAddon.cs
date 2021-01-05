using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingAddon : MonoBehaviour
{

    [Tooltip("Prefabs to be instantiated")]
    public GameObject prefab;
    [Tooltip("Spawn under")]
    public GameObject parent;
    [Tooltip("Allow more objects to instantiate if needed")]
    public bool allowGrowth = true;
    [Tooltip("The amount instantiated on start")]
    public int initialPoolAmount;

    public List<GameObject> objectPool;

    private void Start()
    {
        for (int i = 0; i < initialPoolAmount; i++)
        {
            GameObject obj;
            if (parent == null)
            {
                obj = Instantiate(prefab);
            }
            else
            {
                obj = Instantiate(prefab, parent.transform);
            }
            obj.SetActive(false);
            objectPool.Add(obj);
        }
    }

    public GameObject RequestGameObject()
    {
        for (int i = 0; i < objectPool.Count; i++)
        //foreach (GameObject gObj in objectPool)
        {
            GameObject gObj = objectPool[i];
            if (!gObj.activeInHierarchy)
            {
                return gObj; // Exits code
            }
        }

        if (allowGrowth)
        {
            GameObject obj;
            if (parent == null)
            {
                obj = Instantiate(prefab);
            }
            else
            {
                obj = Instantiate(prefab, parent.transform);
            }
            obj.SetActive(false);
            objectPool.Add(obj);
            return obj; // Exits Code
        }

        return null;
    }
}
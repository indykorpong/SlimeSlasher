using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager SharedInstance;
    public List<ObjectPoolItem> itemsToPool;
    public List<GameObject> pooledMonsters;
    public List<GameObject> pooledItems;
    public List<GameObject> pooledBackgrounds;
    // Use this for initialization
    void Awake()
    {
        SharedInstance = this;
    }
    void Start()
    {
        pooledMonsters = new List<GameObject>();
        pooledItems = new List<GameObject>();
        pooledBackgrounds = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obj = Instantiate(item.objectToPool);
                if (obj.GetComponent<Monster>() != null)
                {
                    pooledMonsters.Add(obj);
                }
                else if (obj.GetComponent<GameItem>() != null)
                {
                    pooledItems.Add(obj);
                }
                else
                {
                    pooledBackgrounds.Add(obj);
                }
                obj.SetActive(false);
            }
        }
    }

    //Get a monster from the object pool
    public GameObject GetPooledMonster(string tag)
    {
        for (int i = 0; i < pooledMonsters.Count; i++)
        {
            if (!pooledMonsters[i].activeInHierarchy && pooledMonsters[i].tag == tag)
            {
                return pooledMonsters[i];   //Return a monster if the gameobject is inactive and the tags are matched
            }
        }
        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.objectToPool.tag == tag)
            {
                if (item.shouldExpand)
                {
                    GameObject obj = (GameObject)Instantiate(item.objectToPool);
                    obj.SetActive(false);
                    pooledMonsters.Add(obj);
                    return obj;   //If the tags aren't matched, create a new one and add to the pool
                }
            }
        }
        return null;
    }

    //Get an item from the object pool
    public GameObject GetPooledItem(string tag)
    {
        for (int i = 0; i < pooledItems.Count; i++)
        {
            if (!pooledItems[i].activeInHierarchy && pooledItems[i].tag == tag)
            {
                return pooledItems[i];
            }
        }
        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.objectToPool.tag == tag)
            {
                if (item.shouldExpand)
                {
                    GameObject obj = (GameObject)Instantiate(item.objectToPool);
                    obj.SetActive(false);
                    pooledItems.Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }

    public GameObject GetPooledBG(string tag)
    {
        for (int i = 0; i < pooledItems.Count; i++)
        {
            if (!pooledBackgrounds[i].activeInHierarchy && pooledBackgrounds[i].tag == tag)
            {
                return pooledBackgrounds[i];
            }
        }
        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.objectToPool.tag == tag)
            {
                if (item.shouldExpand)
                {
                    GameObject obj = (GameObject)Instantiate(item.objectToPool);
                    obj.SetActive(false);
                    pooledBackgrounds.Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }

    //   // Update is called once per frame
    //   void Update () {

    //}

    /*public GameObject GetPooledObject(Monster.MonsterType mt) {
        //checks if the monster type is sideways or not, if it is then check in sideways, if not then check in monsters

        for (int i = 0; i < pooledObjects.Count; i++) {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].GetComponent<Monster>() {
                return pooledObjects[i];
            }
        }
        foreach (ObjectPoolItem item in itemsToPool) {
            if (item.objectToPool.tag == tag) {
                if (item.shouldExpand) {
                    GameObject obj = (GameObject)Instantiate(item.objectToPool);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                    return obj;
                }
            }
        }
    }*/
}

[System.Serializable]
public class ObjectPoolItem
{
    public int amountToPool;
    public GameObject objectToPool;
    public bool shouldExpand;
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {

    public GameObject[] myItem;
    public enum ItemType {
        hell,
        unhell,
    }
    List<string> itemTag = new List<string> { "HellBlob", "UnhellBlob" };

    private static List<List<int>> appearanceChance = new List<List<int>> {
        new List<int> { 5, 0 },
        new List<int> { 5, 0 },
        new List<int> { 5, 0 },
        new List<int> { 5, 0 },
        new List<int> { 5, 0 },
        new List<int> { 5, 0 }
    };

    // Use this for initialization
    private void Awake() {
        //DontDestroyOnLoad(gameObject.GetComponent<AudioSource>());
        //DontDestroyOnLoad(gameObject);
    }

    void Start() {
        StartCoroutine(SpawnMonsters());
    }

    // Update is called once per frame
    void Update() {
        
    }

    IEnumerator SpawnMonsters() {
        while (true) {
            yield return new WaitForSeconds(UnityEngine.Random.Range(30f, 45f));
            //yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 5f));
            if (GameManager.gameIsRunning && !GameManager.tapEnabled) {
                ItemType itemType = RandomItemType();
                //hell is heaven in hell, heaven is hell in heaven
                if (GameManager.hellcourse && itemType == ItemType.hell) {
                    itemType = ItemType.unhell;
                } else if (!GameManager.hellcourse && itemType == ItemType.unhell) {
                    itemType = ItemType.hell;
                }
                GameObject gObj = PoolingManager.SharedInstance.GetPooledItem(itemTag[(int)itemType]);
                gObj.SetActive(true);

                Vector3 randomSpot = gameObject.transform.localPosition;
                float margin = 75;
                float spawnEdge1 = Camera.main.ScreenToWorldPoint(new Vector3(margin, 0, 0)).x;
                float spawnEdge2 = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - margin, 0, 0)).x;
                randomSpot.x = UnityEngine.Random.Range(spawnEdge1, spawnEdge2);
                gObj.transform.SetPositionAndRotation(randomSpot, Quaternion.identity);

                //gObj.GetComponent<GameItem>().InitItem();
                //if(gObj.GetComponent<ModeChangeMonster>() != null) {
                //    gObj.GetComponent<ModeChangeMonster>().SpecialItemAction();
                //}

                Collider2D itemCollider = gObj.GetComponent<Collider2D>();
                var allMonsters = FindObjectsOfType<Monster>();
                foreach (Monster m in allMonsters) {
                    Collider2D mCollider = m.gameObject.GetComponent<Collider2D>();
                    if (!gObj.Equals(m.gameObject) && itemCollider.bounds.Intersects(mCollider.bounds)) {
                        Debug.Log("Item don't spawn because monster");
                        gObj.SetActive(false);
                        break;
                    }
                }
                var allItems = FindObjectsOfType<GameItem>();
                foreach (GameItem i in allItems) {
                    Collider2D iCollider = i.gameObject.GetComponent<Collider2D>();
                    if (!gObj.Equals(i.gameObject) && itemCollider.bounds.Intersects(iCollider.bounds)) {
                        Debug.Log("Item don't spawn because item");
                        gObj.SetActive(false);
                        break;
                    }
                }
            }
        }
    }

    public static ItemType RandomItemType() {
        List<ItemType> randomList = new List<ItemType>();
        List<int> chancelist = appearanceChance[0];
        for (int i = 0; i < appearanceChance.Count - 1; i++) {
            if (GameManager.kills >= GameManager.killsToLevels[i]) {
                chancelist = appearanceChance[i + 1];
            }
        }
        //Debug.Log(chancelist[1]);
        foreach (ItemType m in Enum.GetValues(typeof(ItemType))) {
            for (int i = 0; i < chancelist[(int)m]; i++) {
                randomList.Add(m);
            }
        }
        ItemType randomType = randomList[UnityEngine.Random.Range(0, randomList.Count)];
        //Debug.Log(randomType);
        return randomType;
    }
}


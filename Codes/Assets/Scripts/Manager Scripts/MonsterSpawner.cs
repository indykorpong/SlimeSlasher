using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour {

    public float minSpawnTime;
    public float maxSpawnTime;
    public float minSpawnTimeTap;
    public float maxSpawnTimeTap;

    public GameObject[] myMonster;
    public enum MonsterType {
        normal,
        fast,
        sniper,
        tough,
        conga,
        shield,
        heal,
        explode,
        tapper
    }
    private MonsterType currentType;
    List<string> monsterTag = new List<string> { "NormalBlob", "FastBlob", "SniperBlob", "ToughBlob", "CongaBlob", "ShieldBlob", "HealBlob", "ExplodeBlob", "TapperBlob" };

    private static List<List<int>> appearanceChance = new List<List<int>> {
        new List<int> {82, 5, 5, 5, 0, 2, 2, 2, 2},
        new List<int> {75, 6, 6, 6, 1, 2, 2, 2, 2},
        new List<int> {69, 7, 7, 7, 2, 2, 2, 2, 2},
        new List<int> {60, 8, 8, 8, 2, 2, 2, 2, 2},
        new List<int> {55, 9, 9, 9, 3, 2, 2, 2, 2},
        new List<int> {41, 10, 10, 10, 5, 2, 2, 2, 2}};
    
    // Use this for initialization
    private void Awake() {
        //DontDestroyOnLoad(gameObject.GetComponent<AudioSource>());
        //DontDestroyOnLoad(gameObject);
    }

    private IEnumerator spawnMonsterRef;
    void Start() {
        currentType = MonsterType.normal;
        spawnMonsterRef = SpawnMonsters();
        StartCoroutine(spawnMonsterRef);
    }

    // Update is called once per frame
    void Update() {

    }
    
    private IEnumerator SpawnMonsters() {
        while (true) {
            float randomTime;
            if (GameManager.tapEnabled) {
                //chooses spawntime for TAP!! mode
                randomTime = UnityEngine.Random.Range(minSpawnTimeTap, maxSpawnTimeTap);
            } else {
                //chooses spawntime for normal mode
                randomTime = UnityEngine.Random.Range(minSpawnTime, maxSpawnTime);
            }
            
            // Delay between spawns
            yield return new WaitForSeconds(randomTime);
            if (GameManager.gameIsRunning)
            {


                MonsterType monsterType = RandomMonsterType();
                //you cannot heal while in hell
                while (generatedHealInHell(monsterType)
                    || generatedHealWhenMaxHP(monsterType)
                    || generatedShieldWhenShielded(monsterType)){
                    monsterType = RandomMonsterType();
                }

                //if (GameManager.hellcourse && monsterType == Monster.MonsterType.hell) {
                //    monsterType = Monster.MonsterType.unhell;
                //} else if (!GameManager.hellcourse && monsterType == Monster.MonsterType.unhell) {
                //    monsterType = Monster.MonsterType.hell;
                //}

                //GameObject gObj = Instantiate(myMonster[(int)monsterType]);
                GameObject gObj = PoolingManager.SharedInstance.GetPooledMonster(monsterTag[(int)monsterType]);
                //Debug.Log(monsterType);
                Monster myMonster = gObj.GetComponent<Monster>();
                gObj.SetActive(true);
               
                //Debug.Log("Honk!");

                //Vector3 randomSpot = gameObject.transform.localPosition;
                //float margin = 75;
                //float spawnEdge1 = Camera.main.ScreenToWorldPoint(new Vector3(margin, 0, 0)).x;
                //float spawnEdge2 = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - margin, 0, 0)).x;
                //randomSpot.x = UnityEngine.Random.Range(spawnEdge1, spawnEdge2);
                //randomSpot.y = 6;
                //gObj.transform.SetPositionAndRotation(randomSpot, Quaternion.identity);

                //If the current item is the same as the last one, destroy it
                if (monsterType == MonsterType.shield || monsterType == MonsterType.heal || monsterType == MonsterType.explode || monsterType == MonsterType.tapper){
                    if(currentType == monsterType || GameManager.tapEnabled){
                        gObj.SetActive(false);
                        Debug.Log("Monster doesn't spawn because it's the same as the one before, or it is currently tap mode.");
                    }
                }
                currentType = monsterType;

                //If the spawning monster intersects with another, destroy it
                Collider2D monsterCollider = gObj.GetComponent<Collider2D>();
                var allMonsters = FindObjectsOfType<Monster>();
                foreach(Monster m in allMonsters){
                    Collider2D mCollider = m.gameObject.GetComponent<Collider2D>();
                    if(!gObj.Equals(m.gameObject) && monsterCollider.bounds.Intersects(mCollider.bounds) && currentType != MonsterType.conga){
                        Debug.Log("Monster don't spawn because monster");
                        gObj.SetActive(false);
                        break;
                    }
                }
                var allItems = FindObjectsOfType<GameItem>();
                foreach (GameItem i in allItems) {
                    Collider2D iCollider = i.gameObject.GetComponent<Collider2D>();
                    if (!gObj.Equals(i.gameObject) && monsterCollider.bounds.Intersects(iCollider.bounds)) {
                        Debug.Log("Monster don't spawn because item");
                        gObj.SetActive(false);
                        break;
                    }
                }

                //gObj.GetComponent<Monster>().myType = monsterType;
                //gObj.SetActive(true);

            }
        }
    }

    public static MonsterType RandomMonsterType() {
        List<MonsterType> randomList = new List<MonsterType>();
        List<int> chancelist = appearanceChance[0];
        for (int i = 0; i < appearanceChance.Count - 1; i++) {
            if (GameManager.kills >= GameManager.killsToLevels[i]) {
                chancelist = appearanceChance[i + 1];
            }
        }
        //Debug.Log(chancelist[1]);
        foreach (MonsterType m in Enum.GetValues(typeof(MonsterType))) {
            for (int i = 0; i < chancelist[(int)m]; i++) {
                randomList.Add(m);
            }
        }
        MonsterType randomType = randomList[UnityEngine.Random.Range(0, randomList.Count)];
        //Debug.Log(randomType);
        return randomType;
    }

    private bool generatedHealInHell(MonsterType monsterType) {
        return GameManager.hellcourse && monsterType == MonsterType.heal;
    }

    private bool generatedHealWhenMaxHP(MonsterType monsterType) {
        return GameManager.lives == 3 && monsterType == MonsterType.heal;
    }

    private bool generatedShieldWhenShielded(MonsterType monsterType) {
        return GameManager.shielded && monsterType == MonsterType.shield;
    }
}


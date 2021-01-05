using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {
    public DrawingManager.LineType myLine;
    public MonsterSpawner.MonsterType myType;
    private SpriteRenderer mySymbolSprite;
    public SpriteRenderer myMonsterSprite;
    public SpriteRenderer myMonsterDrawing;
    public Animator myMonsterAnimation;
    public int health;
    public int maxHealth;
    public int points;
    public int maxDifficulty;
    public static int killCap = 90;
    private bool multiplicative;
    private float percentIncreasePerKill;
    public bool spawnRandomly;
    private static GameManager gm;
    private static DrawingManager dm;
    public Vector3 velocity;
    public GameObject myExplosion;
    

    //public enum MonsterType {
    //    none,
    //    normal,
    //    fast,
    //    sniper,
    //    tough,
    //    conga,
    //    hell,
    //    unhell,
    //    multiplier,
    //    heal,
    //    explode
    //}
    //private static List<int> appearanceChance = new List<int>{0, 70, 10, 10, 0, 5, 3, 2};
    //private static List<int> appearanceChance = new List<int> { 0, 60, 10, 10, 5, 5, 6, 4};
    //private static List<int> appearanceChance = new List<int> { 0, 50, 10, 10, 10, 10, 6, 4 };
    //private static List<List<int>> appearanceChance = new List<List<int>> {
    //    new List<int> { 0, 140, 5, 5, 5, 0, 5, 0, 5, 5, 5},
    //    new List<int> { 0, 120, 6, 6, 6, 1, 5, 0, 5, 5, 5},
    //    new List<int> { 0, 120, 7, 7, 7, 2, 7, 0, 7, 7, 7},
    //    new List<int> { 0, 100, 8, 8, 8, 2, 7, 0, 7, 7, 7},
    //    new List<int> { 0, 100, 9, 9, 9, 3, 9, 0, 9, 9, 9},
    //    new List<int> { 0, 70, 10, 10, 10, 5, 9, 0, 9, 9, 9}};
    //private static List<List<int>> appearanceChance = new List<List<int>> {
    //    new List<int> { 0, 0, 0, 0, 0, 10, 5, 0, 5, 5, 5},
    //    new List<int> { 0, 0, 0, 0, 0, 10, 5, 0, 5, 5, 5},
    //    new List<int> { 0, 0, 0, 0, 0, 10, 7, 0, 7, 7, 7},
    //    new List<int> { 0, 0, 0, 0, 0, 10, 7, 0, 7, 7, 7},
    //    new List<int> { 0, 0, 0, 0, 0, 10, 9, 0, 9, 9, 9},
    //    new List<int> { 0, 0, 0, 0, 0, 10, 9, 0, 9, 9, 9}};
    //private static List<int> killsToLevels = new List<int> {10, 25, 50, 75, 100};
    //private static List<int> appearanceChance = new List<int> { 0, 5, 0, 0, 0, 1, 0, 0, 0, 0};
    // Use this for initialization
    public void Awake() {
        multiplicative = true;
        percentIncreasePerKill = 1.5f;
        dm = FindObjectOfType<DrawingManager>();
        gm = FindObjectOfType<GameManager>();
        foreach (SpriteRenderer s in this.GetComponentsInChildren<SpriteRenderer>()) {
            if (s.tag == "MonsterSprite") myMonsterSprite = s;
            if (s.tag == "MonsterDrawing") myMonsterDrawing = s;
        }
        foreach (Animator s in this.GetComponentsInChildren<Animator>()) {
            if (s.tag == "MonsterSprite") myMonsterAnimation = s;
        }
    }
    public void OnEnable() {
        SetMyLine(DrawingManager.RandomLineType(), false);
        health = maxHealth;
        if (spawnRandomly) {
            Vector3 randomSpot = gameObject.transform.localPosition;
            float margin = 75;
            float spawnEdge1 = Camera.main.ScreenToWorldPoint(new Vector3(margin, 0, 0)).x;
            float spawnEdge2 = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - margin, 0, 0)).x;
            randomSpot.x = UnityEngine.Random.Range(spawnEdge1, spawnEdge2);
            randomSpot.y = 6;
            gameObject.transform.SetPositionAndRotation(randomSpot, Quaternion.identity);
        }
        myMonsterDrawing.enabled = !GameManager.tapEnabled;
    }

    //public void SpecialMonsterAction() {
        //depends on the inheriting monster type
    //}

    // Update is called once per frame
    void Update() {
        MonsterMove();
        DestroyIfOutOfBounds(true);
        if (health == 0) {
            DestroySelfAndGetPoints();
        }
        
    }

    public void MonsterMove() {
        float speedMultiplier;
        //is the kills more than the cap? if it is then set it as cap
        int cappedKills = GameManager.kills > killCap ? killCap : GameManager.kills;
        if (multiplicative) {
            speedMultiplier = Mathf.Pow((1f + (percentIncreasePerKill * 0.01f)), cappedKills);
        } else {
            speedMultiplier = 1f + (percentIncreasePerKill * cappedKills);
        }
        transform.position += (velocity * Time.deltaTime * speedMultiplier);
    }
    
    public void DestroyIfOutOfBounds(bool hurt) {
        if (Camera.main.WorldToScreenPoint(transform.localPosition).y < -20) {
            Debug.Log("An enemy has escaped!");
            if (hurt) {
                gm.LoseLife();
                if(GameManager.lives <= 0) {
                    GameManager.killerType = myType;
                }
            }
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }

    public void DestroySelfAndGetPoints() {
        //Debug.Log("Evil vanquished.");
        GameManager.IncrementScore(points);
        Instantiate(myExplosion, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    public void SetMyLine(DrawingManager.LineType newLine) {
        myLine = newLine;
        SpriteRenderer[] mySymbol = gameObject.GetComponentsInChildren<SpriteRenderer>();
        
        foreach (SpriteRenderer s in mySymbol) {
            if (s.tag == "MonsterDrawing") {
                if (myLine == DrawingManager.LineType.nothing) {
                    s.enabled = false;
                } else {
                    s.enabled = true;
                    s.gameObject.transform.localScale = new Vector3(0.125f, 0.125f, 1);
                    s.sprite = dm.possibleDrawings[(int)newLine];
                }
            }
        }
    }

    public void SetMyLine(DrawingManager.LineType newLine, bool bypassDifficulty) {
        if (!bypassDifficulty) {
            while((DrawingManager.symbolDifficulty[(int) newLine] > maxDifficulty
                || DrawingManager.symbolDifficulty[(int) newLine] > (GameManager.kills / 10) + 1)){ //generate a new symbol if the generated symbol has a higher difficulty than the max difficulty allowed for the monster or is greater than (kills/10)+2 (Max 1 for 0-9 kills, Max 2 for 10-19 kills...
                newLine = DrawingManager.RandomLineType();
            }
        }
        SetMyLine(newLine);
    }
}

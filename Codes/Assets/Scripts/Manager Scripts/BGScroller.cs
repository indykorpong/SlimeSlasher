using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroller : MonoBehaviour
{
    public Sprite notHellBG;
    public Sprite hellBG;
    public Color notHellColor;
    public Color hellColor;
    public Vector3 velocity;
    public static int killCap = 90;
    public bool hasSpawnedNewBG = false;

    private Vector3 spawnPoint = new Vector3(0, 10.35f, 10);
    private float _tmp_speedMultiplier;
    private void OnEnable()
    {
        hasSpawnedNewBG = false;
    }
    private void FixedUpdate()
    {
        int cappedKills = GameManager.kills > killCap ? killCap : GameManager.kills;
        _tmp_speedMultiplier = Mathf.Pow(1.015f, cappedKills);
        transform.position += (velocity * Time.fixedDeltaTime * _tmp_speedMultiplier);
        if (gameObject.transform.position.y <= -10.35f)
        {
            if (gameObject.name == "FirstBG")
            {
                Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        if (gameObject.transform.position.y < 0 && !hasSpawnedNewBG)
        {
            GameObject gObj = PoolingManager.SharedInstance.GetPooledBG(gameObject.tag);
            gObj.transform.SetPositionAndRotation(spawnPoint, Quaternion.identity);
            gObj.SetActive(true);
            //gObj.GetComponent<BGScroller>().InitBG();
            hasSpawnedNewBG = true;
        }
        if(!GameManager.hellcourse) {
            gameObject.GetComponent<SpriteRenderer>().sprite = notHellBG;
            Camera.main.backgroundColor = notHellColor;
        } else {
            gameObject.GetComponent<SpriteRenderer>().sprite = hellBG;
            Camera.main.backgroundColor = hellColor;
        }
    }
}

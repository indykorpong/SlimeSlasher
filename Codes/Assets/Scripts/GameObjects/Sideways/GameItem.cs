using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItem : MonoBehaviour
{
    public Vector3 spawnPoint;
    private SpriteRenderer myMonsterSprite;
    public Vector3 velocity;
    public bool isAlive;
    public static int killCap = 90;
    private bool multiplicative;
    public GameObject myExplosion;
    private float percentIncreasePerKill;

    private void Awake() {
        multiplicative = true;
        percentIncreasePerKill = 1.5f;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "GameBoundry")
        {
            gameObject.SetActive(false);
        }
    }

    //public void DestroyIfOutOfBounds() { }
    //public void DestroyIfOutOfBounds() {
    //    //if (Camera.main.WorldToScreenPoint(transform.localPosition).x < -150 || Camera.main.WorldToScreenPoint(transform.localPosition).x > Screen.width + 150) {
    //    //    //Debug.Log("No hell for you, I'm at " + Camera.main.WorldToScreenPoint(transform.localPosition));
    //    //    gameObject.SetActive(false);
    //    //    //Destroy(gameObject);
    //    //}
    //    if (Camera.main.WorldToScreenPoint(transform.localPosition).y < -20) {
    //        Debug.Log("An enemy has escaped!");
    //        //GameManager.LoseLife();
    //        //Destroy(gameObject);
    //        gameObject.SetActive(false);
    //    }
    //}
    public void ItemMove()
    {
        //float speedMultiplier = Mathf.Pow(1.01f, GameManager.kills);
        //if (spawnPoint.x > 0) {
        //    transform.position += (velocity * Time.deltaTime * speedMultiplier);
        //} else {
        //    transform.position -= (velocity * Time.deltaTime * speedMultiplier);
        //}
        float speedMultiplier;
        int cappedKills = GameManager.kills > killCap ? killCap : GameManager.kills;
        if (multiplicative) {
            speedMultiplier = Mathf.Pow((1f + (percentIncreasePerKill * 0.01f)), cappedKills);
        } else {
            speedMultiplier = 1f + (percentIncreasePerKill * cappedKills);
        }
        transform.position += (velocity * Time.deltaTime * speedMultiplier);
    }

    public void OnEnable(){
        //float topMargin = 3;
        //float bottomMargin = -2;
        isAlive = true;
        //spawnPoint = new Vector3 {
        //    x = (Random.Range(0f, 1f) > 0.5) ? Camera.main.ScreenToWorldPoint(new Vector3(-100, 0, 0)).x : Camera.main.ScreenToWorldPoint(new Vector3(Screen.width + 100, 0, 0)).x,
        //    y = Random.Range(topMargin, bottomMargin),
        //    z = 0
        //};
        //this.transform.SetPositionAndRotation(spawnPoint, Quaternion.identity);
    }
}

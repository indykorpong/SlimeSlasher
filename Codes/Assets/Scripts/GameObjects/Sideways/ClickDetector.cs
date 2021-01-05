using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDetector : MonoBehaviour
{

    private RaycastHit2D hit;
    private Vector3 pos;
    private GameItem detectedItem;
    private Monster detectedMonster;
    private Vector2 vec2Zero = Vector2.zero;
    private static GameManager gameManager;
    private static SoundManager soundManager;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        soundManager = FindObjectOfType<SoundManager>();
    }
    private void Update()
    {
        pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        hit = Physics2D.Raycast(pos, vec2Zero);
        if (Input.GetKeyDown(KeyCode.Mouse0) && (hit.collider != null))
        {
            //Debug.Log("I'm hitting " + hit.collider.name);
            //GameManager.hitModeChangeBlob = true;
            detectedItem = hit.collider.GetComponent<GameItem>();
            detectedMonster = hit.collider.GetComponent<Monster>();
            if (detectedItem != null)
            {
                detectedItem.isAlive = false;
            }else if(detectedMonster != null && GameManager.tapEnabled) {
                detectedMonster.health--;
                soundManager.RandomDeathSound().Play();
            }
            detectedItem = null;
        }
    }
}

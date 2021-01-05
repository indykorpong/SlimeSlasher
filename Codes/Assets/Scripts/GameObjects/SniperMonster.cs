using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperMonster : Monster {

    public GameObject bullet;
    public bool isAtLineOfFire = false;
    public bool firstShot = false;
    public int lineOfFire = 3;

    new void OnEnable() {
        base.OnEnable();
        firstShot = false;
    }

    void Update() {
        if (transform.position.y >= lineOfFire) {
            MonsterMove();
            isAtLineOfFire = false;
        } else {
            isAtLineOfFire = true;
        }

        if (isAtLineOfFire && !firstShot) {
            StartCoroutine(ShootTheBullet());
            firstShot = true;
        }

        if (health == 0) {
            DestroySelfAndGetPoints();
        }
        DestroyIfOutOfBounds(true);
    }

    IEnumerator ShootTheBullet() {
        while (true) {
            GameObject bulletObject = PoolingManager.SharedInstance.GetPooledMonster("Bullet");
            bulletObject.SetActive(true);
            bulletObject.transform.SetPositionAndRotation(gameObject.transform.localPosition, Quaternion.identity);
            Monster bulletInstance = bulletObject.GetComponent<Monster>();
            while(bulletInstance.myLine == this.myLine) {
                bulletInstance.SetMyLine(DrawingManager.RandomLineType(), false);
            }
            yield return new WaitForSeconds(2.5f);
        }
    }
}

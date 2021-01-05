using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TapperMonster : Monster {
    
    void Update() {
        MonsterMove();
        DestroyIfOutOfBounds(false);
        if (health <= 0) {
            OnDeath();
        }
    }

    private void OnDeath() {
        GameManager.tapEnabled = true;
        gameObject.SetActive(false);
    }
}

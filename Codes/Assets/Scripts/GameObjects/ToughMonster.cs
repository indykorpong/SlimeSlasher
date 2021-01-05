using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToughMonster : Monster {
    public Sprite noDamage;
    public Sprite oneDamage;
    public Sprite twoDamage;
    public RuntimeAnimatorController threeHP;
    public RuntimeAnimatorController twoHP;
    public RuntimeAnimatorController oneHP;
    void Update() {
        MonsterMove();
        DestroyIfOutOfBounds(true);
        switch (health) {
            case (3):
                myMonsterSprite.sprite = noDamage;
                myMonsterAnimation.runtimeAnimatorController = threeHP;
                break;
            case (2):
                myMonsterSprite.sprite = oneDamage;
                myMonsterAnimation.runtimeAnimatorController = twoHP;
                break;
            case (1):
                myMonsterSprite.sprite = twoDamage;
                myMonsterAnimation.runtimeAnimatorController = oneHP;
                break;

            case (0):
                DestroySelfAndGetPoints();
                break;
        }
    }
}

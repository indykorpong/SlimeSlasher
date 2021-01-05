using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CongaMonster : Monster {

    //public GameObject CongaLineBlob;
    public static List<List<DrawingManager.LineType>> congaWords = new List<List<DrawingManager.LineType>>{
        new List<DrawingManager.LineType> {DrawingManager.LineType.caret, DrawingManager.LineType.N, DrawingManager.LineType.G, DrawingManager.LineType.E, DrawingManager.LineType.upLeftCorner},
        new List<DrawingManager.LineType> {DrawingManager.LineType.lesserThan, DrawingManager.LineType.O, DrawingManager.LineType.N, DrawingManager.LineType.G, DrawingManager.LineType.caret},
        //new List<DrawingManager.LineType> {DrawingManager.LineType.D, DrawingManager.LineType.caret, DrawingManager.LineType.N, DrawingManager.LineType.lesserThan, DrawingManager.LineType.E},
        new List<DrawingManager.LineType> {DrawingManager.LineType.G, DrawingManager.LineType.caret, DrawingManager.LineType.M, DrawingManager.LineType.E, DrawingManager.LineType.S},
        new List<DrawingManager.LineType> {DrawingManager.LineType.S, DrawingManager.LineType.downLeftCorner, DrawingManager.LineType.straightV, DrawingManager.LineType.M, DrawingManager.LineType.E},
        new List<DrawingManager.LineType> {DrawingManager.LineType.M, DrawingManager.LineType.O, DrawingManager.LineType.vShape, DrawingManager.LineType.E, DrawingManager.LineType.S},
        new List<DrawingManager.LineType> {DrawingManager.LineType.upLeftCorner, DrawingManager.LineType.caret, DrawingManager.LineType.N, DrawingManager.LineType.G, DrawingManager.LineType.E},
        new List<DrawingManager.LineType> {DrawingManager.LineType.downLeftCorner, DrawingManager.LineType.straightV, DrawingManager.LineType.N, DrawingManager.LineType.E, DrawingManager.LineType.S},
        new List<DrawingManager.LineType> {DrawingManager.LineType.W, DrawingManager.LineType.upLeftCorner, DrawingManager.LineType.O, DrawingManager.LineType.N, DrawingManager.LineType.G},
    };

    private bool isFirstTime = true;

    new void OnEnable() {
        base.OnEnable();
        if (isFirstTime) {
            isFirstTime = false;
            return;
        }
        
        Vector3 myPosition = gameObject.transform.position;
        //Debug.Log(gameObject.transform.position);
        List<DrawingManager.LineType> choreography = null;
        if (!GameManager.tapEnabled) {
            choreography = congaWords[Random.Range(0, congaWords.Count)];
            SetMyLine(choreography[0]);
        }
        
        myPosition.y += 5f;
        transform.SetPositionAndRotation(myPosition, Quaternion.identity);
        myPosition = gameObject.transform.localPosition;
        //Debug.Log(myPosition);
        for (int i = 0; i < 4; i++) {
            myPosition.y -= 1f;
            GameObject backupDancer = PoolingManager.SharedInstance.GetPooledMonster("CongaBack");
            backupDancer.transform.SetPositionAndRotation(myPosition, Quaternion.identity);
            //Debug.Log(myPosition);
            Monster dancer = backupDancer.GetComponent<Monster>();
            backupDancer.SetActive(true);
            if(!GameManager.tapEnabled) dancer.SetMyLine(choreography[i + 1]);
        }
    }
}

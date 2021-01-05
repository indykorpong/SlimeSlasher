using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBGToUI : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        //RectTransform myCanvasRect = gameObject.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        RectTransform myRect = gameObject.GetComponent<RectTransform>();
        //myRect.localScale = myCanvasRect.lossyScale;
        Debug.Log(Camera.main.rect);
        myRect.rect.Set(Camera.main.rect.x, Camera.main.rect.y, Camera.main.rect.width, Camera.main.rect.height);
            
	}
}

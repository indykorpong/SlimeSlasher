using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeUI : MonoBehaviour
{
    private DrawingManager dm;
    public int lifeNumber;
    private Image myHeart;

    private void Awake()
    {
        dm = FindObjectOfType<DrawingManager>();
        myHeart = gameObject.GetComponent<Image>();
    }

    private void OnEnable()
    {
        GameManager.LivesRemain += SetLifeCount;
        GameManager.OnTapStateChange.AddListener(TapStateChange);

        //GameManager.OnTapperClock += ;
    }

    private void OnDisable()
    {
        GameManager.LivesRemain -= SetLifeCount;
        GameManager.OnTapStateChange.RemoveListener(TapStateChange);

        //GameManager.OnTapperClock += ;

    }

    private void SetLifeCount(int lifeRemain)
    {
        if (lifeNumber <= lifeRemain)
        {
            myHeart.sprite = dm.fullHeart;
        }
        else
        {
            myHeart.sprite = dm.deadHeart;
        }
        myHeart.enabled = (lifeRemain != 0);
    }

    private void TapStateChange(bool tapState) {
        myHeart.enabled = !tapState;
    }
    

    //private void Update()
    //{
    //    if (lifeNumber <= GameManager.lives)
    //    {
    //        myHeart.sprite = dm.fullHeart;
    //    }
    //    else
    //    {
    //        myHeart.sprite = dm.deadHeart;
    //    }
    //}
}

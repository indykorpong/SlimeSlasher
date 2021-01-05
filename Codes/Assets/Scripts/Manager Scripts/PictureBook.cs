using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PictureBook : MonoBehaviour {
    public List<Sprite> pages;
    public int currentPage;
	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Image>().sprite = pages[0];
    }
	
	// Update is called once per frame
	void Update () {
        gameObject.GetComponent<Image>().sprite = pages[currentPage];
	}

    public void DecrementPage() {
        currentPage--;
        if (currentPage < 0) currentPage = 0;
    }

    public void IncrementPage() {
        currentPage++;
        if (currentPage > pages.Count - 1) currentPage = pages.Count - 1;
    }
}

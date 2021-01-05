using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ShowScores : MonoBehaviour
{
    private static int tempScore = 24;
    private static GameObject[] nameAndScores;

    // Use this for initialization
    void Start()
    {
        nameAndScores = new GameObject[DataController.scoreCount];

        for (int i = 0; i < nameAndScores.Length; i++){
            nameAndScores[i] = GameObject.Find("NameAndScore" + (i+1).ToString());
        }
        DataController.instance.LoadPlayerProgress();
        //Modify text in scoreboard
        SetScoreText();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            Debug.Log("pressed space key");

            DataController.instance.LoadPlayerProgress();
            DataController.instance.SavePlayerProgress("Indy", tempScore);
            tempScore += 4;
        }
        SetScoreText();
    }

    public void SetScoreText(){
        //playerText.text = "";
        //scoreText.text = "";
        //for (int i = 0; i < DataController.scoreCount;i++){
        //    if (i >= pScores.Count) break;
        //    playerText.text += i+1 + ". " + pScores[i].playerName + "\n";
        //    scoreText.text += pScores[i].playerScore + "\n";
        //}

        var pScores = DataController.instance.GetPlayerScores();

        for (int i = 0; i < nameAndScores.Length; i++){
            if (i >= pScores.Count) break;
            var textList = nameAndScores[i].GetComponentsInChildren<Text>();
            foreach(Text t in textList){
                if(t.CompareTag("Player Text")){
                    t.text = (i+1).ToString() + ". " + pScores[i].playerName;
                } else if(t.CompareTag("Score Text")){
                    t.text = pScores[i].playerScore.ToString();
                }
            }
        }
    }

}
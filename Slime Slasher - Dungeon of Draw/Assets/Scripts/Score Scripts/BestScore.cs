using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BestScore : MonoBehaviour
{
    public static Text bestScore;
    public bool debugScoreEnabled;
    private static DataController dataController;
    private static int tempScore = 20;
    // Use this for initialization
    private void Awake()
    {
        bestScore = gameObject.GetComponent<Text>();
        dataController = DataController.instance;
        dataController.LoadPlayerProgress();
        UpdateScore();
    }

    public void UpdateScore() {
        if (dataController.GetPlayerScores().Count > 0) {
            bestScore.text = "Best Score : " + dataController.GetPlayerScores()[0].playerScore;
        } else {
            bestScore.text = "Best Score : 0";
        }
    }

    // Update is called once per frame
    void Update(){
        if(debugScoreEnabled && Input.GetKeyDown(KeyCode.Space)) {
            dataController.LoadPlayerProgress();
            dataController.SavePlayerProgress("Indy", tempScore);
            tempScore += 4;
            UpdateScore();
        }
    }

    public void SetScoreTextZero() {
        bestScore.text = "Best Score : 0";
    }
}

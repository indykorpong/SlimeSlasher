using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataController : MonoBehaviour {
    private static DataController _instance = null;
    private static List<PlayerScore> playerScores;
    private static string separator = "\t\t";
    private static char[] delimiters = { '\r', '\t' };

    public static int scoreCount = 7;
	// Use this for initialization
    public static DataController instance {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(DataController)) as DataController;
            }

            if (_instance == null)
            {
                GameObject obj = new GameObject("DataController");
                _instance = obj.AddComponent(typeof(DataController)) as DataController;
                Debug.Log("Created a DataController.");
            }
            return _instance;
        }
    }

	void Start () {
        DontDestroyOnLoad(gameObject);
        //PlayerPrefs.DeleteAll();
    }

    public List<PlayerScore> GetPlayerScores(){
        return playerScores;
    }

    public void LoadPlayerProgress()
    {
        playerScores = new List<PlayerScore>();

        for (int i = 0; i < scoreCount; i++)
        {
            if (PlayerPrefs.HasKey("Score" + i))
            {
                Debug.Log("PlayerPrefs with " + i + " : " + PlayerPrefs.GetString("Score" + i));
                string[] scoreFormat = PlayerPrefs.GetString("Score" + i).Split(delimiters, System.StringSplitOptions.RemoveEmptyEntries);

                var newPlayerScore = new PlayerScore(scoreFormat[0], int.Parse(scoreFormat[1]));
                playerScores.Add(newPlayerScore);
                playerScores.Sort((x, y) => y.playerScore.CompareTo(x.playerScore));
            }
            else
            {
                break;
            }
        }

        Debug.Log("Loading part");
        var tempStr = "";
        foreach (PlayerScore p in playerScores)
        {
            tempStr += p.playerScore + " ";
        }
        Debug.Log(tempStr);
    }

    public void SavePlayerProgress(string name, int score)
    {
        playerScores = new List<PlayerScore>();

        for (int i = 0; i < scoreCount; i++)
        {
            if (PlayerPrefs.HasKey("Score" + i))
            {
                string[] scoreFormat = PlayerPrefs.GetString("Score" + i).Split(delimiters, System.StringSplitOptions.RemoveEmptyEntries);
                playerScores.Add(new PlayerScore(scoreFormat[0], int.Parse(scoreFormat[1])));
            }
            else
            {
                break;
            }
        }
        if (playerScores.Count < 1)
        {
            PlayerPrefs.SetString("Score0", name + separator + score);
        }

        //Add player score in scoreboard and sort the scoreboard 
        playerScores.Add(new PlayerScore(name, score));
        playerScores.Sort((x, y) => y.playerScore.CompareTo(x.playerScore));

        Debug.Log("Saving part");
        var tempStr = "";
        foreach (PlayerScore p in playerScores)
        {
            tempStr += p.playerScore + " ";
        }
        Debug.Log(tempStr);

        for (int i = 0; i < scoreCount; i++)
        {
            if (i >= playerScores.Count) break;
            PlayerPrefs.SetString("Score" + i, playerScores[i].GetScoreAsString());
        }
    }
}

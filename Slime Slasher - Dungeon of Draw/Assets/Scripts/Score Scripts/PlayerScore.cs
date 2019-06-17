using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore
{
    public string playerName;
    public int playerScore;
    public PlayerScore(string pName, int pScore)
    {
        this.playerName = pName;
        this.playerScore = pScore;
    }
    public string GetScoreAsString()
    {
        return playerName + "\t\t" + playerScore;
    }
}
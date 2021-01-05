using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience: MonoBehaviour
{
    private ulong exp;
    private ulong maxExp;
    private int level;
    private ulong[] expArray = ExpPerLevel.instance.GetExpArray();

    private void Start()
    {
        
    }

    public Experience(ulong exp){
        this.exp = exp;
        UpdateValues();
    }

    public ulong GetExpValue(){
        return this.exp;
    }

    public void SetExpValue(ulong exp){
        this.exp = exp;
        UpdateValues();
    }

    public ulong GetMaxExp(){
        return this.maxExp;
    }

    public int GetLevel(){
        return this.level;
    }

    private void UpdateValues(){
        int current_level = this.level;
        ulong current_exp = this.exp;
        //expArray = ExpPerLevel.instance.GetExpArray();
        for (int i = 1; i <= 200; i++)
        {
            if (current_exp >= expArray[200])
            {
                current_level = 200;
                break;
            }
            else if (current_exp >= expArray[i - 1] && current_exp < expArray[i])
            {
                current_level = i; // At first, you are at level 1. To get level 2, you must gain "expArray[1]" exp.
                break;
            }
        }
        this.level = current_level;
        this.maxExp = expArray[current_level];
        //Debug.Log("Exp: " + this.exp + ", Level: " + this.level + ", Max exp.: " + this.maxExp);
    }
}

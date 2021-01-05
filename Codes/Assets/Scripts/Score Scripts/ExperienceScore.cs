using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceScore : MonoBehaviour {

    private static ulong[] expArray;
    private static float expBarValue;
    private static ulong numerator, denominator;
    private static ulong expGained = 0;
    private static int current_score = 0;

    private static Experience experience;

    public Slider expBar;
    public Text expText;
    public Text levelText;
    public Text descriptionText;
    public Animator animator;

	void Start () {
        expArray = ExpPerLevel.instance.GetExpArray();
        experience = new Experience(0);
        //FillListExp();
        ResetExpAndLevel();
        expGained = GainExp(140);
        GainLevel();
        expBarValue = CalcExpBarValue();
	}

    private void Update() {
        //int oldExpLevel = GetCurrentExp().GetLevel();
        float oldExpBarValue = expBar.value;

        if(Input.GetKeyDown(KeyCode.P)){
            expGained = GainExp(500);
            GainLevel();
            expBarValue = CalcExpBarValue();
        }

        if(!GameManager.score.Equals(null)) current_score = GameManager.score;
        descriptionText.text = "From this round\nYou get " + current_score + " exp";
        expText.text = numerator.ToString() + " / " + denominator.ToString();
        levelText.text = "Level " + GetCurrentLevel();

        // Update ExpBar
        float t = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        expBar.value += (expBarValue - oldExpBarValue) * t;
        Debug.Log(t);

        //ulong deltaValue;
        //while(expGained > 0){
        //    ulong maxExpValue = expArray[oldExpLevel] - expArray[oldExpLevel - 1];
        //    deltaValue = maxExpValue - oldExpValue;

        //    expBar.value += ((float)deltaValue / (float)maxExpValue) * t;

        //    if(expGained > maxExpValue){
        //        expGained -= deltaValue;
        //        oldExpLevel += 1;
        //        oldExpValue = 0;
        //    } else {
        //        expGained = 0;
        //        oldExpValue = expGained;
        //    }

        //    Debug.Log("maxExpValue: " + maxExpValue);
        //}
        //StartCoroutine(UpdateExpBar());
    }

    private IEnumerator UpdateExpBar(){
        int oldExpLevel = GetCurrentExp().GetLevel();
        ulong oldExpValue = GetCurrentExp().GetExpValue();

        if (Input.GetKeyDown(KeyCode.P))
        {
            expGained = GainExp(500);
            GainLevel();
            expBarValue = CalcExpBarValue();
        }

        float t = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        expBar.value += expBarValue * t;
        yield return new WaitForEndOfFrame();

    }

    private static float CalcExpBarValue(){
        Experience expObject = GetCurrentExp();
        ulong exp = expObject.GetExpValue();
        int level = expObject.GetLevel();
        ulong maxExp = expObject.GetMaxExp();
        numerator = exp - expArray[level - 1];
        denominator = maxExp - expArray[level - 1];
        float percent = ((float)numerator)/((float)denominator);

        Debug.Log("maxExp: " + denominator);
        Debug.Log("exp: " + numerator);
        Debug.Log("percent: " + percent);
        return percent;
    }

    public static Experience GetCurrentExp(){
        string exp_str = PlayerPrefs.GetString("Experience", "0");
        ulong exp = System.UInt64.Parse(exp_str);
        experience.SetExpValue(exp);
        return experience;
    }

    public static ulong GainExp(ulong score){
        Experience expObject = GetCurrentExp();
        ulong newExp = expObject.GetExpValue() + score;
        expObject.SetExpValue(newExp);
        PlayerPrefs.SetString("Experience", newExp.ToString());
        return score;
    }

    public static void ResetExpAndLevel(){
        PlayerPrefs.DeleteKey("Experience");
        PlayerPrefs.DeleteKey("Level");
    }

    public static int GetCurrentLevel(){
        return PlayerPrefs.GetInt("Level", 1);
    }

    private static int GainLevel(){
        int current_level = GetCurrentExp().GetLevel();
        PlayerPrefs.SetInt("Level", current_level);
        return current_level;
    }

    //private static void FillListExp(){
    //    ulong exp = 0;
    //    ulong killsPerGame = 100;

    //    expArray[0] = 0;
    //    for (ulong i = 1; i <= 200; i++) {
    //        exp += (ulong)(i * killsPerGame * gamesToLevelUp(i));
    //        expArray[i] = exp;
    //    }
    //}

    //private static ulong gamesToLevelUp(ulong k){
    //    ulong result = 1;
    //    ulong a;
    //    double ke = (double)k;
    //    a = (ulong)System.Math.Ceiling(ke / 5.0);
    //    for (ulong i = 0; i < a; i++)
    //    {
    //        result *= 2;
    //    }
    //    //
    //    return result;
    //}  
}

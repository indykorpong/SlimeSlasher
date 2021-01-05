using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour {
    public GameObject pauseButton;

#if UNITY_IOS
    void Awake() {
        Advertisement.Initialize("2656494"); // your iOS AppStore game ID
    }
#endif

#if UNITY_ANDROID
    void Awake() {
        Advertisement.Initialize("2656493"); // your Google Playstore game ID
    }
#endif



    public void ShowRewardedAd() {
        Debug.Log(string.Format("Platform is {0}supported\nUnity Ads {1}initialized", Advertisement.isSupported ? "" : "not ", Advertisement.isInitialized ? "" : "not "));
        const string RewardedPlacementId = "rewardedVideo";


        if (!Advertisement.IsReady(RewardedPlacementId)) {
            Debug.Log(string.Format("Ads not ready for placement '{0}'", RewardedPlacementId));
            return;
        }

        var options = new ShowOptions { resultCallback = HandleShowResult };
        Advertisement.Show(RewardedPlacementId, options);

    }

    public void ShowVideoAd() {
        Debug.Log(string.Format("Platform is {0}supported\nUnity Ads {1}initialized", Advertisement.isSupported ? "" : "not ", Advertisement.isInitialized ? "" : "not "));
        const string RewardedPlacementId = "video";


        if (!Advertisement.IsReady(RewardedPlacementId)) {
            Debug.Log(string.Format("Ads not ready for placement '{0}'", RewardedPlacementId));
            return;
        }

        var options = new ShowOptions { resultCallback = HandleResultForNoRewards };
        Advertisement.Show(RewardedPlacementId, options);

    }


    private void HandleShowResult(ShowResult result) {
        switch (result) {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                GameManager.Revive();
                //advertisementEvent.Invoke(); //This is my reward invoke function
                //YOUR CODE TO REWARD THE GAMER
                // Give coins etc.
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }

    private void HandleResultForNoRewards(ShowResult result) {
        switch (result) {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown and game is now running.");
                //GameManager.Revive();
                //advertisementEvent.Invoke(); //This is my reward invoke function
                //YOUR CODE TO REWARD THE GAMER
                // Give coins etc.
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end and game is now running.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown and game is now running.");
                break;
        }
        pauseButton.SetActive(true);
        GameManager.PlayAgain();
        
    }

}

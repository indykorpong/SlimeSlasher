using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Runtime.InteropServices;

public class Share : MonoBehaviour
{
    public enum ShareType {
        WithNormalBlob,
        WithKillerBlob,
        WithRandomBlob
    }
    private bool isProcessing = false;
    public Canvas mainCanvas;
    public string shareText;
    public Text scoreText;
    private string gameLink = "\nDownload the game on iOS or Google Play today!" + "\niOS: (iOS link here)" + "\nGoogle Play: (Google Play link here)";
    public string subject;
    public Image background;
    public Image slime;
    public Sprite shareImageWithNoSlime;
    public Sprite shareImageWithSlime;
    public Sprite[] slimeSprite;
    public ShareType shareType;

    private void OnEnable()
    {
        if (!isProcessing)
            StartCoroutine(ShareScreenshot());
        switch (shareType) {
            case ShareType.WithNormalBlob:
                background.sprite = shareImageWithSlime;
                slime.enabled = false;
                break;
            case ShareType.WithKillerBlob:
                background.sprite = shareImageWithNoSlime;
                slime.enabled = true;
                slime.sprite = slimeSprite[(int) GameManager.killerType];
                break;

            case ShareType.WithRandomBlob:
                background.sprite = shareImageWithNoSlime;
                slime.sprite = slimeSprite[Random.Range(0, slimeSprite.Length)];
                break;
        }
    }

    private IEnumerator ShareScreenshot()
    {
        scoreText.text = GameManager.score.ToString();
        mainCanvas.gameObject.SetActive(false);
        isProcessing = true;
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.width, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, (Screen.height - Screen.width)/2, Screen.width, Screen.width), 0, 0);
        ss.Apply();

        string filePath = Path.Combine(Application.persistentDataPath, "shared img.png");
        //ScreenCapture.CaptureScreenshot
        File.WriteAllBytes(filePath, ss.EncodeToPNG());
        DelayedShareImage(filePath);
        Debug.Log(filePath);
        //Texture2D screenTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
        //screenTexture.Apply();

        //byte[] dataToSave = Resources.Load<TextAsset>(imageName).bytes;

        //string destination = Path.Combine(Application.persistentDataPath, System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".png");
        //Debug.Log(destination);
        //File.WriteAllBytes(destination, dataToSave);

        isProcessing = false;
        this.gameObject.SetActive(false);
        mainCanvas.gameObject.SetActive(true);

    }

    private void DelayedShareImage(string filePath){
        StartCoroutine(WaitForSharedImage(filePath));
    }

    private IEnumerator WaitForSharedImage(string filePath)
    {
        while (!File.Exists(filePath))
        {
            yield return new WaitForSeconds(0.05f);
        }
        Debug.Log("fakkayu!");
#if UNITY_ANDROID
        if (!Application.isEditor)
        {

            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string> ("ACTION_SEND"));
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + filePath);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), shareText + gameLink);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);
            intentObject.Call<AndroidJavaObject>("setType", "image/png");
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

            currentActivity.Call("startActivity", intentObject);

        }
#elif UNITY_IOS
        CallSocialShareAdvanced(shareText, subject, "", filePath);
#endif
    }
#if UNITY_IOS
    public struct ConfigStruct
    {
        public string title;
        public string message;
    }

    [DllImport("__Internal")] private static extern void showAlertMessage(ref ConfigStruct conf);

    public struct SocialSharingStruct
    {
        public string text;
        public string url;
        public string image;
        public string subject;
    }

    [DllImport("__Internal")] private static extern void showSocialSharing(ref SocialSharingStruct conf);

    public static void CallSocialShare(string title, string message)
    {
        ConfigStruct conf = new ConfigStruct();
        conf.title = title;
        conf.message = message;
        showAlertMessage(ref conf);
    }


    public static void CallSocialShareAdvanced(string defaultTxt, string subject, string url, string img)
    {
        SocialSharingStruct conf = new SocialSharingStruct();
        conf.text = defaultTxt;
        conf.url = url;
        conf.image = img;
        conf.subject = subject;

        showSocialSharing(ref conf);
    }
#endif
}
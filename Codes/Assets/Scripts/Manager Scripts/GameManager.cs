using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
//using UnityEngine.SceneManagement;

//Control the entire flow of the main game
public class GameManager : MonoBehaviour {

    public class UnityE_Int : UnityEvent<int> { }
    public class UnityE_Bool : UnityEvent<bool> { }
    public class UnityE_Float : UnityEvent<float> { }

    public AdsManager adsManagerObject;
    //public CameraShake cameraShakeObject;

    public static AudioSource hurt;
    public static AudioSource recordScratch;
    public static AudioSource shieldBreak;
    public static AudioSource comboUp;

    public static GameManager instance = null;

    public static bool gameIsRunning { get { return _gameIsRunning; } set { _gameIsRunning = value; IsGameRunning(value); } }
    private static bool _gameIsRunning;
    public delegate void currentGameState(bool isRunning);
    public static event currentGameState IsGameRunning;

    public static bool hellcourse { get { return _hellcourse; } set { _hellcourse = value; HellCourseValue(value); } }
    private static bool _hellcourse;
    public delegate void hellcourseValue(bool isHell);
    public static event hellcourseValue HellCourseValue;

    public static bool shielded { get { return _shielded; } set { _shielded = value; IsUserShielded(value); } }
    private static bool _shielded;
    public delegate void CurrentlyShielded(bool isShielded);
    public static event CurrentlyShielded IsUserShielded;

    public static bool adWatchedThisRound;
    public static bool hitModeChangeBlob = false; //just added

    public static int maxLives;

    public static int lives { get { return _lives; } set { _lives = value; LivesRemain(value); } }
    private static int _lives;
    public delegate void LifeCountRemain(int lifeCount);
    public static event LifeCountRemain LivesRemain;

    public static int kills { get { return _kills; } set { _kills = value; if (CurrentKillCount != null) CurrentKillCount(value); } }
    private static int _kills;
    public delegate void KillCountChanges(int killC);
    public static event KillCountChanges CurrentKillCount;

    public static int chain { get { return _chain; } set { _chain = value; OnChainChange.Invoke(value); } }
    private static int _chain;
    public static UnityE_Int OnChainChange = new UnityE_Int();


    public static int score;
    public static int multiplierFromBlob;
    private static int oldMultiplier;
    public static int targetFrameRate = 60;
    public static List<int> killsToLevels = new List<int> { 10, 25, 50, 75, 100 };

    public static Image life1;
    public static Image life2;
    public static Image life3;
    //private static Image lifePointImage; //just added
    //private static Image lifePointImage2;

    //public static SpriteRenderer hellBG;

    //private static DrawingManager dm;
    private static DataController dc;

    private static GameObject death;

    private static Button tryAgainButton;
    //private static Button submitButton;
    private static Button watchAdButton;
    private static Button giveUpButton;
    private static Button shareButton;
    public GameObject pauseButton;

    //private static InputField submitName;
    private static Button mainMenuButton;

    private static GameObject mainMenuBtnObj;
    private static Vector3 mainMenuPosition;
    private static Vector3 initiatedMenuPosition;

    private static bool kbShown;
    //private VirtualKeyboard vk;
    public bool debugKeysEnabled;

    public static bool tapEnabled { get { return _tapEnabled; } set { _tapEnabled = value; OnTapStateChange.Invoke(value); } }
    private static bool _tapEnabled;
    public static UnityE_Bool OnTapStateChange = new UnityE_Bool();

    private static Text scoreBoard;
    private static Text multiplierBoard;
    private static Text scoreText;
    public static MonsterSpawner.MonsterType killerType;

    //public UnityEvent continueAfterDeath;


    public enum GameState {
        gameStarted,
        gameOver
    }

    //1: game started or restarted, 2: game over
    public static GameState gameState { get { return _gameState; } set { _gameState = value; CurrentGameState(value); } }
    private static GameState _gameState;
    public delegate void gameStateValue(GameState g);
    public static event gameStateValue CurrentGameState;


    private void Awake() {
        dc = DataController.instance;
        //dm = FindObjectOfType<DrawingManager>();
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(gameObject);

        maxLives = 3;
        oldMultiplier = 1;

        //score = 0;
        //kills = 0;
        //lives = maxLives;
        //hellBG.enabled = false;
        //gameIsRunning = true;
        //death.SetActive(false);
        //submitName.gameObject.SetActive(false);
        //submitButton.gameObject.SetActive(false);

        scoreBoard = GameObject.Find("ScoreDisplay").GetComponent<Text>();
        //multiplierBoard = GameObject.Find("MultiplierDisplay").GetComponent<Text>();

        life1 = GameObject.Find("life1").GetComponent<Image>();
        life2 = GameObject.Find("life2").GetComponent<Image>();
        life3 = GameObject.Find("life3").GetComponent<Image>();
        //lifePointImage = GameObject.Find("Life Points").GetComponent<Image>();

        //hellBG = GameObject.Find("HellScreen").GetComponent<SpriteRenderer>();

        death = GameObject.Find("Death Package");

        recordScratch = GameObject.Find("RecordScratcher").GetComponent<AudioSource>();
        //recordScratch.volume = PlayerPrefs.GetFloat("sfxVolume", 1);
        hurt = GameObject.Find("PlayerTakeDamage").GetComponent<AudioSource>();
        //hurt.volume = PlayerPrefs.GetFloat("sfxVolume", 1);
        shieldBreak = GameObject.Find("ShieldBreak").GetComponent<AudioSource>();
        comboUp = GameObject.Find("ComboUp").GetComponent<AudioSource>();

        tryAgainButton = GameObject.Find("Try Again Button").GetComponent<Button>();
        //submitName = GameObject.Find("SubmitName").GetComponent<InputField>();
        //submitButton = GameObject.Find("SubmitButton").GetComponent<Button>();
        mainMenuButton = GameObject.Find("Main Menu Button").GetComponent<Button>();
        watchAdButton = GameObject.Find("WatchAdButton").GetComponent<Button>();
        giveUpButton = GameObject.Find("GiveUpButton").GetComponent<Button>();
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        shareButton = GameObject.Find("Share Button").GetComponent<Button>();

        //mainMenuBtnObj = mainMenuButton.gameObject;
        //mainMenuPosition = mainMenuBtnObj.transform.position;
        //initiatedMenuPosition = mainMenuBtnObj.transform.position;

        //tryAgainButton.onClick.AddListener(PlayAgain);
        //submitButton.onClick.AddListener(SubmitScore);
        mainMenuButton.onClick.AddListener(MainMenuClicked);
    }

    private void OnEnable() {
        OnTapStateChange.AddListener(OnTapperMonster);
    }

    private void OnDisable() {
        OnTapStateChange.RemoveListener(OnTapperMonster);
    }

    public void OnTapperMonster(bool isEnabled) {
        if (isEnabled) {
            if (startTimerRef != null) StopCoroutine(startTimerRef);
            startTimerRef = startTimer();
            StartCoroutine(startTimerRef);
        }
    }
    public static float maxTapperTime = 10f;
    public static float internalClock_tapper {
        get { return _internalClock_tapper; }
        set { _internalClock_tapper = value; OnTapperClock.Invoke(value); }
    }
    public static float _internalClock_tapper;
    public static UnityE_Float OnTapperClock = new UnityE_Float();

    private IEnumerator startTimerRef;
    private IEnumerator startTimer() {
        KillAllMonsters(true);
        internalClock_tapper = maxTapperTime;
        while (internalClock_tapper > 0f) {

            // Timer

            if (!Pause.gamePaused) internalClock_tapper -= Time.unscaledDeltaTime;
            yield return null;
        }
        internalClock_tapper = 0f;
        KillAllMonsters(true);
        tapEnabled = false;

        // After timer
    }

    private void Start() {
        tapEnabled = false;
        //vk = new VirtualKeyboard();
        kbShown = false;
        SetGameState(GameState.gameStarted);
        SetLifePoints();
        UpdateScore();
        SetTargetFrameRate();
    }

    private void Update() {
        //if (hellcourse) {
        //    Time.timeScale = 1.5f;
        //    hellBG.enabled = true;
        //} else {
        //    Time.timeScale = 1;
        //    hellBG.enabled = false;
        //}
        if (!Pause.gamePaused) {
            if (hellcourse) {
                Time.timeScale = 1.5f;
                //hellBG.enabled = true;
            } else {
                Time.timeScale = 1;
                //hellBG.enabled = false;
            }
        } else {
            Time.timeScale = 0f;
        }
        //if (submitName.isFocused && !kbShown)
        //{
        //    //vk.ShowTouchKeyboard();
        //    kbShown = true;
        //}
        //else if (!submitName.isFocused && kbShown)
        //{
        //    //vk.HideTouchKeyboard();
        //    kbShown = false;
        //}

        //Debug.Log(hitModeChangeBlob);

        //debug stuff onwards, please remove before release
        if (gameIsRunning && debugKeysEnabled) {
            //Take Damage (D)
            if (Input.GetKeyDown(KeyCode.D)) {
                LoseLife();
            }

            //Recover Health (R)
            if (Input.GetKeyDown(KeyCode.R)) {
                lives = maxLives;
            }

            //Clear Screen (C)
            if (Input.GetKeyDown(KeyCode.C)) {
                KillAllMonsters(false);
            }

            //Hell/Unhell (H)
            if (Input.GetKeyDown(KeyCode.H)) {
                SetHell(!hellcourse);
            }
            //Toggle Tap Mode (T)
            if (Input.GetKeyDown(KeyCode.T)) {
                tapEnabled = !tapEnabled;
            }
            //Toggle Shield (S)
            if (Input.GetKeyDown(KeyCode.S)) {
                shielded = !shielded;
            }
            //Sets kills to the kill cap referenced in Monster so the game runs at max speed (K)
            if (Input.GetKeyDown(KeyCode.K)) {
                kills = Monster.killCap;
                UpdateScore();
            }
        }
    }

    private static void SetGameState(GameState state) {
        switch (state) {
            case GameState.gameStarted:
                gameIsRunning = true;
                gameState = GameState.gameStarted;
                multiplierFromBlob = 0;
                //oldMultiplier = 1;
                hellcourse = false;
                hitModeChangeBlob = false;

                scoreBoard.enabled = true;
                //multiplierBoard.enabled = true;
                death.SetActive(false);
                adWatchedThisRound = false;
                //tapEnabled = false;


                //musicController.GetComponentInChildren<AudioSource>().Stop();
                //musicController.GetComponentInChildren<AudioSource>().Play();

                //submitName.gameObject.SetActive(false);
                //submitButton.gameObject.SetActive(false);
                tryAgainButton.gameObject.SetActive(false);
                watchAdButton.gameObject.SetActive(false);
                giveUpButton.gameObject.SetActive(false);
                shareButton.gameObject.SetActive(false);


                //mainMenuPosition = initiatedMenuPosition;
                //mainMenuBtnObj.transform.SetPositionAndRotation(mainMenuPosition, Quaternion.identity);

                //lifePointImage.sprite = dm.lifePoint1;
                break;
            case GameState.gameOver:
                gameIsRunning = false;
                gameState = GameState.gameOver;
                hellcourse = false;
                //tapEnabled = false;
                scoreBoard.enabled = false;
                //multiplierBoard.enabled = false;
                death.SetActive(true);
                if (adWatchedThisRound || score <= 100) {
                    SubmitScore();
                    ShowAdButtons(false);
                    //ShowSubmitButtons(false);
                    ShowTryAgainButtons(true);
                } else {
                    ShowTryAgainButtons(false);
                    //ShowSubmitButtons(false);
                    ShowAdButtons(true);
                }


                //lifePointImage.sprite = dm.lifePoint2;
                break;
        }
    }

    private static void SetLifePoints() {
        if (gameState == GameState.gameStarted) {
            kills = 0;
            chain = 0;
            score = 0;
            multiplierFromBlob = 0;
            oldMultiplier = 1;
            shielded = false;
            lives = maxLives;
        }
    }

    private static void GameOver() {
        recordScratch.Play();
        KillAllMonsters();

        SetGameState(GameState.gameOver);

        //gameIsRunning = false;
        //hellcourse = false;
        //submitName.gameObject.SetActive(true);
        //submitButton.gameObject.SetActive(true);
        //tryAgainButton.gameObject.SetActive(false);
        //scoreBoard.enabled = false;
        //multiplierBoard.enabled = false;
        //death.SetActive(true);

        //submitName.ActivateInputField();

        string scoreString = score.ToString();
        scoreText.text = scoreString;
        Debug.Log("お前はもう死んでいる");

        //death.GetComponentInChildren<Text>().text = gameOverText;
    }

    public static void PlayAgain() {
        //death.SetActive(false);
        //gameIsRunning = true;
        //scoreBoard.enabled = true;
        //multiplierBoard.enabled = true;
        //kills = 0;
        //chain = 0;
        //score = 0;
        //lives = maxLives;
        SoundManager.musicInitialized = false;
        SetGameState(GameState.gameStarted);
        SetLifePoints();
        UpdateScore();
        //UpdateLives();
    }


    public void PlayAgainWithAds(float chance) {
        //death.SetActive(false);
        //gameIsRunning = true;
        //scoreBoard.enabled = true;
        //multiplierBoard.enabled = true;
        //kills = 0;
        //chain = 0;
        //score = 0;
        //lives = maxLives;
        if (Random.Range(0f, 1f) <= chance) {
            Time.timeScale = 0;
            pauseButton.SetActive(false);
            adsManagerObject.ShowVideoAd();
        } else {
            PlayAgain();
        }
        //UpdateLives();
    }

    private static void MainMenuClicked() {
        SoundManager.musicInitialized = false;
        scoreBoard.enabled = true;
        //multiplierBoard.enabled = true;

        gameState = GameState.gameStarted;
        multiplierFromBlob = 0;
        hellcourse = false;
        hitModeChangeBlob = false;

        SetLifePoints();
        UpdateScore();
        lives = 0;

        Debug.Log("Main Menu clicked");
    }

    public ObjectShake objectShake;
    public void LoseLife() {
        if (tapEnabled) return;

        objectShake.ShakeObject();

        //cameraShakeObject.shakeDuration = 0.1f;
        if (shielded) {
            shielded = false;
            shieldBreak.Play();
        } else {
            if (hellcourse) {
                hellcourse = false;
                foreach (GameItem i in FindObjectsOfType<GameItem>()) {
                    if (i.name == "Heavenblob") {
                        i.gameObject.SetActive(false);
                    }
                }
            }
            lives--;
            hurt.Play();
            chain = 0;
            multiplierFromBlob = 0;
            if (lives == 0) {
                GameOver();
            }
            //UpdateLives();
            UpdateScore();
        }
    }

    /*public static void UpdateLives() {
        switch (lives) {
            case 3:
                life1.sprite = dm.fullHeart;
                life2.sprite = dm.fullHeart;
                life3.sprite = dm.fullHeart;
                break;
            case 2:
                life1.sprite = dm.fullHeart;
                life2.sprite = dm.fullHeart;
                life3.sprite = dm.deadHeart;
                break;
            case 1:
                life1.sprite = dm.fullHeart;
                life2.sprite = dm.deadHeart;
                life3.sprite = dm.deadHeart;
                break;
            case 0:
                life1.sprite = dm.deadHeart;
                life2.sprite = dm.deadHeart;
                life3.sprite = dm.deadHeart;
                break;
            default:
                break;

        }
    }*/

    private static void SubmitScore() {
        //submitName.DeactivateInputField();
        //ShowSubmitButtons(false);
        //ShowTryAgainButtons(true);


        //mainMenuPosition.y = initiatedMenuPosition.y - 0.5f;
        //mainMenuBtnObj.transform.SetPositionAndRotation(mainMenuPosition, Quaternion.identity);

        //var name = submitName.text;
        //if (name == "")
        //{
        //    name = "Anonymous";
        //}

        dc.SavePlayerProgress("Anonymous", score);
        //submitted = true;
        //SetActiveItems();
    }

    public static void SetHell(bool hell) {
        hellcourse = hell;
        SoundManager.musicInitialized = false;
        UpdateScore();
        //KillAllMonsters();
        foreach (GameItem i in FindObjectsOfType<GameItem>()) {
            if ((i.name == "Heavenblob" && !hellcourse) || (i.name == "Hellblob" && hellcourse)) {
                i.gameObject.SetActive(false);
            }
        }

        hitModeChangeBlob = true;
    }

    public static int MultiplierValue { get { return _currentMultiplier; } set {
            if (value == _currentMultiplier) return;
            _currentMultiplier = value;
            currMultiplier.Invoke(value); } }
    private static int _currentMultiplier = 0;
    public static UnityE_Int currMultiplier = new UnityE_Int();

    public static void UpdateScore() {
        scoreBoard.text = "Kills: " + kills + "\nScore: " + score + "\nCombo: " + chain;

        //multiplierBoard.text = "x" + CurrentMultiplier();
        MultiplierValue = CurrentMultiplier();

        if (CurrentMultiplier() != oldMultiplier) {
            if (CurrentMultiplier() > oldMultiplier) comboUp.Play();
            oldMultiplier = CurrentMultiplier();
        }

        //if (hellcourse) {
        //    scoreBoard.color = Color.white;
        //    multiplierBoard.color = Color.white;
        //} else {
        //    scoreBoard.color = Color.white;
        //    multiplierBoard.color = Color.white;
        //}
    }

    public static void IncrementScore() {
        int hellMultiplier = hellcourse ? 2 : 1;
        score += hellMultiplier * CurrentMultiplier();
        kills++;
        chain++;
        UpdateScore();
    }

    public static void IncrementScore(int value) {
        score += CurrentMultiplier() * value;
        kills++;
        chain++;
        UpdateScore();
    }

    public static int CurrentMultiplier() {
        int multiplier = (chain / 5) + 1;
        if (multiplier > 16) multiplier = 16;
        if (multiplier < 1) multiplier = 1;
        multiplier += multiplierFromBlob;
        if (hellcourse) multiplier *= 2;
        //multiply the level here
        return multiplier;
    }

    public static void KillAllMonsters() {
        //GameObject[] MonsterObjectList = GameObject.FindGameObjectsWithTag("Monsters");
        var MonsterObjectList = FindObjectsOfType<Monster>();
        foreach (Monster m in MonsterObjectList) {
            Debug.Log("Destroyed monsters");
            m.gameObject.SetActive(false);
        }

        //GameObject[] GameItemObjectList = GameObject.FindGameObjectsWithTag("GameItem");
        var GameItemObjectList = FindObjectsOfType<GameItem>();
        foreach (GameItem sm in GameItemObjectList) {
            Debug.Log("Destroyed Sideways monsters");
            sm.gameObject.SetActive(false);
            //Destroy(sm.gameObject);
        }
    }

    public static void KillAllMonsters(bool explode) {
        //GameObject[] MonsterObjectList = GameObject.FindGameObjectsWithTag("Monsters");
        var MonsterObjectList = FindObjectsOfType<Monster>();
        foreach (Monster m in MonsterObjectList) {
            if (explode) Instantiate(m.myExplosion, m.transform.position, Quaternion.identity);
            Debug.Log("Destroyed monsters");
            m.gameObject.SetActive(false);
        }

        //GameObject[] GameItemObjectList = GameObject.FindGameObjectsWithTag("GameItem");
        var GameItemObjectList = FindObjectsOfType<GameItem>();
        foreach (GameItem sm in GameItemObjectList) {
            Debug.Log("Destroyed Sideways monsters");
            sm.gameObject.SetActive(false);
            //Destroy(sm.gameObject);
        }
    }

    public void GiveUp() {
        SubmitScore();
        adWatchedThisRound = true;
        ShowAdButtons(false);
        ShowTryAgainButtons(true);
        //ShowSubmitButtons(true);

    }

    public static void Revive() {
        SoundManager.musicInitialized = false;
        lives = maxLives;
        KillAllMonsters(false);
        SetGameState(GameState.gameStarted);
        adWatchedThisRound = true;
    }

    public static void ShowAdButtons(bool show) {
        giveUpButton.gameObject.SetActive(show);
        watchAdButton.gameObject.SetActive(show);
    }

    //public static void ShowSubmitButtons(bool show)
    //{
    //    submitName.gameObject.SetActive(show);
    //    submitButton.gameObject.SetActive(show);
    //    mainMenuButton.gameObject.SetActive(show);
    //}

    public static void ShowTryAgainButtons(bool show) {
        tryAgainButton.gameObject.SetActive(show);
        mainMenuButton.gameObject.SetActive(show);
        shareButton.gameObject.SetActive(show);
    }

    public static void SetTargetFrameRate() {
        if (!Application.targetFrameRate.Equals(targetFrameRate)) {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = targetFrameRate;
        }
    }

    public static float HellModeMultiplier() {
        if (hellcourse) return 1.5f;
        else return 1;
    }
}

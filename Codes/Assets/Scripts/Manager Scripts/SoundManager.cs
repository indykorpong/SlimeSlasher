using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    AudioSource[] sources;
    public static SoundManager _instance = null;
    public AudioMixerSnapshot playing;
    public AudioMixerSnapshot paused;

    public float m_toPause;
    public float m_toPlay;

    private static SoundManager smInstance;

    public static bool musicInitialized;

    public AudioSource normalBGM;
    public AudioSource hellBGM;
    public AudioSource menuBGM;

    public AudioSource incorrectSound;
    public AudioSource shieldSound;
    public AudioSource[] weakDeathSounds;
    public AudioSource[] mediumDeathSounds;
    public AudioSource[] strongDeathSounds;
    public AudioSource[] monsterHurtSounds;

    public bool showDebugMsg = false;

    private void Awake()
    {
        /*DontDestroyOnLoad(this);
        if (smInstance == null) {
            smInstance = this;
        } else {
            Destroy(gameObject);
        }*/
        sources = GetComponentsInChildren<AudioSource>();
        //normalBGM = GameObject.Find("NormalModeBGM").GetComponent<AudioSource>();
        //hellBGM = GameObject.Find("HellModeBGM").GetComponent<AudioSource>();
        currentBGMRef = normalBGM;
    }
    private void OnEnable()
    {
        GameManager.HellCourseValue += IsHellCourse;
        GameManager.CurrentGameState += CurrentGameStatus;
        GameManager.IsGameRunning += IsGameRunning;
        GameManager.IsUserShielded += IsUserShielded;
        Pause.IsGamePaused += IsGamePaused;
        SoundSettings.OnSFXVolumeChange += SetSFXVolume;
        SoundSettings.OnMusicVolumeChange += SetMusicVolume;
    }
    private void OnDisable()
    {
        GameManager.HellCourseValue -= IsHellCourse;
        GameManager.CurrentGameState -= CurrentGameStatus;
        GameManager.IsGameRunning -= IsGameRunning;
        GameManager.IsUserShielded -= IsUserShielded;
        Pause.IsGamePaused -= IsGamePaused;
        SoundSettings.OnSFXVolumeChange -= SetSFXVolume;
        SoundSettings.OnMusicVolumeChange -= SetMusicVolume;
    }
    void Start()
    {
        musicInitialized = false;
        currentBGMRef = normalBGM;

        SetSFXVolume(PlayerPrefs.GetFloat("sfxVolume", 1f));
        SetMusicVolume(PlayerPrefs.GetFloat("musicVolume", 1f));
    }

    private void IsHellCourse(bool isHell)
    {
        if (showDebugMsg) Debug.Log("IsHellCourse: " + isHell.ToString());

        if (GameManager.GameState.gameOver == GameManager.gameState) return;

        currentBGMRef.Stop();
        if (isHell)
        {
            currentBGMRef = hellBGM;
        }
        else
        {
            currentBGMRef = normalBGM;
        }
        currentBGMRef.Play();
    }
    private void CurrentGameStatus(GameManager.GameState g)
    {
        if (showDebugMsg) Debug.Log("game over: " + g.ToString());
        if (g == GameManager.GameState.gameOver)
        {
            hellBGM.Stop();
            normalBGM.Stop();
        }
        else if (g == GameManager.GameState.gameStarted && !musicInitialized)
        {
            hellBGM.Stop();
            normalBGM.Stop();
            currentBGMRef.time = 0;
            currentBGMRef.Play();
            musicInitialized = true;
        }
    }
    private void IsGameRunning(bool isRunning)
    {
        if (showDebugMsg) Debug.Log("IsGameRunning: " + isRunning.ToString());
        if (isRunning)
        {
            currentBGMRef.UnPause();
            //playing.TransitionTo(m_toPlay);
        }
        else
        {
            currentBGMRef.Pause();
            //paused.TransitionTo(m_toPause);
        }
    }
    private void IsGamePaused(bool isPaused)
    {
        if (showDebugMsg) Debug.Log("IsGamePaused: " + isPaused.ToString());
        if (isPaused)
        {
            currentBGMRef.Pause();
            //paused.TransitionTo(m_toPause);
        }
        else
        {
            currentBGMRef.UnPause();
            //playing.TransitionTo(m_toPlay);
        }
    }
    private void SetSFXVolume(float vol)
    {
        foreach (AudioSource audiso in sources)
        {
            if (audiso.tag == "sfx")
            {
                audiso.volume = vol;
            }
        }
    }
    private void SetMusicVolume(float vol)
    {
        foreach (AudioSource audiso in sources)
        {
            if (audiso.tag == "bgm")
            {
                audiso.volume = vol;
            }
        }
    }
    private void IsUserShielded(bool shield) {
        if (shield) shieldSound.Play();
    }

    public AudioSource RandomDeathSound(int intensity) {
        AudioSource[] deathSoundList;
        switch (intensity) {
            case 0:
                deathSoundList = weakDeathSounds;
                break;
            case 1:
                deathSoundList = mediumDeathSounds;
                break;

            case 2:
                deathSoundList = strongDeathSounds;
                break;
            default:
                deathSoundList = RandomSoundList();
                break;
        }

        return deathSoundList[Random.Range(0, deathSoundList.Length)];
    }
    public AudioSource RandomDeathSound() {
        return RandomDeathSound(Random.Range(0, 3));
    }
    public AudioSource RandomHurtSound() {
        return monsterHurtSounds[Random.Range(0, monsterHurtSounds.Length)];
    }
    public AudioSource[] RandomSoundList() {
        switch (Random.Range(0, 3)) {
            case 0:
                return weakDeathSounds;
            case 1:
                return mediumDeathSounds;
            case 2:
                return strongDeathSounds;
            default:
                return weakDeathSounds;
        }
    }
    private AudioSource currentBGMRef;
    //void Update()
    //{
    //    // Change Audio base on current mode
    //    //if (GameManager.hellcourse)
    //    //{
    //    //    currentBGMRef = hellBGM;
    //    //}
    //    //else
    //    //{
    //    //    currentBGMRef = normalBGM;
    //    //}

    //    // Stop audio when game is paused
    //    //if (GameManager.gameState == GameManager.GameState.gameOver)
    //    //{
    //    //    hellBGM.Stop();
    //    //    normalBGM.Stop();
    //    //}

    //    // Stop audio when isn't in game
    //    //if (GameManager.gameState == GameManager.GameState.gameStarted && !musicInitialized)
    //    //{
    //    //    hellBGM.Stop();
    //    //    normalBGM.Stop();
    //    //    currentBGMRef.time = 0;
    //    //    currentBGMRef.Play();
    //    //    musicInitialized = true;
    //    //}

    //    // Pausing and un-pausing
    //    //if (Pause.gamePaused || !GameManager.gameIsRunning)
    //    //{
    //    //    currentBGMRef.Pause();
    //    //    //paused.TransitionTo(m_toPause);
    //    //}
    //    //else
    //    //{
    //    //    currentBGMRef.UnPause();
    //    //    //playing.TransitionTo(m_toPlay);
    //    //}

    //    //foreach (AudioSource audiso in sources)
    //    //{
    //    //    if (audiso.tag == "sfx")
    //    //    {
    //    //        audiso.volume = PlayerPrefs.GetFloat("sfxVolume", 1);
    //    //    }
    //    //    else if (audiso.tag == "bgm")
    //    //    {
    //    //        audiso.volume = PlayerPrefs.GetFloat("musicVolume", 1);
    //    //    }
    //    //}
    //}


    /*public static SoundManager instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<SoundManager>() as SoundManager;
            }
            if (_instance == null) {
                GameObject obj = new GameObject("SoundManager");
                _instance = obj.AddComponent(typeof(SoundManager)) as SoundManager;
                Debug.Log("Created SoundManager!");
            }
            return _instance;
        }
    }*/

}
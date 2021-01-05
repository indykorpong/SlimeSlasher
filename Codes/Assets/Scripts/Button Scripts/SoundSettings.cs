using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour {

    public Slider sfxSlider;
    public Slider musicSlider;
    public static float sfxVolume;
    public static float musicVolume;

	// Use this for initialization
	void Start () {
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 1f);
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 1f);

        sfxSlider.onValueChanged.AddListener(ChangeSFXVolume);
        musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
        
    }
	
	// Update is called once per frame
    void ChangeSFXVolume(float vol) {
        Debug.Log("SFX vol: "+vol);
        PlayerPrefs.SetFloat("sfxVolume", vol);
        OnSFXVolumeChange(vol);
    }
    public delegate void WhenSFXVolumeChange(float vol);
    public static event WhenSFXVolumeChange OnSFXVolumeChange;

    void ChangeMusicVolume(float vol) {
        Debug.Log("Music vol: " + vol);
        PlayerPrefs.SetFloat("musicVolume", vol);
        OnMusicVolumeChange(vol);
    }
    public delegate void WhenMusicVolumeChange(float vol);
    public static event WhenMusicVolumeChange OnMusicVolumeChange;
}

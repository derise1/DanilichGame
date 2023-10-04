using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Random=UnityEngine.Random;

public class VolumeManagerLevel : MonoBehaviour
{
    private static readonly string FirstPlay = "FirstPlay";
    private static readonly string MusicPref = "MusicPref";
    private static readonly string SoundEffectsPref = "SoundEffectsPref";
    private int firstPlayInt;
    public Slider musicSlider, soundEffectsSlider;
    private float musicFloat, soundEffectsFloat;
    public AudioSource musicAudio;
    public AudioClip[] music;
    public AudioSource soundAudioEffects;
    public AudioSource soundEffectsPlayer;
    public AudioSource soundEffectsGun;
    public AudioSource[] soundEffectsEnemy;
    public int numAudio1;
    public int numAudio2;
    
    void Start()
    {
        int a = Random.Range(numAudio1, numAudio2);
        musicAudio.PlayOneShot(music[a]);
        firstPlayInt = PlayerPrefs.GetInt(FirstPlay);

        if(firstPlayInt == 0)
        {
            musicFloat = 0.25f;
            soundEffectsFloat = 0.75f;
            musicSlider.value = musicFloat;
            soundEffectsSlider.value = soundEffectsFloat;
            PlayerPrefs.SetFloat(MusicPref, musicFloat);
            PlayerPrefs.SetFloat(SoundEffectsPref, soundEffectsFloat);
            PlayerPrefs.SetInt(FirstPlay, -1);
        }
        else
        {
            musicFloat = PlayerPrefs.GetFloat(MusicPref);
            musicSlider.value = musicFloat;
            soundEffectsFloat = PlayerPrefs.GetFloat(SoundEffectsPref);
            soundEffectsSlider.value = soundEffectsFloat;
        }
    }

    public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat(MusicPref, musicSlider.value);
        PlayerPrefs.SetFloat(SoundEffectsPref, soundEffectsSlider.value);
    }

    void OnApplicationFocus(bool inFocus)
    {
        if (!inFocus)
        {
            SaveSoundSettings();
        }
    }

    public void UpdateSound()
    {
        musicAudio.volume = musicSlider.value;
        soundAudioEffects.volume = soundEffectsSlider.value;
        soundEffectsPlayer.volume = soundEffectsSlider.value;
        soundEffectsGun.volume = soundEffectsSlider.value;

        for(int i = 0; i < soundEffectsEnemy.Length; i++){
            soundEffectsEnemy[i].volume = soundEffectsSlider.value;
        }
    }
}
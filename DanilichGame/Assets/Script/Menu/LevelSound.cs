using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSound : MonoBehaviour
{
    private static readonly string MusicPref = "MusicPref";
    private static readonly string SoundEffectsPref = "SoundEffectsPref";
    private float musicFloat, soundEffectsFloat;
    public AudioSource musicAudio;
    public AudioClip[] music;
    public AudioSource soundEffectsPlayer;
    public AudioSource soundEffectsGun;

    private void Awake()
    {
        LevelSoundSettings();
    }

    private void LevelSoundSettings()
    {
        musicFloat = PlayerPrefs.GetFloat(MusicPref);
        soundEffectsFloat = PlayerPrefs.GetFloat(SoundEffectsPref);

        musicAudio.volume = musicFloat;

        soundEffectsPlayer.volume = soundEffectsFloat;
        soundEffectsGun.volume = soundEffectsFloat;
    }
}

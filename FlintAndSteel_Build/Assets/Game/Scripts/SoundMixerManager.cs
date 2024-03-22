using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public void SetMasterVolume(float volumeLevel)
    {
        audioMixer.SetFloat("masterVolume", volumeLevel);
    }

    public void SetMusicVolume(float volumeLevel)
    {
        audioMixer.SetFloat("musicVolume", volumeLevel);
    }

    public void SetFXVolume(float volumeLevel)
    {
        audioMixer.SetFloat("soundFXVolume", volumeLevel);
    }
}

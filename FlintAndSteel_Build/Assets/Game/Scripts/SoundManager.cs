using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioSource soundObject;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        // spawn in the sound object
        AudioSource audioSource = Instantiate(soundObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlayRandomSoundClip(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
        // assign index value
        int randClip = Random.Range(0, audioClip.Length);

        // spawn in the sound object
        AudioSource audioSource = Instantiate(soundObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip[randClip]; // changed to an array so it will grab a random clip

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }
}

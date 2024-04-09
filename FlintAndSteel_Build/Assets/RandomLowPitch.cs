using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLowPitch : MonoBehaviour
{
    [SerializeField] AudioClip[] sounds;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        int soundChoice = Random.Range(0, sounds.Length);
        
        audioSource.clip = sounds[soundChoice];
        audioSource.pitch = Random.Range(0.75f, 0.9f);

        audioSource.Play();
    }

}

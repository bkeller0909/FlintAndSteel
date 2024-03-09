using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkSounds : MonoBehaviour
{
    [SerializeField] AudioClip[] clangSounds;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        int soundChoice = Random.Range(0, clangSounds.Length);
        
        audioSource.clip = clangSounds[soundChoice];
        audioSource.pitch = Random.Range(1.15f, 1.45f);

        audioSource.Play();
    }

}

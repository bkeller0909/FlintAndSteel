using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSounds : MonoBehaviour
{
    [SerializeField] AudioClip[] explosionSounds;
    [SerializeField] AudioSource audioSource;

    private void Awake()
    {
        int soundChoice = Random.Range(0, explosionSounds.Length);

        audioSource.clip = explosionSounds[soundChoice];
        audioSource.pitch = Random.Range(0.75f, 0.95f);

        audioSource.Play();
    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeAudio : MonoBehaviour
{
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();

        Destroy(gameObject, 2f);
    }
}

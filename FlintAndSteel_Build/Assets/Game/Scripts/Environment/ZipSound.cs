using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZipSound : MonoBehaviour
{
    [SerializeField] AudioClip zipSound;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = zipSound;
        audioSource.pitch = Random.Range(1.0f, 1.5f);
        audioSource.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSpot : MonoBehaviour
{
    [SerializeField] GameObject shootingEnemy;

    [Header("Audio")]
    [SerializeField] AudioClip gunClick;
    [SerializeField] AudioClip gunShot;

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public bool explosion = false;
    public void startTrigger()
    {
        shootingEnemy.GetComponent<ShootEnemy>().Shoot();

        audioSource.volume = 0.6f;
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.clip = gunShot;
        audioSource.Play();
    }

    public void PlayGunClick()
    {
        audioSource.volume = 0.85f;
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.clip = gunClick;
        audioSource.Play();
    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seagull : MonoBehaviour
{
    bool flewAway;                  // destroys the seagull after a certain amount of time
    bool isFlying;
    [SerializeField] private float startDelay = -1;       // idle animation start delay to prevent the birds from having synced up animations
    private float delayTimer;
    private Animator animator;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private bool isBGSeagull;          // if the seagull is going to be in the background, it will play different animations.

    private void Awake()
    {
        flewAway = false;
        animator = GetComponent<Animator>();

        // If startDelay is set to -1, it will be assigned a random value instead.
        if (startDelay == -1)
        {
            startDelay = UnityEngine.Random.Range(0.01f, 2.50f);
        }
        delayTimer = startDelay;
    }
    private void Update()
    {
        // When the start delay is over, idle animation starts
        delayTimer -= Time.deltaTime;
        if (delayTimer <= 0) 
        {
            if (isBGSeagull == false)
            {
                animator.SetBool("Begin", true);
            }
            else
            {
                animator.SetBool("BeginBG", true);
            }
            
        }

        // destroys the seagull after a certain amount of time
        if (flewAway) 
        {
            audioSource.volume -= Time.deltaTime * 0.1f;
            Destroy(gameObject, 4f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // fly away when the player gets close
        if (other.tag == "Player" && flewAway == false)
        {
            FlyAway();
        }
    }

    // Function that starts the flying animation
    private void FlyAway()
    {
        animator.SetBool("Startled", true);
        audioSource.pitch = UnityEngine.Random.Range(0.65f, 1.35f);
        audioSource.Play();
        flewAway = true;
    }
}

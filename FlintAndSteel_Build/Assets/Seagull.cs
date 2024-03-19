using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seagull : MonoBehaviour
{
    bool flewAway;                  // destroys the seagull after a certain amount of time
    private float startDelay;       // idle animation start delay to prevent the birds from having synced up animations
    private float delayTimer;
    private Animator animator;

    private void Awake()
    {
        flewAway = false;
        animator = GetComponent<Animator>();
        startDelay = UnityEngine.Random.Range(0.01f, 2.50f);
        delayTimer = startDelay;
    }
    private void Update()
    {
        // When the start delay is over, idle animation starts
        delayTimer -= Time.deltaTime;
        if (delayTimer <= 0) 
        {
            animator.SetBool("Begin", true);
        }

        // destroys the seagull after a certain amount of time
        if (flewAway) 
        {
            Destroy(gameObject, 4f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // fly away when the player gets close
        if (other.tag == "Player")
        {
            FlyAway();
        }
    }

    // Function that starts the flying animation
    private void FlyAway()
    {
        animator.SetBool("Startled", true);
        flewAway = true;
    }
}

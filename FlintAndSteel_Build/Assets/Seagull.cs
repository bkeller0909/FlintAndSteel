using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seagull : MonoBehaviour
{

    private float startDelay;
    private float delayTimer;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        startDelay = UnityEngine.Random.Range(0.01f, 2.00f);
        delayTimer = startDelay;
    }
    private void Update()
    {
        delayTimer -= Time.deltaTime;
        if (delayTimer <= 0) 
        {
            animator.SetBool("Begin", true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            FlyAway();
        }
    }

    private void FlyAway()
    {
        animator.SetBool("Startled", true);
    }
}

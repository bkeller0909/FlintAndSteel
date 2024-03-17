using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{
    private float crumbleTimer;
    [SerializeField] float timeToCrumble;
    private float revertTimer;
    [SerializeField] float timeToRevert;
    private bool playerCollided;

    private AudioSource audioSource;
    private Animator animator;

    private bool crumbled;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        crumbleTimer = timeToCrumble;
        revertTimer = timeToRevert;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCollided) 
        {
            Crumble();
        }

        if (crumbled) 
        {
            Revert();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerCollided = true;
        }
    }

    void Crumble()
    {
        // Start timer for crumbling
        crumbleTimer -= Time.deltaTime;

        if (crumbleTimer <= 0)
        {
            animator.SetBool("Respawn", false);
            animator.SetBool("Falling", true);

            // Disable collider
            gameObject.GetComponent<BoxCollider>().enabled = false;

            // Change pitch and play sound
            audioSource.pitch = Random.Range(0.88f, 1.12f);
            audioSource.Play();

            crumbled = true;
            playerCollided = false;

            // Reset timer
            crumbleTimer = timeToCrumble;
        }
    }  
    
    void Revert()
    {
        // Start timer to revert platform
        revertTimer -= Time.deltaTime;

        if (revertTimer <= 0) 
        {
            animator.SetBool("Respawn", true);
            animator.SetBool("Falling", false);

            // Enable collider
            gameObject.GetComponent<BoxCollider>().enabled = true;

            crumbled = false;

            // Reset timer
            revertTimer = timeToRevert;
        }
    }
}

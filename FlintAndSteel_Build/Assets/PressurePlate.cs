using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public bool activated;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        activated = false;    
    }

    private void OnTriggerStay(Collider other)
    {
        // if the player or boulder goes onto the pressure plate, activate
        if (other.CompareTag("Feet") || other.CompareTag("Pushable"))
        {
            activated = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // if the player or boulder leaves, deactivate
        if (other.CompareTag("Feet") || other.CompareTag("Pushable"))
        {
            activated = false;   
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // if the player or boulder leaves, deactivate
        if (other.CompareTag("Feet") || other.CompareTag("Pushable"))
        {
            audioSource.pitch = Random.Range(1.3f, 1.5f);
            audioSource.Play();
        }
    }
}

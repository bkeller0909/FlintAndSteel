using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    [SerializeField] AudioClip[] sandSteps;
    [SerializeField] AudioClip[] grassSteps;
    [SerializeField] AudioClip[] stoneSteps;
    [SerializeField] AudioClip[] woodSteps;

    bool onSand;
    bool onGrass;
    bool onStone;
    bool onWood;
    

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Sand"))
        {
            onSand = true;
            onGrass = false;
            onStone = false;
            onWood = false;
        }
        
        if (other.CompareTag("Grass"))
        {
            onSand = false;
            onGrass = true;
            onStone = false;
            onWood = false;
        }

        if (other.CompareTag("Stone"))
        {
            onSand = false;
            onGrass = false;
            onStone = true;
            onWood = false;
        }
        
        if (other.CompareTag("Wood"))
        {
            onSand = false;
            onGrass = false;
            onStone = false;
            onWood = true;
        }
    }

    public void PlayFootstep()
    {
        // depending on the material the player is on, pick a random clip from the array and play it
        if (onSand)
        {
            int soundChoice = Random.Range(0, sandSteps.Length);
            audioSource.PlayOneShot(sandSteps[soundChoice]);
            Debug.Log("Sand");
        }
        else if (onGrass)
        {
            int soundChoice = Random.Range(0, grassSteps.Length);
            audioSource.PlayOneShot(grassSteps[soundChoice]);
            Debug.Log("Grass");
        }
        else if (onStone)
        {
            int soundChoice = Random.Range(0, stoneSteps.Length);
            audioSource.PlayOneShot(stoneSteps[soundChoice]);
            Debug.Log("Stone");
        }
        else if (onWood)
        {
            int soundChoice = Random.Range(0, woodSteps.Length);
            audioSource.PlayOneShot(woodSteps[soundChoice]);
            Debug.Log("Wood");
        }
        else
        {
            // play a default sound
        }
    }
}

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sand"))
        {
            onSand = true;
            onGrass = false;
            onStone = false;
            onWood = false;
        }
        else if (other.CompareTag("Grass"))
        {
            onSand = false;
            onGrass = true;
            onStone = false;
            onWood = false;
        }
        else if (other.CompareTag("Stone"))
        {
            onSand = false;
            onGrass = false;
            onStone = true;
            onWood = false;
        }
        else if (other.CompareTag("Wood"))
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
            int soundChoice = Random.Range(0, sandSteps.Length + 1);
            audioSource.PlayOneShot(sandSteps[soundChoice]);
        }
        else if (onGrass)
        {
            int soundChoice = Random.Range(0, grassSteps.Length + 1);
            audioSource.PlayOneShot(grassSteps[soundChoice]);
        }
        else if (onStone)
        {
            int soundChoice = Random.Range(0, stoneSteps.Length + 1);
            audioSource.PlayOneShot(stoneSteps[soundChoice]);
        }
        else if (onWood)
        {
            int soundChoice = Random.Range(0, woodSteps.Length + 1);
            audioSource.PlayOneShot(woodSteps[soundChoice]);
        }
    }
}

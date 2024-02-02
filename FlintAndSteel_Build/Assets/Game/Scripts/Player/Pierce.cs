//-----------------------------------------------------------------------
//Script for the sword to pierce into the wall or any object
//Tag the object as Pierce so that the sword will pierce the object
//Right now it's gonna destroy the objects it's gonna pierce into since the animation is not ready

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pierce : MonoBehaviour
{
    [SerializeField]
    // Animator SwordPierceAnimation
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        // Check if the thrown sword collides with a specific object (e.g., objects with the "Stabbable" tag).
        if (collision.gameObject.CompareTag("Pierce"))
        {
            Debug.LogError("Sword stabs onto " + collision.gameObject.name);


            //Add the animation to make the sword stab into the object you need
            Destroy(collision.gameObject);
        }
    }
}

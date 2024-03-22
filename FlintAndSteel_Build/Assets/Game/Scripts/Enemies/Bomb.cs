using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem explosion;

    void OnCollisionEnter(Collision collision)
    {
        // Check if the bomb collided with something
        // If it did, destroy the bomb after 2.0 seconds
        Destroy(gameObject,2.0f);
        
        
    }


    private void OnDestroy()
    {
        Instantiate(explosion);
        explosion.Play();
    }

}

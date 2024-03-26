using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem explosionPrefab; // Renamed from 'explosion' to 'explosionPrefab' for clarity

    void OnCollisionEnter(Collision collision)
    {
        // Check if the bomb collided with something
        // If it did, destroy the bomb after 2.0 seconds
        Destroy(gameObject, 2.0f);
    }

    private void OnDestroy()
    {
        // Instantiate explosion at the bomb's position
        ParticleSystem explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        explosion.Play();

        // Destroy the explosion particle system after its duration
        Destroy(explosion.gameObject, explosion.main.duration);
    }
}

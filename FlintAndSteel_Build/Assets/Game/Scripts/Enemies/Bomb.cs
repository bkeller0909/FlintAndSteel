using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float explosionTime = 3f; // Time before the bomb explodes

    [SerializeField]
    private ParticleSystem[] explosion;

    void Start()
    {
        // Start a countdown before the bomb explodes
        Invoke("Explode", explosionTime);
    }

    void Explode()
    {
        foreach (ParticleSystem particle in explosion)
        {
            particle.Play();
        }
        // Add explosion effects or any other logic here
        Debug.Log("Boom! The bomb exploded!");
        Destroy(gameObject); // Destroy the bomb object after exploding
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the bomb collided with something
        // If it did, destroy the bomb after 2.0 seconds
        Destroy(gameObject, 2.0f);
    }
}

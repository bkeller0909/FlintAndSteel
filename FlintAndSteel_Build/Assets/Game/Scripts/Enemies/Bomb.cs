using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionPrefab; // Renamed from 'explosion' to 'explosionPrefab' for clarity
    [SerializeField]
    private GameObject explosionSound;

    bool startTimer;
    float explosionTimer = 2.0f;

    void OnCollisionEnter(Collision collision)
    {
        // Check if the bomb collided with something
        // If it did, destroy the bomb after 2.0 seconds
        startTimer = true;
    }

    private void Update()
    {
        if (startTimer)
        {
            explosionTimer -= Time.deltaTime;
        }

        if (explosionTimer <= 0)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Instantiate(explosionSound, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}

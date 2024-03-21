using UnityEngine;

public class Bomb : MonoBehaviour
{
    // Optional explosion effect prefab
    public GameObject explosionEffectPrefab;

    // Boolean to check if the bomb has exploded
    private bool hasExploded = false;

    // OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the bomb hasn't already exploded and if it collided with something
        if (!hasExploded)
        {
            // Set the flag to prevent multiple explosions
            hasExploded = true;

            // Optionally spawn an explosion effect
            if (explosionEffectPrefab != null)
            {
                Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            }

            Debug.Log("Bomb Destroyed");
            // Destroy the bomb object
            Destroy(gameObject);
        }
    }
}

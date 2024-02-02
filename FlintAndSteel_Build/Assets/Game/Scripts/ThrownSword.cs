using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownSword : MonoBehaviour
{
    [SerializeField] private GameObject swordPlatform;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WoodWall"))
        {
            Instantiate(swordPlatform, other.transform.position, Quaternion.identity);
            Destroy(gameObject);
            return;
        }

        if (other.CompareTag("Player"))
        {
            return;
        }

        // If sword doesnt hit a wood wall, enemy, or player: destroy itself (replace with animation in the future)
        Destroy(gameObject);
    }
}

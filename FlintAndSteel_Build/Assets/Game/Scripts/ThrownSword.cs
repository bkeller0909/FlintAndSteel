using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownSword : MonoBehaviour
{
    [SerializeField] private GameObject swordPlatform;
    private float platformSpawnOffset;
    private float platformSpawnDirection;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WoodWall"))
        {
            Instantiate(swordPlatform, other.transform.position + new Vector3(platformSpawnOffset, 0, 0), Quaternion.Euler(-90, platformSpawnDirection, 0));
            Destroy(gameObject);
            return;
        }

        if (other.CompareTag("Player"))
        {
            return;
        }

        if(other.CompareTag("Coin"))
        {
            return;
        }

        if(other.CompareTag("Enemy"))
        {
            return;
        }

        // If sword doesnt hit a wood wall, enemy, or player: destroy itself (replace with animation in the future)
        Destroy(gameObject);
    }

    private void Update()
    {
        // checks if it can find our player
        if (GameObject.FindGameObjectWithTag("Player") != null) 
        {
            //references player's position and using that to determine which way the platform should spawn
            Transform playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            if (transform.position.x < playerTransform.position.x)
            {
                platformSpawnDirection = 90;
                platformSpawnOffset = 0.5f;
            }
            else
            {
                platformSpawnDirection = -90;
                platformSpawnOffset = -0.5f;
            }
        }
    }
}

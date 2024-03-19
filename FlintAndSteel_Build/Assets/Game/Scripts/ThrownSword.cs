using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownSword : MonoBehaviour
{
    [SerializeField] private GameObject swordPlatform;
    [SerializeField] private GameObject sparks;
    private float platformSpawnOffset;
    private float platformSpawnDirection;

    GameObject player;
    PlayerAttackScript playerAttackScript;

    bool hitWoodWall = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerAttackScript = player.GetComponent<PlayerAttackScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WoodWall"))
        {
            hitWoodWall = true;
            Instantiate(swordPlatform, other.transform.position + new Vector3(platformSpawnOffset, 0, 0), Quaternion.Euler(-90, platformSpawnDirection, 0));
            Destroy(gameObject);
            return;
        }
        
        if (other.CompareTag("Stone") || other.CompareTag("Pushable") && !hitWoodWall)
        {
            Instantiate(sparks, transform.position, Quaternion.Euler(0, Vector3.Angle(other.transform.position, transform.position), 0));
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

        if(other.CompareTag("Respawn"))
        {
            return;
        }

        if(other.CompareTag("Boss"))
        {
            return;
        }

        if(other.CompareTag("WorldBorder"))
        {
            return;
        }

        if (other.CompareTag("Hook Climb"))
        {
            return;
        }

        if (other.CompareTag("ExplosionSpot"))
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
                platformSpawnOffset = 0.68f;
            }
            else
            {
                platformSpawnDirection = -90;
                platformSpawnOffset = -0.68f;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownSword : MonoBehaviour
{
    [SerializeField] private GameObject swordPlatform;
    [SerializeField] private GameObject sparks;
    private float platformSpawnOffset;
    private float platformSpawnDirection;

    [SerializeField] private GameObject trailGO;
    private TrailRenderer trail;

    GameObject player;
    PlayerAttackScript playerAttackScript;

    bool hitWoodWall = false;
    bool triggerEntered = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerAttackScript = player.GetComponent<PlayerAttackScript>();

        if (trailGO != null ) 
        {
            trail = trailGO.GetComponent<TrailRenderer>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggerEntered)
            return;

        if (other.CompareTag("WoodWall"))
        {
            triggerEntered = true;
            hitWoodWall = true;

            if (other.GetComponent<WoodWall>().swordOnLeft)
            {
                platformSpawnDirection = -90;
                platformSpawnOffset = -0.68f;
            }
            else
            {
                platformSpawnDirection = 90;
                platformSpawnOffset = 0.68f;
            }

            Instantiate(swordPlatform, other.transform.position + new Vector3(platformSpawnOffset, 0, 0), Quaternion.Euler(-90, platformSpawnDirection, 0));

            Destroy(gameObject);
            return;
        }
        
        if (other.CompareTag("Stone") || other.CompareTag("Pushable") && !hitWoodWall)
        {
            triggerEntered = true;
            Instantiate(sparks, transform.position, Quaternion.Euler(0, Vector3.Angle(other.transform.position, transform.position), 0));
            playerAttackScript.SwordRecall();
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
        triggerEntered = true;
        playerAttackScript.SwordRecall();
        Destroy(gameObject);
    }

    private void Update()
    {
        if (trailGO != null) 
        {
            if (playerAttackScript.isSwordThrown)
            {
                trail.gameObject.SetActive(true);
                trail.emitting = true;
                trail.time = 0.05f;
            }
            else
            {
                trail.emitting = false;
                trail.gameObject.SetActive(false);
            }
        }
    }
}

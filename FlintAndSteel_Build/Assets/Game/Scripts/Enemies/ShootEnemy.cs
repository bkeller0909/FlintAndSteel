using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEnemy : MonoBehaviour
{
    #region PrivateFields
    [Header("Debug")]
    [SerializeField] bool showDebug = false;

    [Header("Movement Options")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float moveDistance = 5f;

    [Header("Targeting Options")]
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float returnRange = 5f;
    [SerializeField] private bool detectionEnabled = true;
    [SerializeField] private Transform player;

    [Header("Bullet Stuff")]
    [SerializeField] private Transform fireLocation;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] GameObject shotParticles;

    [Header("Hit Effects")]
    [SerializeField] private GameObject bloodEffect;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private GameObject smallDeathEffect;

    //Sound effects
    [SerializeField] private AudioClip PistolCocking;
    [SerializeField] private AudioSource audioSource;

    private Vector3 startPosition;
    private float travelledDistance = 0f;
    private float extraTravelledDistance = 0f;
    private bool movingForward = true;
    private bool isReturningToStart = false;

    private int enemyMaxHealth = 1;
    private int enemyCurrentHealth;

    private float shootTimer;
    private Animator animator;

    private bool shot = false;
    #endregion

    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        startPosition = transform.position;     //Enemy starting coords
        enemyCurrentHealth = enemyMaxHealth;

        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            if (showDebug == true) Debug.LogError("Player not found!");
        }

   

    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Check for returning to start position
        if (isReturningToStart)
        {
            ReturnToStartPosition();
        }
        else if (distanceToPlayer <= detectionRange && detectionEnabled)
        {
            DelayShoot();
            //MoveAwayNShoot();
        }
        else
        {
            Patrol();
        }
    }

    private void MoveAwayNShoot()
    {
        // More logic needed to be added

        if (player != null)
        {
            if (detectionEnabled == true)
            {
                // Move towards the player
                Vector3 directionToPlayer = (transform.position - player.position).normalized;
                directionToPlayer.y = 0; // Keep Y-axis unchanged
                directionToPlayer.z = 0; // Keep Z-axis unchanged

                transform.Translate(directionToPlayer * moveSpeed * Time.deltaTime);

                extraTravelledDistance += moveSpeed * Time.deltaTime;

                if (extraTravelledDistance >= returnRange)
                {
                    isReturningToStart = true;
                }
            }
            else
            {
                detectionRange = 0;
            }

        }
    }

    /// <summary>
    /// shooting timer that shoots after so many seconds
    /// </summary>
    private void DelayShoot()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f && bulletPrefab != null)
        {
            if (!shot)
            {
                shot = true;

                animator.SetTrigger("ThrowSword");
                shootTimer = shootInterval;
            }
        }
    }
    /// <summary>
    /// shoots enemy bullet prefab out of FireLocation
    /// </summary>
    public void Shoot()
    {
        Vector3 dir = (player.position - fireLocation.position).normalized;
        
        dir.z = 0;

        GameObject bulletGO = Instantiate(bulletPrefab, fireLocation.position, Quaternion.identity);
        EnemyBullet bulletScript = bulletGO.GetComponent<EnemyBullet>();
        bulletScript.SetSpeed(10);
        bulletScript.Fire(dir);

        Instantiate(shotParticles, fireLocation.position, Quaternion.identity);

        shot = false;
    }

    private void Patrol()
    {
        if (movingForward)
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        else
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

        // Updates the travelled distance
        travelledDistance += moveSpeed * Time.deltaTime;

        // Check if the Enemy moved required distance
        if (travelledDistance >= moveDistance)
        {
            // Reset travelled distance
            travelledDistance = 0f;

            // Reverse the movement direction
            movingForward = !movingForward;
        }
    }

    private void ReturnToStartPosition()
    {
        // instead of returning needs to try to maintain distance and go back towards player if player goes backwards (left)

        // Debug log to check if this function is called
        if (showDebug == true) Debug.Log("Attempting to return to start position.");

        float distanceToStart = Vector3.Distance(transform.position, startPosition);

        // Log the distance to start
        if (showDebug == true) Debug.Log($"Distance to Start: {distanceToStart}");

        if (distanceToStart > 0.1f)
        {
            Vector3 directionToStart = (startPosition - transform.position).normalized;
            directionToStart.y = 0;
            directionToStart.z = 0;

            transform.Translate(directionToStart * moveSpeed * Time.deltaTime, Space.World);

            // Debug log to confirm direction and movement
            if (showDebug == true) Debug.Log($"Moving towards start. Direction: {directionToStart}, Speed: {moveSpeed}");
        }
        else
        {
            // Reached or very close to start position
            if (showDebug == true) Debug.Log("Reached start position.");

            isReturningToStart = false;
            movingForward = true;
            extraTravelledDistance = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword"))
        {

            Damaged(1); // take 1 damage
            if (enemyCurrentHealth > 0)
            {
                Instantiate(bloodEffect, transform.position, Quaternion.identity);
            }
        }
    }

    private void Damaged(int damage)
    {
        try
        {
            enemyCurrentHealth -= damage; // Lower Health with whatever damage was received

            if (showDebug) Debug.Log("Enemy Health: " + enemyCurrentHealth);

            if (enemyCurrentHealth <= 0) // If health is or less than 0 enemy is dead
            {
                if (showDebug) Debug.Log("MainEnemy Killed");
                Instantiate(deathEffect, transform.position, Quaternion.identity);
                Instantiate(smallDeathEffect, transform.position, Quaternion.identity);
                gameObject.SetActive(false);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error in Damaged function: {e.Message}");
        }
    }
}


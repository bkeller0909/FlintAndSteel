using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
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

    private Vector3 startPosition;
    private float travelledDistance = 0f;
    private float extraTravelledDistance = 0f;
    private bool movingForward = true;
    private bool isReturningToStart = false;

    private int enemyMaxHealth = 1; //Maximum possible health
    private int enemyCurrentHealth;
    #endregion

    void Start()
    {
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
            MoveTowardsPlayer();
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        // flip direction
        if (movingForward)
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            transform.localScale = new Vector3(-1, 1, 1);
        }

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

    private void MoveTowardsPlayer()
    {
        if (player != null)
        {
            if (detectionEnabled == true)
            {
                // Move towards the player
                Vector3 directionToPlayer = (player.position - transform.position).normalized;
                directionToPlayer.y = 0; // Keep Y-axis unchanged
                directionToPlayer.z = 0; // Keep Z-axis unchanged

                transform.Translate(directionToPlayer * moveSpeed * Time.deltaTime);

                // flip direction
                if (directionToPlayer.x < 0)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }

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

    private void ReturnToStartPosition()
    {
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
        }
    }

    private void Damaged(int damage)
    {
        enemyCurrentHealth -= damage; // lower Health with whatever damage was recieved

        if (showDebug == true) Debug.Log("Enemy Health: " + enemyCurrentHealth);

        if (enemyCurrentHealth <= 0)  // if health is or less than 0 enemy is dead
        {
            if (showDebug == true) Debug.Log("MainEnemy Killed");
            gameObject.SetActive(false);
        }
    }
}

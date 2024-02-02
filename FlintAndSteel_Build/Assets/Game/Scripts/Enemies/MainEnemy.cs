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
    [SerializeField] private bool detectionEnabled = true;
    [SerializeField] private Transform player;

    private Vector3 startPosition;
    private float traveledDistance = 0f;
    private bool movingForward = true;

    private int enemyMaxHealth = 3; //Maximum possible health
    private int enemyCurrentHealth; 
    #endregion

    // Start is called before the first frame update
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
            if(showDebug == true) Debug.LogError("Player not found!");
        }

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRange)
            {
                if (detectionEnabled == true)
                {
                    // Move towards the player
                    Vector3 directionToPlayer = (player.position - transform.position).normalized;
                    directionToPlayer.y = 0; // Keep Y-axis unchanged
                    directionToPlayer.z = 0; // Keep Z-axis unchanged
                    transform.Translate(directionToPlayer * moveSpeed * Time.deltaTime);
                }
                else
                {
                    detectionRange = 0;
                }
            }
            else
            {
                if (movingForward)
                    transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
                else
                    transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);


                // Updates the traveled distance
                traveledDistance += moveSpeed * Time.deltaTime;

                // Check if the Enemy moved required distance
                if (traveledDistance >= moveDistance)
                {
                    // Reset traveled distance
                    traveledDistance = 0f;

                    // Reverse the movement direction
                    movingForward = !movingForward;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            Damaged(1); // take 1 damage
            if (showDebug == true) Debug.Log("Enemy Health: " + enemyCurrentHealth);
        }
    }

    private void Damaged(int damage)
    {
        enemyCurrentHealth -= damage; // lower Health with whatever damage was recieved

        if (enemyCurrentHealth <= 0)  // if health is or less than 0 enemy is dead
        {
            if (showDebug == true) Debug.Log("MainEnemy Killed");
            gameObject.SetActive(false);
        }
    }
}

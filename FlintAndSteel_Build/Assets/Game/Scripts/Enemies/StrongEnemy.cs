using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongEnemy : MonoBehaviour
{
    #region PrivateFields
    [Header("Debug")]
    [SerializeField] private bool showDebug = false;

    [Header("Movement Options")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float moveDistance = 5f;
    [SerializeField] private LayerMask groundLayer = (1 << 0) | (1 << 20);
    [SerializeField] private float groundCheckDistance = 5f;

    [Header("Dashing Mechanic")]
    [SerializeField] private float dashStrength;
    [SerializeField] private float timeBetweenDashes;
    [SerializeField] private float dashLength;
    private float dashTimer;
    private float dashLengthTimer;
    private bool dashing;


    [Header("Targeting Options")]
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private bool detectionEnabled = true;
    [SerializeField] private Transform player;

    [Header("Hit Effects")]
    [SerializeField] private GameObject bloodEffect;
    [SerializeField] private GameObject deathEffect;


    private Vector3 startPosition;
    private float travelledDistance = 0f;
    private bool movingForward = true;

    private int enemyMaxHealth = 4;
    private int enemyCurrentHealth;

    private Animator animator;
    private PlayerAttackScript playerAtkScript;

    private enum State
    {
        Patrolling,
        Chasing,
        Returning
    }

    private State currentState;
    #endregion

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        startPosition = transform.position;
        enemyCurrentHealth = enemyMaxHealth;
        dashTimer = timeBetweenDashes;
        dashLengthTimer = dashLength;
        dashing = false;

        FindPlayer();
        currentState = State.Patrolling;
    }

    void Update()
    {
        if (detectionEnabled) CheckForPlayer();

        switch (currentState)
        {
            case State.Patrolling:
                Patrol();
                break;
            case State.Chasing:
                MoveTowardsPlayer();
                break;
            case State.Returning:
                ReturnToStartPosition();
                break;
        }
    }

    /// <summary>
    /// Locates the player GameObject by tag and assigns it to the 'player' variable.
    /// </summary>
    private void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            playerAtkScript = playerObject.GetComponent<PlayerAttackScript>();
        }
        else if (showDebug)
        {
            Debug.LogError("Player not found!");
        }
    }

    /// <summary>
    /// Handles patrolling behavior of the enemy, moving it back and forth.
    /// </summary>
    private void Patrol()
    {
        Vector3 direction = movingForward ? Vector3.left : Vector3.right;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
        transform.localScale = movingForward ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);

        travelledDistance += moveSpeed * Time.deltaTime;
        if (travelledDistance >= moveDistance)
        {
            travelledDistance = 0f;
            movingForward = !movingForward;
        }
    }

    /// <summary>
    /// Checks the distance to the player and changes the state to chasing or returning based on proximity.
    /// </summary>
    private void CheckForPlayer()
    {
        if (player && Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            currentState = State.Chasing;
        }
        else if (currentState == State.Chasing)
        {
            currentState = State.Returning;
        }
    }

    /// <summary>
    /// Directs the enemy towards the player when the enemy is in chasing state.
    /// </summary>
    private void MoveTowardsPlayer()
    {
        if (!detectionEnabled || player == null)
        {
            currentState = State.Returning;
            return;
        }

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        directionToPlayer.z = 0; // Keep Z-axis unchanged, since movement is along the X-axis

        // Determine the direction to angle the raycast based on enemy orientation
        float angleDirection = transform.localScale.x > 0 ? 1f : -1f; // Assumes scale.x > 0 is facing right, < 0 is facing left
        Vector3 angledDirection = Quaternion.Euler(0, 0, -15 * angleDirection) * Vector3.down; // Angles the raycast slightly outward in front of the enemy

        // Edge Detection for Chasing
        Vector3 groundCheckStartPoint = transform.position + directionToPlayer * (moveSpeed * Time.deltaTime + 0.1f);
        groundCheckStartPoint.y += 0.5f; // Adjust this offset to start the raycast from above the ground
        bool isGroundAhead = Physics.Raycast(groundCheckStartPoint, angledDirection, groundCheckDistance, groundLayer);

        if (!isGroundAhead)
        {
            if (showDebug) Debug.Log("No ground ahead while chasing. Stopping chase.");
            currentState = State.Returning;
            return;
        }

        transform.Translate(directionToPlayer * moveSpeed * Time.deltaTime, Space.World);

        // Flip enemy to face player
        if (directionToPlayer.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Adjusted to correctly flip based on direction
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        Dashing(directionToPlayer);

        if (Vector3.Distance(transform.position, player.position) > detectionRange)
        {
            currentState = State.Returning;
        }
    }
    /// <summary>
    /// The function manages the enemy's movement, toggling between dashing and regular movement towards the player.
    /// </summary>
    /// <param name="directionToPlayer">The direction to the Player.</param>
    private void Dashing(Vector3 directionToPlayer)
    {
        if (!dashing)
        {
            transform.Translate(directionToPlayer * moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(directionToPlayer * dashStrength * Time.deltaTime);
            dashLengthTimer -= Time.deltaTime;
            if (dashLengthTimer <= 0)
            {
                dashing = false;
                dashLengthTimer = dashLength;
                dashTimer = timeBetweenDashes;
            }
        }

        dashTimer -= Time.deltaTime;
        if (dashTimer <= 0 && currentState == State.Chasing)
        {
            dashing = true;
        }
    }

    /// <summary>
    /// Returns the enemy to its starting position when the player is no longer in chase range.
    /// </summary>
    private void ReturnToStartPosition()
    {
        if (showDebug) Debug.Log("Attempting to return to start position.");

        Vector3 directionToStart = (startPosition - transform.position).normalized;
        directionToStart.z = 0; // Keep Z-axis unchanged

        if (directionToStart.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Facing left
        }
        else if (directionToStart.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Facing right
        }

        transform.Translate(directionToStart * moveSpeed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, startPosition) <= 0.1f)
        {
            if (showDebug) Debug.Log("Reached start position.");
            currentState = State.Patrolling;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            Damaged(1); // Take 1 damage
            if (enemyCurrentHealth > 0) 
            {
                Instantiate(bloodEffect, transform.position, Quaternion.identity);
                animator.SetTrigger("Hit");
                playerAtkScript.SwordRecall();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animator.SetTrigger("ThrowSword");
        }
    }

    /// <summary>
    /// Applies damage to the enemy.
    /// </summary>
    /// <param name="damage">The amount of damage to apply to the enemy.</param>
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
                gameObject.SetActive(false);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error in Damaged function: {e.Message}");
        }
    }

}

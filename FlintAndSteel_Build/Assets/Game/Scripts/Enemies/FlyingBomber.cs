using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBomber : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    //[SerializeField] private Transform[] waypoints;
    private int currentWaypointIndex = 0;

    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private float bombDropInterval = 5f;
    [SerializeField] private GameObject fruitPrefab;

    private int enemyMaxHealth = 2;
    private int enemyCurrentHealth;

    public bool isDead = false;
    private int hitCount = 0;
    private GameObject[] birdWalls;
    private int currentBirdWall = 0;
    private Transform targetBirdWall;
    private float initialYposition;

    private bool movingRight = true;
    private Vector3 initialPosition;
   //[SerializeField] private float moveDistance = 15.0f;



    private void Start()
    {
        birdWalls = GameObject.FindGameObjectsWithTag("BirdWall");
        //initialPosition = transform.position;   
        enemyCurrentHealth = enemyMaxHealth;
        StartCoroutine(DropBombRoutine());
        FindNextBirdWall();
    }

    private void Update()
    {
        if (!isDead)
        {
            MoveTowardsWall();
            //MoveEnemy();
            //MoveBetweenWaypoints();
        }
    }
    private void MoveTowardsWall()
    {
        if (birdWalls.Length == 0)
            return;

        transform.position = Vector3.MoveTowards(transform.position, targetBirdWall.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetBirdWall.position) < 0.1f)
        {
            FindNextBirdWall();
        }
    }
    private void FindNextBirdWall()
    {
        if (birdWalls.Length == 0)
            return;

        currentBirdWall = (currentBirdWall + 1) % birdWalls.Length;
        targetBirdWall = birdWalls[currentBirdWall].transform;
    }
    //private void MoveEnemy()
    //{
    //    if (movingRight)
    //    {
    //        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

    //        if (transform.position.x >= initialPosition.x + moveDistance)
    //        {
    //            movingRight = false;
    //        }
    //    }
    //    else
    //    {
    //        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

    //        if (transform.position.x <= initialPosition.x - moveDistance)
    //        {
    //            movingRight = true;
    //        }
    //    }

    //    Debug.Log("Enemy position: " + transform.position);
    //}

    //private void MoveBetweenWaypoints()
    //{
    //    if (waypoints.Length == 0)
    //        return;

    //    Transform currentWaypoint = waypoints[currentWaypointIndex];

    //    transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);

    //    if (Vector3.Distance(transform.position, currentWaypoint.position) < 0.1f)
    //    {
    //        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    //    }
    //}

    private IEnumerator DropBombRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(bombDropInterval);
            if (!isDead)
            {
                DropBomb();
            }
        }
    }

    private void DropBomb()
    {
        Instantiate(bombPrefab, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sword"))
        {
            TakeDamage(1); // Sword deals 1 damage
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            enemyCurrentHealth -= damage;
            if (enemyCurrentHealth <= 0)
            {
                hitCount++;
                if (hitCount >= 2) // Check if hit count is 2 or more
                {
                    Die();
                }
                else
                {
                    enemyCurrentHealth = enemyMaxHealth; // Reset health for the next hit
                }
            }
        }
    }

    private void Die()
    {
        isDead = true;
        //Play the death animation and particle effect
        DropFruit();
        Destroy(gameObject); // Destroy the enemy object
    }

    private void DropFruit()
    {
        float randomFloat = Random.value;//generates a random number between 0 and 1

       //if the random number is less than or equal to 0.5 it spawns the fruit
        if (randomFloat <= 0.5f)
        {
        Instantiate(fruitPrefab, transform.position, Quaternion.identity);  
            
        }
    }
}

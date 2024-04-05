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

    [SerializeField]
    private GameObject BirdModel;
    private int enemyMaxHealth = 2;
    private int enemyCurrentHealth;

    [SerializeField] GameObject bloodEffect;

    public bool isDead = false;
    private GameObject[] birdWalls;
    private int currentBirdWall = 0;
    private Transform targetBirdWall;
    private float initialYposition;

    private bool movingRight = true;
    private Vector3 initialPosition;

    //[SerializeField] private float moveDistance = 15.0f;

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("Boss") == null)
        {
            Destroy(gameObject);
        }
    }

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

        if (GameObject.FindGameObjectWithTag("Boss") == null)
        {
            Destroy(gameObject);
        }
    }
    private void MoveTowardsWall()
    {
        if (birdWalls.Length == 0)
            return;

        // Calculate the direction to the next bird wall
        Vector3 direction = (targetBirdWall.position - transform.position).normalized;

        // Move the enemy towards the wall
        transform.position = Vector3.MoveTowards(transform.position, targetBirdWall.position, moveSpeed * Time.deltaTime);

        BirdModel.transform.rotation = Quaternion.Euler(-80f, BirdModel.transform.rotation.eulerAngles.y, 0f);
        if (Vector3.Distance(transform.position, targetBirdWall.position) < 0.1f)
        {
            // Find the next bird wall
            FindNextBirdWall();
        }

        // Rotate the model to face the opposite direction of movement
        Quaternion targetRotation = Quaternion.LookRotation(-direction);
        BirdModel.transform.rotation = Quaternion.RotateTowards(BirdModel.transform.rotation, targetRotation, 180 * Time.deltaTime);
    }




    private void FindNextBirdWall()
    {
        if (birdWalls.Length == 0)
            return;

        currentBirdWall = (currentBirdWall + 1) % birdWalls.Length;
        targetBirdWall = birdWalls[currentBirdWall].transform;
    }

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
            Instantiate(bloodEffect, transform.position, Quaternion.identity);
            if (enemyCurrentHealth <= 0)
            {
                Die();
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

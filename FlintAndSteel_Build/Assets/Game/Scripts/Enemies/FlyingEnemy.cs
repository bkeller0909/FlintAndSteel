using System.Collections;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Transform[] waypoints;
    private int currentWaypointIndex = 0;

    [SerializeField] private float attackSpeed = 10.0f;
    [SerializeField] private float detectionRange = 5.0f;
    [SerializeField] private GameObject player;
    private bool isAttacking = false;

    private void Start()
    {
        if (waypoints.Length == 0)
        {
            Debug.LogError("No waypoints defined for the Flying Enemy!");
            enabled = false;
        }
    }

    private void Update()
    {
        MoveBetweenWaypoints();

        if (!isAttacking && Vector3.Distance(transform.position, player.transform.position) <= detectionRange)
        {
            isAttacking = true;
            StartCoroutine(AttackPlayer());
        }
    }

    private void MoveBetweenWaypoints()
    {
        Transform currentWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, currentWaypoint.position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    private IEnumerator AttackPlayer()
    {
        while (Vector3.Distance(transform.position, player.transform.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, attackSpeed * Time.deltaTime);
            yield return null;
        }
        // Add attack logic here, such as dealing damage to the player

        yield return new WaitForSeconds(5f); //maybe a varaible, this is the cooldown it takes teh boss to be able to attack again after hitting the player
         
        isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
        }
    }
}

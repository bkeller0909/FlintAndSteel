using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Transform[] waypoints; // Define waypoints for movement
    [SerializeField] private float detectionRange = 5f; // Detection range for the player
    private int currentWaypointIndex = 0;
    private Transform player; // Reference to the player's transform
    private bool isPlayerDetected = false; // Flag to track if the player is detected

    private float attackSpeed = 10.0f;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find the player object
        if (waypoints.Length == 0)
        {
            Debug.LogError("No waypoints defined for the Flying Enemy!");
            enabled = false; // Disable the script if no waypoints are defined
        }
    }

    private void Update()
    {
        if (!isPlayerDetected)
        {
            CheckForPlayerDetection();
            MoveBetweenWaypoints();
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    private void CheckForPlayerDetection()
    {
        // Check if the player is within detection range
        if (Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            isPlayerDetected = true;
        }
    }

    private void MoveBetweenWaypoints()
    {
        Transform currentWaypoint = waypoints[currentWaypointIndex];

        // Move towards the current waypoint
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);

        // Check if reached the current waypoint
        if (Vector3.Distance(transform.position, currentWaypoint.position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Move to the next waypoint
        }
    }

    private void MoveTowardsPlayer()
    {
        // Move towards the player
        transform.position = Vector3.MoveTowards(transform.position, player.position, attackSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Handle collision with the player
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBomber : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Transform[] waypoints; // Define waypoints for movement
    private int currentWaypointIndex = 0;

    [SerializeField] private GameObject bombPrefab; // Reference to the bomb prefab
    [SerializeField] private float bombDropInterval = 5f; // Interval between bomb drops

    private void Start()
    {
        if (waypoints.Length == 0)
        {
            Debug.LogError("No waypoints defined for the Flying Enemy!");
            enabled = false; // Disable the script if no waypoints are defined
        }

        StartCoroutine(DropBombRoutine());
    }

    private void Update()
    {
        MoveBetweenWaypoints();
    }

    private void MoveBetweenWaypoints()
    {
        if (waypoints.Length == 0)
            return;

        Transform currentWaypoint = waypoints[currentWaypointIndex];

        // Move towards the current waypoint
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);

        // Check if reached the current waypoint
        if (Vector3.Distance(transform.position, currentWaypoint.position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Move to the next waypoint
        }
    }

    private IEnumerator DropBombRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(bombDropInterval);
            DropBomb();
        }
    }

    private void DropBomb()
    {
        Instantiate(bombPrefab, transform.position, Quaternion.identity); // Instantiate a bomb at the current position
    }
}

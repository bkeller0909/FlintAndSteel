using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorMovement : MonoBehaviour
{
    [SerializeField] private GameObject crosshair;
    [SerializeField] private Transform UICenter;

    [SerializeField] private float crosshairSpeed = 5f; // Adjust the speed as needed
    [SerializeField] private float maxDistanceFromPlayer = 5f; // Adjust the maximum distance

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.usingController == true)
        {
            MoveCrosshair();
        }
    }

    public void MoveCrosshair()
    {
        // Get input from the controller
        float horizontalInput = Input.GetAxis("Horizontal"); // Adjust axis names as needed
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the movement direction based on input
        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0f).normalized;

        // Move the crosshair
        Vector3 newPosition = crosshair.transform.position + moveDirection * crosshairSpeed * Time.deltaTime;

        // Calculate distance from player
        float distanceFromPlayer = Vector3.Distance(newPosition, UICenter.position);

        // Limit the crosshair movement within a maximum distance from the player
        if (distanceFromPlayer <= maxDistanceFromPlayer)
        {
            crosshair.transform.position = newPosition;
        }
        else
        {
            // If the crosshair exceeds the maximum distance, clamp it back to the maximum distance
            Vector3 directionToPlayer = (UICenter.position - newPosition).normalized;
            crosshair.transform.position = UICenter.position + directionToPlayer * maxDistanceFromPlayer;
        }
    }
}

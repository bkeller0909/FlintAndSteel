using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.UI;

public class CursorMovement : MonoBehaviour
{
    [SerializeField] private Image crosshair;
    [SerializeField] private int numPoints = 20;
    [SerializeField] private int radius = 2; //Radius of the circle

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
        //Claculate the angle increment for each point
        float angleIncrement = 360f / numPoints;

        //Get the current angle based on time or any other factor
        Vector3 playerPosition = transform.position;

        //Get the direction from the player to the cursor
        Vector3 direction = crosshair.rectTransform.position - playerPosition;

        // Calculate the angle of the direction vector
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Snap the angle to the nearest angle increment
        float snappedAngle = Mathf.Round(angle / angleIncrement) * angleIncrement;

        // Calculate the position of the cursor based on the snapped angle
        float x = playerPosition.x + radius * Mathf.Cos(snappedAngle * Mathf.Deg2Rad);
        float y = playerPosition.y + radius * Mathf.Sin(snappedAngle * Mathf.Deg2Rad);

        Vector3 crosshairPosition = new Vector3(x, y, 0f);

        crosshair.rectTransform.position = crosshairPosition;
    }
}

using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.UI;

public class CursorMovement : MonoBehaviour
{
    [SerializeField] private Image crosshair;

    // Update is called once per frame
    void Update()
    {
        MoveCrosshair();
    }

    public void MoveCrosshair()
    {
        Vector3 crosshairPosition = crosshair.rectTransform.position;
        Vector3 aim = new Vector3(transform.position.x, transform.position.y, 0f);

        if (aim.magnitude > 0f)
        {
            aim.Normalize();
            crosshair.rectTransform.position = aim;
        }
    }
}

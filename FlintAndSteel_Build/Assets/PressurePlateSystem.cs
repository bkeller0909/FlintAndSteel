using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateSystem : MonoBehaviour
{
    [SerializeField] GameObject pressurePlateGO;
    private PressurePlate pressurePlate;

    [SerializeField] GameObject movingObjectGO;
    [SerializeField] bool moveUp;
    [SerializeField] float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        pressurePlate = pressurePlateGO.GetComponent<PressurePlate>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Activated();
    }

    void Activated()
    {
        if (pressurePlate.activated) 
        {
        // move object
            if (moveUp) 
            {
                movingObjectGO.GetComponent<Rigidbody>().velocity = new Vector3 (0, moveSpeed, 0);
            }
            else
            {
                movingObjectGO.GetComponent<Rigidbody>().velocity = new Vector3(0, -moveSpeed, 0);
            }
        }
        // if the pressure plate is deactivated, move object back to original position
        else
        {
            if (moveUp)
            {
                movingObjectGO.GetComponent<Rigidbody>().velocity = new Vector3(0, -moveSpeed, 0);
            }
            else
            {
                movingObjectGO.GetComponent<Rigidbody>().velocity = new Vector3(0, moveSpeed, 0);
            }
        }
    }
}

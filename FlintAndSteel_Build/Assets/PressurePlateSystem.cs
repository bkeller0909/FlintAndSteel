using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PressurePlateSystem : MonoBehaviour
{
    // Variables for the final boss door
    [SerializeField] GameObject boss;
    [SerializeField] bool isBossDoor = false;

    [SerializeField] GameObject pressurePlateGO;
    private PressurePlate pressurePlate;

    [SerializeField] GameObject movingObjectGO;
    [SerializeField] bool moveUp;
    [SerializeField] float moveSpeed;

    [SerializeField] GameObject wispTrail;

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
        if (!isBossDoor)
        {
            if (pressurePlate.activated)
            {
                // move object
                if (moveUp)
                {
                    movingObjectGO.GetComponent<Rigidbody>().velocity = new Vector3(0, moveSpeed, 0);
                }
                else
                {
                    movingObjectGO.GetComponent<Rigidbody>().velocity = new Vector3(0, -moveSpeed, 0);
                }

                //enable the wisp trail
                if(wispTrail != null)
                {
                    wispTrail.SetActive(true);
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

                //Disable the wisp trail if the plate is not set
                if (wispTrail != null)
                {
                    wispTrail.SetActive(false);
                }
            }
        }
        else
        {
            if (boss.activeInHierarchy)
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
            else
            {
                // move object
                if (moveUp)
                {
                    movingObjectGO.GetComponent<Rigidbody>().velocity = new Vector3(0, moveSpeed, 0);
                }
                else
                {
                    movingObjectGO.GetComponent<Rigidbody>().velocity = new Vector3(0, -moveSpeed, 0);
                }
            }
        }
        
    }
}

//------------------------------------------------------------------
//Scriptfor the sword throw aiming and recalling the sword
//Attach this script to the player and tag the object that needs to be thrown as Sword
//Check isKinematic and disable gravity
//ADjust the throw force as you need in the inspector
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAttackScript : MonoBehaviour
{
    // Start is called before the first frame update
    Transform throwDirection;
    [SerializeField] float throwForce = 10.0f;
    [SerializeField] GameObject sword;
    private bool isSwordThrown;
    private Rigidbody rb;
    private Collider swordCollider;
    Camera mainCamera;
    [SerializeField] private Transform throwPosition;

    void Start()
    {
        mainCamera = Camera.main;
        rb = sword.GetComponent<Rigidbody>();
        swordCollider = sword.GetComponent<Collider>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
        else
        {
            Debug.LogError("Rigidbody not found on the sword GameObject.");
        }
        swordCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isSwordThrown)
            {
                SwordRecall();
            }
            else
            {
                AimAndThrow();
            }
        }
    }

    void AimAndThrow()
    {
        //Getting the mouse position
        Vector3 mousePosition = Input.mousePosition;
 
        
        Ray rayCast = mainCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(rayCast, out RaycastHit hitInfo, Mathf.Infinity)) 
        {
            Vector3 direction = hitInfo.point - transform.position;
            direction.Normalize();
            direction.z = 0;

            //After getting the position of the mouse it calls the throw sword function to the trow the sword
            ThrowSword(direction);
        }
    }
    void ThrowSword(Vector3 direction)
    {

        GameObject thrownSword = Instantiate(sword, throwPosition.position, Quaternion.LookRotation(direction));
        rb = thrownSword.GetComponent<Rigidbody>();
        thrownSword.GetComponent<Collider>().enabled = true;
        

        if (rb != null)
        {
            sword.SetActive(false);
            rb.isKinematic = false;
            isSwordThrown = true;
        
            rb.AddForce(direction * throwForce, ForceMode.Impulse);
            Debug.Log("Sword throw script runs");
        }
        else
        {
            Debug.LogError("Rigidbody not found on the thrown sword GameObject.");
        }
    }
    void SwordRecall()
    {
        swordCollider.enabled = false;
    
        Destroy(GameObject.FindWithTag("Sword")); // Destroy the thrown sword 
        sword.SetActive(true);
        isSwordThrown = false;
    }
}

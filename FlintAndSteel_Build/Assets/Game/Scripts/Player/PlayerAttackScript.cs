//------------------------------------------------------------------
//Scriptfor the sword throw aiming and recalling the sword
//Attach this script to the player and tag the object that needs to be thrown as Sword
//Check isKinematic and disable gravity
//ADjust the throw force as you need in the inspector
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAttackScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float throwForce = 10.0f;
    [SerializeField] GameObject sword;
    private bool isSwordThrown;
    private Rigidbody rb;
    private Collider swordCollider;
    Camera mainCamera;
    [SerializeField] private Transform throwPosition;
    public bool steelFound;

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
        if (steelFound)
        {
            if (Input.GetMouseButtonDown(0) && GetComponent<Throwing>().pulling == false)
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

        UpdateSteelRenderer();
    }

    private void UpdateSteelRenderer()
    {
        //hide the sword if the player hasnt picked it up yet
        if (!steelFound && sword != null)
        {
            //Hide all renderers
            Renderer[] swordPieces = sword.GetComponentsInChildren<Renderer>();

            for (int i = 0; i < swordPieces.Length; i++)
            {
                swordPieces[i].enabled = false;
            }
        }
        else
        {
            //Show all renderers
            Renderer[] swordPieces = sword.GetComponentsInChildren<Renderer>();

            for (int i = 0; i < swordPieces.Length; i++)
            {
                swordPieces[i].enabled = true;
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
            //Setting a min distance
            float minThrowDistance = 1f;
            
            //Check to make sure the mouse is clicked a distance away from the player
            if (Vector3.Distance(hitInfo.point, transform.position) > minThrowDistance)
            {
                Vector3 direction = hitInfo.point - transform.position;
                direction.Normalize();
                direction.z = 0;

                //After getting the position of the mouse it calls the throw sword function to the trow the sword
                ThrowSword(direction);
            }

            else
            {
                Debug.Log("Mouse click too close to the character. Sword mnot thrown");
            }
        }
    }
    void ThrowSword(Vector3 direction)
    {
        Quaternion rotation = Quaternion.LookRotation(direction);
        //Sets the rotation in real time right before the sword is thrown
        rotation *= Quaternion.Euler(90,0, 0);
        GameObject thrownSword = Instantiate(sword, throwPosition.position, rotation);// Quaternion.LookRotation(direction));
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

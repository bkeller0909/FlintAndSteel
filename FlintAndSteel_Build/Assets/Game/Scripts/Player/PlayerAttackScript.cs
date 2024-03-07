using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
{
    [SerializeField] float throwForce = 10.0f;
    [SerializeField] GameObject sword;
    private bool isSwordThrown;
    private Rigidbody rb;
    private Collider swordCollider;
    private Camera mainCamera;
    [SerializeField] private Transform throwPosition;
    public bool steelFound;

    [SerializeField] Renderer[] swordRenderer;
    Material swordMaterial;

    private float fadeOutValue = 0f; // Initial fade out value
    private float fadeOutSpeed = 0.5f; // Speed of fade out

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

        swordMaterial = swordRenderer[0].material;
        SetFadeOutValue(0f); // Set initial fade out value
    }

    void Update()
    {
        if (steelFound)
        {
            if (Input.GetMouseButtonDown(0) && !GetComponent<Throwing>().pulling)
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
        if (!steelFound && sword != null)
        {
            SetFadeOutValue(0f); // Hide the sword if the player hasn't picked it up yet
        }
        else
        {
            SetFadeOutValue(1f); // Show the sword if the player has picked it up
        }
    }

    void AimAndThrow()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray rayCast = mainCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(rayCast, out RaycastHit hitInfo, Mathf.Infinity))
        {
            float minThrowDistance = 1f;
            
            if (Vector3.Distance(hitInfo.point, transform.position) > minThrowDistance)
            {
                Vector3 direction = hitInfo.point - transform.position;
                direction.Normalize();
                direction.z = 0;

                ThrowSword(direction);
            }
            else
            {
                Debug.Log("Mouse click too close to the character. Sword not thrown");
            }
        }
    }

    void ThrowSword(Vector3 direction)
    {
        Quaternion rotation = Quaternion.LookRotation(direction);
        rotation *= Quaternion.Euler(90, 0, 0);
        GameObject thrownSword = Instantiate(sword, throwPosition.position, rotation);
        rb = thrownSword.GetComponent<Rigidbody>();
        thrownSword.GetComponent<Collider>().enabled = true;

        if (rb != null)
        {
            sword.SetActive(false);
            rb.isKinematic = false;
            isSwordThrown = true;
            rb.AddForce(direction * throwForce, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("Rigidbody not found on the thrown sword GameObject.");
        }
    }

    void SwordRecall()
    {
        swordCollider.enabled = false;
        Destroy(GameObject.FindWithTag("Sword"));
        
        isSwordThrown = false;
        
        // Start fade-out coroutine when sword is recalled
        StartCoroutine(FadeOutSword());
        sword.SetActive(true);
        //Debug.LogWarning(fadeOutValue.ToString());
    }

    // Coroutine to gradually fade out the sword
   public IEnumerator FadeOutSword()
    {
        while (fadeOutValue < 1f)
        {
            fadeOutValue += fadeOutSpeed * Time.deltaTime;
            SetFadeOutValue(fadeOutValue);
           
            yield return null;
        }
        fadeOutValue = 0;
    }

    // Function to set the fade out value of the sword material
    void SetFadeOutValue(float value)
    {
        foreach (Renderer renderer in swordRenderer)
        {
            renderer.material.SetFloat("_FadeOut", value);
        }
    }
}

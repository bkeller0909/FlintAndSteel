using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerAttackScript : MonoBehaviour
{
    [SerializeField] float throwForce = 10.0f;
    [SerializeField] GameObject sword;
    public bool isSwordThrown;
    private Rigidbody rb;
    private Collider swordCollider;
    private Camera mainCamera;
    [SerializeField] private Transform throwPosition;
    public bool steelFound;

    [SerializeField] Renderer[] swordRenderer;
    Material swordMaterial;

    [SerializeField] Sprite[] steelIcons;
    [SerializeField] Image steelIconUI;

    private Animator animator;

    public LayerMask raycastLayerMask;

    private float fadeOutValue = 0f; // Initial fade out value
    private float fadeOutSpeed = 1.0f; // Speed of fade out

    [SerializeField] Image cursor;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        if (steelFound)
        {
            if (steelIconUI)
                steelIconUI.sprite = steelIcons[1];
        }
        else
        {
            if (steelIconUI)
                steelIconUI.sprite = steelIcons[0];
        }

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

        //SetFadeOutValue(0f); // Set initial fade out value
    }

    void Update()
    {
        if (steelFound)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Throw") && !GetComponent<Throwing>().pulling)
            {
                if (isSwordThrown)
                {
                    SwordRecall();
                }
                else if(this.GetComponent<PlayerZipline>().isZipping == false) // This stops Scally from throwing his sword on the zipline.
                {
                    AimAndThrow();
                }
            }

            if (isSwordThrown)
            {
                if (steelIconUI)
                    steelIconUI.sprite = steelIcons[0];
            }
            else
            {
                if (steelIconUI)
                    steelIconUI.sprite = steelIcons[1];
            }
        }

        // MoveCrosshair();
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
        

        if (!GameManager.Instance.usingController)
        {
            // For mouse aiming
            Vector3 mousePosition = Input.mousePosition;
            Ray rayCast = mainCamera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(rayCast, out RaycastHit hitInfo, Mathf.Infinity))
            {
                float minThrowDistance = 1f;

                hitInfo.point = new Vector3(hitInfo.point.x, hitInfo.point.y, 0);

                if (Vector3.Distance(hitInfo.point, transform.position) > minThrowDistance)
                { 
                    animator.SetTrigger("ThrowSword");

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
        else
        {
            // For the controller aiming
            Vector3 cursorPosition = cursor.rectTransform.position;
            Ray controllerRay = mainCamera.ScreenPointToRay(cursorPosition);
            if (Physics.Raycast(controllerRay, out RaycastHit hit, Mathf.Infinity))
            {
                float minThrowDistance = 1f;

                hit.point = new Vector3(hit.point.x, hit.point.y, 0);

                if (Vector3.Distance(hit.point, transform.position) > minThrowDistance)
                {
                    animator.SetTrigger("ThrowSword");
                    Vector3 direction = hit.point - transform.position;
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
    }
        
    /*public void MoveCrosshair()
    {
        
    }*/

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

    public void SwordRecall()
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
        fadeOutValue = 0;

        while (fadeOutValue < 1f)
        {
            fadeOutValue += fadeOutSpeed * Time.deltaTime;
            SetFadeOutValue(fadeOutValue);
           
            if (isSwordThrown) 
            {
                fadeOutValue = 1;
                SetFadeOutValue(1);
            }

            yield return null;
        }
        
    }

    // Function to set the fade out value of the sword material
    public void SetFadeOutValue(float value)
    {
        foreach (Renderer renderer in swordRenderer)
        {
            renderer.material.SetFloat("_FadeOut", value);
        }
    }
}

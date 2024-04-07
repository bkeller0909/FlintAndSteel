using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class Zipline : MonoBehaviour
{

    [SerializeField] private GameObject zipEffect; // zipline effect
    [SerializeField] private GameObject zipEffectSmoke; // Smoke
    private GameObject zipEffectClone;
    private GameObject zipEffectCloneSmoke;

    [Tooltip("Place the Zipline you want to go to if you use this Zipline")]
    [SerializeField] protected Zipline targetZip;

    [Tooltip("The speed in which the player moves down the Zipline")]
    [SerializeField] protected float zipSpeed = 5.0f;
    [Tooltip("The max ramp up speed of the player going down the Zipline")]
    [SerializeField] protected float MaxZipSpeed = 15.0f; // Not implemented yet

    [Tooltip("The step speed of going between currentVelocity and MaxZipSpeed")]
    [SerializeField] protected float zipStepSpeed = 3.0f;

    private float ModifiedZipSpeed = 0;

    [Tooltip("The Size of the cast sphere that checks if you are below the ZipTransform")]
    [SerializeField] protected float zipScale = 0.2f;

    [Tooltip("The distance you need to be from the target Zipline to stop using the Zipline automatically")]
    [SerializeField] protected float arrivalThreshold = 0.8f;

    [Tooltip("Slot for ZipTransform child of ZipLine Anchor")]
    public Transform zipTransform; // The transform point of the child of "Zipline Anchor" called "zipTransform"
    private Vector3 StartingPos = Vector3.zero;

    [Tooltip("Offset from the zipline the player rides")]
    public float offsetZip = -2.0f; // This could be moved. Not going to change it. - JRL

    [SerializeField] protected GameObject player;

    [SerializeField] private float playerZOffset;

    public bool zipping = false;

    private bool beginingOfZip = false; 
    private bool attachToZip = true;
    protected GameObject localZip;
    private float playerVelocityX = 0;
    private float savedZPosition = 0;


    // Update is called once per frame
    void Update()
    {
        
        if (!zipping || localZip == null)
            return;
        if (attachToZip)
        {
            AttachToZip();
        }
        else if (!attachToZip)
        {
            
            CheckIfNotZip();
            InitializeInitialMomentum();
            MoveThroughZip();
        }
    }

    private void CheckIfNotZip()
    {
        if (Vector3.Distance(localZip.transform.position, targetZip.zipTransform.position) <= arrivalThreshold || Input.GetButtonUp("Grab"))
        {
            
            if (targetZip.GetComponent<Rigidbody>().position.x < localZip.GetComponent<Rigidbody>().position.x) // If end position is left do this
            {
                ModifiedZipSpeed = -ModifiedZipSpeed;
            }
            
            ResetZipline();
        }
    }

    private void MoveThroughZip()
    {
        ModifiedZipSpeed = Mathf.Lerp(ModifiedZipSpeed, MaxZipSpeed, zipStepSpeed * Time.deltaTime);
        float step = ModifiedZipSpeed * Time.deltaTime;
        localZip.GetComponent<Rigidbody>().position = Vector3.MoveTowards(localZip.GetComponent<Rigidbody>().position, targetZip.GetComponent<Rigidbody>().position, step);
        zipEffectClone.transform.position = zipEffectCloneSmoke.transform.position = localZip.GetComponent<Rigidbody>().position;
        SetPlayerRotation(player); // keeps player rotation 
        player.GetComponent<Rigidbody>().position = Vector3.MoveTowards(player.GetComponent<Rigidbody>().position, localZip.GetComponent<Rigidbody>().position + new Vector3(0, offsetZip, 0), step);
    }

    private void InitializeInitialMomentum()
    {
        if (beginingOfZip)
        {
            if (targetZip.GetComponent<Rigidbody>().position.x > localZip.GetComponent<Rigidbody>().position.x && playerVelocityX > 0) // If end position is right do this
                ModifiedZipSpeed = zipSpeed + playerVelocityX;
            else if (targetZip.GetComponent<Rigidbody>().position.x < localZip.GetComponent<Rigidbody>().position.x && playerVelocityX < 0) // If end position is left do this
            {
                ModifiedZipSpeed = zipSpeed - playerVelocityX;
            }
            else
            {
                ModifiedZipSpeed = zipSpeed; // If Player velocity is zero then just make it zipspeed
            }
            beginingOfZip = false;
        }
    }

    private void AttachToZip()
    {
        if (player.GetComponent<Rigidbody>().position == localZip.GetComponent<Rigidbody>().position + new Vector3(0, offsetZip, 0))
        {
            player.GetComponent<Rigidbody>().position = Vector3.MoveTowards(player.GetComponent<Rigidbody>().position, localZip.GetComponent<Rigidbody>().position + new Vector3(0, offsetZip, 0), 16.0f * Time.deltaTime);
        }
        else
        {
            attachToZip = false;
        }
    }

    public void StartZippingRope(GameObject player)
    {
        if (zipping)
            return;
        player.GetComponent<PlayerZipline>().isZipping = true;

        localZip = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        localZip.GetComponentInChildren<Renderer>().enabled = false;
        // zipTransform
        // targetZip

        
        float distanceBetweenPoints = Vector3.Distance(zipTransform.position, targetZip.transform.position);
        float playerPosition = Vector3.Distance(zipTransform.position, player.transform.position);
        float ratio = playerPosition / distanceBetweenPoints;

        Vector3 newPosition = Vector3.Lerp(zipTransform.position, targetZip.transform.position, ratio);

        StartingPos = new Vector3(player.transform.position.x, newPosition.y, player.transform.position.z); // + 0.5f to y. This might be different for each zipline.

        localZip.transform.position = StartingPos;


        zipEffectClone = Instantiate(zipEffect, StartingPos, Quaternion.identity); // Create zipEffect copy at startingPos of player childed to local zip
        zipEffectCloneSmoke = Instantiate(zipEffectSmoke, StartingPos, Quaternion.identity);
        localZip.transform.localScale = new Vector3(zipScale, zipScale, zipScale);
        localZip.AddComponent<Rigidbody>().useGravity = false;
        localZip.GetComponent<Collider>().isTrigger = true;
        savedZPosition = player.GetComponent<Rigidbody>().position.z;
        playerVelocityX = player.GetComponent<Rigidbody>().velocity.x;
        player.GetComponent<Rigidbody>().useGravity = false;
        player.GetComponent<Rigidbody>().isKinematic = true;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<PlayerMove>().enabled = false;
        player.GetComponent<CharacterMotor>().enabled = false;
        SetPlayerRotation(player); // Sets the player rotation to the correct one given the trajectory
        player.transform.parent = localZip.transform;
        beginingOfZip = true;
        attachToZip = true;
        zipping = true;
    }

    private void SetPlayerRotation(GameObject player)
    {
        if (targetZip.transform.position.x > player.transform.position.x)
        {
            player.GetComponent<Rigidbody>().transform.eulerAngles = new Vector3(player.GetComponent<Rigidbody>().transform.eulerAngles.x, 90, player.GetComponent<Rigidbody>().transform.eulerAngles.z);
        }
        else
        {
            player.GetComponent<Rigidbody>().transform.eulerAngles = new Vector3(player.GetComponent<Rigidbody>().transform.eulerAngles.x, 270, player.GetComponent<Rigidbody>().transform.eulerAngles.z);
        }
    }

    protected void ResetZipline()
    {
        if (!zipping) 
            return;
        
        GameObject player = localZip.transform.GetChild(0).gameObject;
        player.GetComponent<PlayerZipline>().isZipping = false;
        zipEffectClone.GetComponent<ParticleSystem>().Stop();
        zipEffectCloneSmoke.GetComponent<ParticleSystem>().Stop();

        Destroy(zipEffectClone); // Destroys zipEffectClone ...
        Destroy(zipEffectCloneSmoke, 6f);
        
        player.GetComponent<Rigidbody>().useGravity = true;
        player.GetComponent<Rigidbody>().isKinematic = false;
        player.GetComponent<Rigidbody>().velocity = new Vector3(ModifiedZipSpeed, 0, 0);
        player.GetComponent<PlayerMove>().enabled = true; 
        player.GetComponent<CharacterMotor>().enabled = true;

        player.transform.parent = null;
        player.GetComponent<Rigidbody>().position = new Vector3(player.GetComponent<Rigidbody>().position.x, player.GetComponent<Rigidbody>().position.y, savedZPosition);
        savedZPosition = 0;
        Destroy(localZip);
        StartingPos = Vector3.zero;
        localZip = null;
        beginingOfZip = true;
        attachToZip = true;
        zipping = false;
    }
}

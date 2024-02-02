using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zipline : MonoBehaviour
{

    [Tooltip("Place the Zipline you want to go to if you use this Zipline")]
    [SerializeField] private Zipline targetZip;
    [Tooltip("The speed in which the player moves down the Zipline")]
    [SerializeField] private float zipSpeed = 50.0f;

    [Tooltip("The Size of the cast sphere that checks if you are below the ZipTransform")]
    [SerializeField] private float zipScale = 0.2f;

    [Tooltip("The distance you need to be from the target Zipline to stop using the Zipline automatically")]
    [SerializeField] private float arrivalThreshold = 0.4f;

    [Tooltip("Slot for ZipTransform child of ZipLine Anchor")]  
    public Transform zipTransform; // The transform point of the child of "Zipline Anchor" called "zipTransform"

    [Tooltip("Offset from the zipline the player rides")]
    public float offsetZip = -2.0f; // This could be moved. Not going to change it. - JRL

    [SerializeField] private GameObject player;

    private bool zipping = false;
    private GameObject localZip;
    
    



    // Update is called once per frame
    void Update()
    {
        if (!zipping || localZip == null)
            return;

        localZip.GetComponent<Rigidbody>().AddForce((targetZip.zipTransform.position - zipTransform.position).normalized * zipSpeed * Time.deltaTime, ForceMode.Acceleration);
        player.GetComponent<Rigidbody>().position = new Vector3(localZip.transform.position.x, localZip.transform.position.y + offsetZip, localZip.transform.position.z);


        if (Vector3.Distance(localZip.transform.position, targetZip.zipTransform.position) <= arrivalThreshold)
        {
            ResetZipline();
        }

        if (Input.GetButtonUp("Grab"))
        {
            ResetZipline();
        }
    }

    public void StartZipping(GameObject player)
    {
        if (zipping)
            return;
        localZip = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        localZip.transform.position = zipTransform.position;
        localZip.transform.localScale = new Vector3(zipScale, zipScale, zipScale);
        localZip.AddComponent<Rigidbody>().useGravity = false;
        localZip.GetComponent<Collider>().isTrigger = true;

        player.GetComponent<Rigidbody>().useGravity = false;
        player.GetComponent<Rigidbody>().isKinematic = true;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<PlayerMove>().enabled = false;
        player.GetComponent<CharacterMotor>().enabled = false;
        player.transform.parent = localZip.transform;
        zipping = true;
    }

    private void ResetZipline()
    {
        if (!zipping) 
            return;

        GameObject player = localZip.transform.GetChild(0).gameObject;
        player.GetComponent<Rigidbody>().useGravity = true;
        player.GetComponent<Rigidbody>().isKinematic = false;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<PlayerMove>().enabled = true; 
        player.GetComponent<CharacterMotor>().enabled = true;

        player.transform.parent = null;
        Destroy(localZip);
        localZip = null;
        zipping = false;
        Debug.Log("ZipLine has been reset");
    }
}

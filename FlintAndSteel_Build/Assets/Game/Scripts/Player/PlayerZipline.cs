using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZipline : MonoBehaviour
{
    [SerializeField] private float checkOffset = 1.0f;
    [SerializeField] private float checkRadius = 2.0f;

   

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Grab"))
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position + new Vector3(0, checkOffset, 0), checkRadius, Vector3.up);
            foreach (RaycastHit hit in hits)
            {
                if(hit.collider.tag == "Zipline")
                {
                    hit.collider.GetComponent<Zipline>().StartZipping(gameObject);
                }
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerZipline : MonoBehaviour
{
    [SerializeField] private float checkOffset = 1.0f;
    [SerializeField] private float checkRadius = 2.0f;
    [SerializeField] private float hookRangeZip = 2.0f;
   

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Grab"))
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position + new Vector3(0, checkOffset, 0), checkRadius, Vector3.up);
            foreach (RaycastHit hit in hits)
            {
                float distance = Mathf.Abs(this.transform.position.y - hit.collider.transform.position.y);
                if (hit.collider.tag == "ZipRope" && distance <= hookRangeZip)
                {
                    hit.collider.GetComponent<Zipline>().StartZippingRope(gameObject);
                }




                //if (hit.collider.tag == "Zipline" && distance <= hookRangeZip)
                //{
                //    hit.collider.GetComponent<Zipline>().StartZipping(gameObject);
                //}
                //else if(hit.collider.tag == "ZipRope" && distance <= hookRangeZip)
                //{
                //   hit.collider.GetComponent<Zipline>().StartZippingRope(gameObject);
                //}
            }
        }
    }
}

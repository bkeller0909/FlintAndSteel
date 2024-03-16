using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(0, 8.5f, -12.25f), ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ExplosionSpot") 
        {
            other.GetComponent<ExplosionSpot>().explosion = true;
            Destroy(gameObject);
        }
    }
}

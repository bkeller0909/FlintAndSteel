using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPickup : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(true);
    }

    private void OnTriggerStay(Collider other)
    {
        // If the player is in range and right clicks, he picked up Steel!
        if (other.CompareTag("Player"))
        {
            if (Input.GetMouseButtonDown(1))
            {
                other.gameObject.GetComponent<PlayerAttackScript>().steelFound = true;
                gameObject.SetActive(false);
            }
        }
    }
}

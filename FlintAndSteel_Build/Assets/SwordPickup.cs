using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPickup : MonoBehaviour
{
    bool inRange;
    GameObject playerGO;

    private void Start()
    {
        gameObject.SetActive(true);
    }

    private void Update()
    {
        // If the player is in range and right clicks, he picked up Steel!
        if (Input.GetMouseButtonDown(0) && inRange)
        {
            playerGO.gameObject.GetComponent<PlayerAttackScript>().steelFound = true;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            playerGO = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
        }
    }
}

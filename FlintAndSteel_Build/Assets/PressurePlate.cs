using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public bool activated;

    // Start is called before the first frame update
    void Start()
    {
        activated = false;    
    }

    private void OnTriggerStay(Collider other)
    {
        // if the player or boulder goes onto the pressure plate, activate
        if (other.CompareTag("Player") || other.CompareTag("Pushable"))
        {
            activated = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // if the player or boulder leaves, deactivate
        if (other.CompareTag("Player") || other.CompareTag("Pushable"))
        {
            activated = false;
        }
    }
}

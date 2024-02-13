using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWall : MonoBehaviour
{
    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0, 2, 0);
    }
}

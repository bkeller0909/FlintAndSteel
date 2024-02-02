using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSword : MonoBehaviour
{
    private PlayerMove playerMove;

    // increase jump height while standing on the sword
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerMove = collision.gameObject.GetComponent<PlayerMove>();
            playerMove.jumpForce.y = playerMove.jumpForce.y + 2;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerMove = collision.gameObject.GetComponent<PlayerMove>();
            playerMove.jumpForce.y = playerMove.jumpForce.y - 2;
        }
    }

    private void OnDestroy()
    {
        if (playerMove != null)
        {
            playerMove.jumpForce.y = 13;
        }
    }
}

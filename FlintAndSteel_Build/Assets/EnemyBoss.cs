using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBoss : MonoBehaviour
{
    private bool beginFight;
    Rigidbody rb;

    [SerializeField] float jumpStrength;
    [SerializeField] float jumpDistance;

    [SerializeField] GameObject cannonBall;
    [SerializeField] Transform firePosition;

    public enum attackType { jumpLeft, jumpRight, jumpUp, shootTowards, shootUp }

    private GameObject player;
    private int lastAttack;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (beginFight)
        {
            JumpLeft();
            Debug.Log("began");
        }

        AttackPattern();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            beginFight = true;
            player = other.gameObject;
        }
    }

    private void JumpLeft()
    {
        rb.AddForce(new Vector3(-jumpDistance, jumpStrength, 0));
        lastAttack = (int)attackType.jumpLeft;
        beginFight = false;
        Debug.Log("jump left");
    }

    private void JumpRight() 
    {
        rb.AddForce(new Vector3(jumpDistance, jumpStrength, 0));
        lastAttack = (int)attackType.jumpRight;
        Debug.Log("jump right");
    }
    private void JumpUp() 
    {
        rb.AddForce(new Vector3(0, jumpStrength * 1.33f, 0));
        lastAttack = (int)attackType.jumpUp;
    }

    private void ShootTowards()
    {
        if (player.transform.position.x < transform.position.x) 
        {
            GameObject cannonBallClone = Instantiate(cannonBall, firePosition.position, Quaternion.identity);
            cannonBallClone.GetComponent<Rigidbody>().velocity = new Vector3(-10, 0, 0);
        }
        else
        {
            GameObject cannonBallClone = Instantiate(cannonBall, firePosition.position, Quaternion.identity);
            cannonBallClone.GetComponent<Rigidbody>().velocity = new Vector3 (10, 00, 0);
        }

        lastAttack = (int)attackType.shootTowards;
    }
    private void ShootUp()
    {
        GameObject cannonBallClone = Instantiate(cannonBall, firePosition.position, Quaternion.identity);
        cannonBallClone.GetComponent<Rigidbody>().velocity = new Vector3(1, 8, 0);
        GameObject cannonBallClone2 = Instantiate(cannonBall, firePosition.position, Quaternion.identity);
        cannonBallClone2.GetComponent<Rigidbody>().velocity = new Vector3(0, 8, 0);
        GameObject cannonBallClone3 = Instantiate(cannonBall, firePosition.position, Quaternion.identity);
        cannonBallClone3.GetComponent<Rigidbody>().velocity = new Vector3(-1, 8, 0);

        lastAttack = (int)attackType.shootUp;
    }

    private void AttackPattern()
    {
        if (lastAttack == (int)attackType.jumpLeft)
        {
            int rand = Random.Range(1, 4);
            if (rand == (int)attackType.jumpRight)
            {
              
            }
            else if (rand == (int)attackType.jumpUp) 
            {
                
            }

        }
    }
}

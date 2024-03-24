using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowd : MonoBehaviour
{
    Animator animator;

    float startDelay;

    [SerializeField] GameObject boss;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        startDelay = Random.Range(0.0f, 0.25f);

        animator.SetFloat("SpdMult", Random.Range(0.4f, 0.75f));
  
    }


    private void Update()
    {
        startDelay -= Time.deltaTime;

        if (boss != null ) 
        {
            if (startDelay < 0.0f && boss.GetComponent<EnemyBoss>().health > 0) 
            {
                animator.SetBool("Cheering", true);
            }
            else
            {
                animator.SetBool("Cheering", false);
            }
        }
        else
        {
            animator.SetBool("Cheering", false);
        }

    }
}

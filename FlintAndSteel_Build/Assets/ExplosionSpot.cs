using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSpot : MonoBehaviour
{
    [SerializeField] GameObject shootingEnemy;

    public bool explosion = false;

    public void startTrigger()
    {
        shootingEnemy.GetComponent<ShootEnemy>().Shoot();
    }
}


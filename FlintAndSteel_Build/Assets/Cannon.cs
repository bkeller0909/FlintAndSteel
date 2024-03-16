using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] Transform cannonBarrel;
    [SerializeField] Transform fireLocation;

    [SerializeField] ParticleSystem shotParticles;
    [SerializeField] GameObject explosionParticles;
    [SerializeField] GameObject explosionSound;

    [SerializeField] GameObject cannonBall;

    [Header("Cannon Stats")]
    [SerializeField] float shootSpeed;

    private AudioSource aSource;
    private float shotTimer;

    bool shot;

    private void Start()
    {
        aSource = GetComponent<AudioSource>();

        shotTimer = shootSpeed;
    }

    private void Update()
    {
        shotTimer -= Time.deltaTime;

        if (shotTimer <= 0 ) 
        {
            Shoot();
        }

        Explosion();
    }

    private void Shoot()
    {
        if (!shot)
        {
            shotParticles.gameObject.transform.position = cannonBarrel.position;
            shotParticles.Play();
            shot = true;

            Instantiate(cannonBall, cannonBarrel.position, Quaternion.identity);

            aSource.pitch = Random.Range(0.75f, 0.95f);
            aSource.Play();
        }  
    }

    private void Explosion()
    {
        if (fireLocation.GetComponent<ExplosionSpot>().explosion)
        {
            Instantiate(explosionParticles, fireLocation.position, Quaternion.identity);
            Instantiate(explosionSound, fireLocation.position, Quaternion.identity);

            shotTimer = shootSpeed;
            shot = false;
            fireLocation.GetComponent<ExplosionSpot>().explosion = false;
        }
    }
}

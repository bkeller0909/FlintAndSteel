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

    [Header("Cannon Stats")]
    [SerializeField] float shootSpeed;
    [SerializeField] float timeToExplode;

    private AudioSource aSource;
    private float shotTimer;
    private float explosionTimer;

    bool shot;

    private void Start()
    {
        aSource = GetComponent<AudioSource>();

        shotTimer = shootSpeed;
        explosionTimer = timeToExplode;
    }

    private void Update()
    {
        shotTimer -= Time.deltaTime;

        if (shotTimer <= 0 ) 
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (!shot)
        {
            shotParticles.gameObject.transform.position = cannonBarrel.position;
            shotParticles.Play();
            shot = true;

            aSource.pitch = Random.Range(0.75f, 0.95f);
            aSource.Play();
        }

        explosionTimer -= Time.deltaTime;

        if (explosionTimer <= 0 )
        {
            Instantiate(explosionParticles, fireLocation.position, Quaternion.identity);
            Instantiate(explosionSound, fireLocation.position, Quaternion.identity);

            shotTimer = shootSpeed;
            explosionTimer = timeToExplode;
            shot = false;
        }    
    }
}

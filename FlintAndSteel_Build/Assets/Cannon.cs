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

    [SerializeField] bool activated = false;

    [Header("Cannon Stats")]
    [SerializeField] float shootSpeed;
    [SerializeField] float startDelay;

    private AudioSource aSource;
    private float shotTimer;
    private float delayTimer;

    bool shot;

    //Stuff for the shader
    [SerializeField] Renderer[] cannonRender;
    private float fadeOutValue = 1.0f; // Initial fade out value
    private float fadeOutSpeed = 1.0f; // Speed of fade out

    bool coolingDown = false;

    private void Awake()
    {
        aSource = GetComponent<AudioSource>();

        delayTimer = startDelay;
        shotTimer = shootSpeed;
    }

    private void Start()
    {
        aSource = GetComponent<AudioSource>();

        shotTimer = shootSpeed;
    }

    private void Update()
    {
        if (activated)
        {
            delayTimer -= Time.deltaTime;

            if (delayTimer <= 0)
            {
                shotTimer -= Time.deltaTime;

                // Update fade out value based on shoot timer
                if (coolingDown == true)
                {
                    fadeOutValue = Mathf.Lerp(0.0f, 1.0f, 0.7f - shotTimer / shootSpeed);
                    if (fadeOutValue <= 0)
                    {
                        coolingDown = false;
                    }
                }
                else
                {
                    fadeOutValue = Mathf.Lerp(1.0f, 0.0f, 0.7f - shotTimer / shootSpeed);
                }

                // Update shader
                SetFadeOutValue(fadeOutValue);

                if (shotTimer <= 0)
                {
                    Shoot();
                    coolingDown = true;
                }
            }

            Explosion();
        }
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
            coolingDown = false;
            fadeOutValue = 1.0f;
            fireLocation.GetComponent<ExplosionSpot>().explosion = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            activated = true;
        }
    }

    public void SetFadeOutValue(float value)
    {
        foreach (Renderer renderer in cannonRender)
        {
            renderer.material.SetFloat("_glowTime", value);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedCannon : MonoBehaviour
{
    [SerializeField] float lifeTime;
    [SerializeField] GameObject disapearParticles;

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0 )
        {
            Instantiate(disapearParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}

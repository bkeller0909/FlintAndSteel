using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] protected float speed = 5f;
    [SerializeField] float lifeTime = 5f;
    protected Vector3 direction;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void Fire(Vector3 dir)
    {
        direction = dir;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
            Destroy(gameObject);
        else if (other.CompareTag("Player")) // when hitting a player it will be destroyed similiarly to hitting walls
            Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    #region serialized or public variables
    [SerializeField]
    private Transform player;

    [SerializeField]
    private float idleDuration = 5f;

    private float idleStartDuration;
    public enum BossActionType
    {
        Idle,
        Moving,
        Attacking
    }

    [SerializeField]
    private float movementSpeed = 10.0f;

    //Dash variables
    [SerializeField]
    private float dashDistance = 5.0f;
    [SerializeField]
    private float dashSpeed = 30.0f;

    [SerializeField]
    private float chaseDuration = 5.0f;

    private float shootTimer;

    [SerializeField]
    private Transform Barrel;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private float shootInterval = 2.0f;

    [SerializeField]
    private float shootDuration = 5.0f;

    [SerializeField]
    private GameObject characterModel;

    [SerializeField]
    private ParticleSystem chargeUpParticles;
    [SerializeField]
    private ParticleSystem shootParticles;

    [SerializeField]
    private AudioClip shotSound;

    #endregion

    //Intializes the boss variable to idle in the beggining of the game
    private BossActionType eCurState = BossActionType.Idle;

    private bool isVulnerable = true;

    bool isMoving = false;

    bool isAway = false;

    bool isAttacking = false;

    private float health = 100;
    private Vector3 intitalPosition;

    private Animator animator;

    private int dashCount = 0;

    int flipDirection = 1;

    Vector3 startScale;
    Vector3 characterStartScale;

    private AudioSource audioSource;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponentInChildren<AudioSource>();

        startScale = transform.localScale;
        characterStartScale = characterModel.transform.localScale;

        chargeUpParticles.Stop();

        idleStartDuration = idleDuration;
    }

    private void Update()
    {
        //Updates the boss's state    
        switch (eCurState)
        {
            case BossActionType.Idle:
                HandleIdleState();
                break;

            case BossActionType.Moving:
                HandleMovingState();
                break;

            case BossActionType.Attacking:
                HandleAttackingState();
                break;
        }
    }

    private void HandleIdleState()
    {
        StartCoroutine(IdleState());
        isVulnerable = true;
    }

    private void HandleMovingState()
    {
        if (!isMoving)
            StartCoroutine(MoveTowardsPlayerTimer(chaseDuration));

        isVulnerable = false;

    }

    private void HandleAttackingState()
    {
        float distanceToPlayer = MoveTowardsPlayer();

        if (!isAttacking)
        {
            ShootTimer();
        }

        isVulnerable = false;
    }

    private void TakeDamage(float damage)
    {
        if (isVulnerable)
        {
            health -= damage;
            if (health <= 0)
            {
                //Boss is defeated
                gameObject.SetActive(false);
            }

            idleDuration = 0.1f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            TakeDamage(10.0f);
            Debug.LogWarning("The boss has taken 10 damage");
            Debug.LogWarning("Health left: " + health);
        }

        if (other.CompareTag("Player"))
        {
            //attack anim
            animator.SetTrigger("ThrowSword");
        }
    }

    private IEnumerator IdleState()
    {
        yield return new WaitForSeconds(idleDuration);
        animator.SetBool("Cooldown", false);
        eCurState = BossActionType.Attacking;
    }

    private IEnumerator MoveTowardsPlayerTimer(float duration)
    {
        isMoving = true;
        float timer = 0f;

        while (timer < duration)
        {
            MoveTowardsPlayer();
            timer += Time.deltaTime;
            yield return null;
        }

        isMoving = false;
    }


    private float MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > 1.0f)
        {
            // Uncomment this line to make boss move towards player
            // transform.Translate(direction * Mathf.Min(movementSpeed * Time.deltaTime, distanceToPlayer - 1.0f));
        }

        return distanceToPlayer;
    }

    private IEnumerator DashAttack(Vector3 direction, float distance, float speed)
    {
        isAttacking = true;
        transform.localScale = startScale - new Vector3(0, startScale.y * 0.1f, 0);
        float distanceCovered = 19.0f;
        float distancePerPoint = distance / distanceCovered;
        direction = new Vector3(direction.x, 0, 0).normalized;

       
        for (int i = 0; i < distanceCovered; i++)
        {
            Vector3 nextPoint = transform.position + direction * distancePerPoint * (i + 1);

            while (Vector3.Distance(transform.position, nextPoint) > 0.1f)
            {
                Vector3 movement = direction * speed * Time.deltaTime;

                if (Vector3.Distance(transform.position, nextPoint) < movement.magnitude)
                    transform.position = nextPoint;
                else
                    transform.Translate(movement);

                yield return null;
            }
        }

        flipDirection *= -1;
        characterModel.transform.localScale = new Vector3(characterStartScale.x, characterStartScale.y, characterStartScale.z * flipDirection);
        
        //transform.rotation = targetRotation;
        dashCount++;
        transform.localScale = startScale;
        isAttacking = false;
        idleDuration = idleStartDuration;
        animator.SetBool("Cooldown", true);
        eCurState = BossActionType.Idle;
        
    }



    private void Shoot()
    {
        isAttacking = true;
        Vector3 dir = (player.position - Barrel.position).normalized;
        dir.y = 0;
        dir.z = 0;

        audioSource.pitch = Random.Range(1.15f, 1.35f);
        audioSource.clip = shotSound;
        audioSource.Play();

        GameObject bulletGo = Instantiate(bulletPrefab, Barrel.position, Quaternion.identity);
        EnemyBullet bulletScript = bulletGo.GetComponent<EnemyBullet>();
        bulletScript.SetSpeed(10.0f);
        bulletScript.Fire(dir);

        StartCoroutine(DashAfterShootingTimer(2.0f)); // Dash after shooting for 10 seconds
    }

    private void ShootTimer()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer < 0.0f && bulletPrefab != null)
        {
            float randomNumber = Random.Range(0.0f, 1.0f);
            if (randomNumber < 0.13f)
            {
                Shoot();
                Debug.Log("NormalShot");
            }
            else if (randomNumber >= 0.13 && randomNumber < 0.55f)
            {
                DoubleShot();
                Debug.Log("DoubleShot");
            }
            else
            {
                ScatterShot();
                Debug.Log("ScatterShot");
            }

            if (health <= 40f)
            {
                shootTimer = shootInterval - (shootInterval * 0.3f);
            }
            else
            {
                shootTimer = shootInterval;
            }
        }
    }

    private void ScatterShot()
    {
        Vector3[] directions = new Vector3[3];
        Vector3 playerDirection = (player.position - Barrel.position).normalized;

        StartCoroutine(ScatterShotSound(shotSound));
        directions[0] = playerDirection;
        directions[1] = Quaternion.Euler(0, 0, 30) * playerDirection;
        directions[2] = Quaternion.Euler(0, 0, -30) * playerDirection;

        foreach (Vector3 dir in directions)
        {
            GameObject bulletGo = Instantiate(bulletPrefab, Barrel.position, Quaternion.identity);
            EnemyBullet bulletScript = bulletGo.GetComponent<EnemyBullet>();
            bulletScript.SetSpeed(10);
            bulletScript.Fire(dir);
        }
    }

    private void DoubleShot()
    {
        Vector3[] directions = new Vector3[2];
        Vector3 playerDirection = (player.position - Barrel.position).normalized;

        StartCoroutine(DoubleShotSound(shotSound));
        directions[0] = playerDirection;
        directions[1] = playerDirection;

        float heightOffset = -0.75f;
        foreach (Vector3 dir in directions)
        {
            GameObject bulletGo = Instantiate(bulletPrefab, Barrel.position + new Vector3(0, heightOffset, 0), Quaternion.identity);
            EnemyBullet bulletScript = bulletGo.GetComponent<EnemyBullet>();
            bulletScript.SetSpeed(10);
            bulletScript.Fire(dir);
            heightOffset = 0.75f;
        }
    }

    private IEnumerator DashAfterShootingTimer(float duration)
    {
        chargeUpParticles.Play();
        yield return new WaitForSeconds(duration);
        // Call DashAttack after shooting for 10 seconds
        Vector3 direction = (player.position - transform.position).normalized;
        chargeUpParticles.Stop();
        StartCoroutine(DashAttack(direction, dashDistance, dashSpeed));
        
    }

    private IEnumerator ScatterShotSound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.pitch = Random.Range(1.15f, 1.35f);
     
        audioSource.Play();
        yield return new WaitForSeconds(0.075f);
        audioSource.Play();
        yield return new WaitForSeconds(0.075f);
        audioSource.Play();
    }

    private IEnumerator DoubleShotSound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.pitch = Random.Range(1.15f, 1.35f);

        audioSource.Play();
        yield return new WaitForSeconds(0.075f);
        audioSource.Play();
    }
}

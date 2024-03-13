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
    
    #endregion

    //Intializes the boss variable to idle in the beggining of the game
    private BossActionType eCurState = BossActionType.Idle;

    private bool isVulnerable = true;

    bool isMoving = false;

    bool isAway = false;

    bool isAttacking = false;

    private float health = 100;
    private Vector3 intitalPosition;

    [SerializeField]
  //  private Material bossMaterial;

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

        //Sets the boss color to white to show weakness
    //    bossMaterial.color = Color.white;

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
        {//Calls the shoot function
            ShootTimer();
        }
        if (distanceToPlayer <= dashDistance && !isAttacking)
        {
            //Calls the dash function
            Vector3 direction = (player.position - transform.position).normalized;

            StartCoroutine(DashAttack(direction, dashDistance, dashSpeed));

        }
        isVulnerable = false;
      //  bossMaterial.color = Color.red;

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
    }
    private IEnumerator IdleState()
    {
        //Makes the boss stay in the idle state for the duation needed.
        //Potentially make it so that the boss only gets damaged when he is in the idle state

        yield return new WaitForSeconds(idleDuration); // Adjust the duration as needed

        eCurState = BossActionType.Attacking;
    }
    /// <summary>
    /// Adds a timer so that the boss only chasers the player as longa as the duration is set
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    private IEnumerator MoveTowardsPlayerTimer(float duration)
    {
        isMoving = true;
        float timer = 0f;

        while (timer < duration)
        {
            MoveTowardsPlayer();    //Calls the function to make the boss follow the player around
            timer += Time.deltaTime;
            yield return null;
        }

        // After the specified duration, transition to the idle state
        isMoving = false;
    }


    /// <summary>
    /// Handles the logic for moving the boss towards the player. It also returns the position of the player
    /// as a float so that it can be used later in other functions as well
    /// </summary>
    /// <returns></returns>
    /// 
    private float MoveTowardsPlayer()
    {

        Vector3 direction = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > 1.0f)
        {
            //  transform.Translate(direction * Mathf.Min(movementSpeed * Time.deltaTime, distanceToPlayer - 1.0f));
        }


        if (distanceToPlayer < 3.0f)
        {
            eCurState = BossActionType.Attacking;

        }
        return distanceToPlayer;
    }

    /// <summary>
    /// Performs the dash attack of the boss
    /// It takes in 3 variables, the position of the player, the dash distance and the speed of the dash
    /// </summary>>
    /// <returns></returns>
    private IEnumerator DashAttack(Vector3 direction, float distance, float speed)
    {
        //The distance travelled while dashing
        isAttacking = true;
        int distanceCovered = 15;
        float distancePerPoint = distance / distanceCovered;
        direction = new Vector3(direction.x, 0, 0).normalized;

        for (int i = 0; i < distanceCovered; i++)
        {
            Vector3 nextPoint = transform.position + direction * distancePerPoint * (i + 1);


            while (Vector3.Distance(transform.position, nextPoint) > 0.1f)
            {
                //The amount the dash will move each frame
                // Calculate movement towards the next point
                Vector3 movement = direction * speed * Time.deltaTime;

                // Ensure that the movement doesn't overshoot the next point
                if (Vector3.Distance(transform.position, nextPoint) < movement.magnitude)
                    transform.position = nextPoint; // Snap to the next point
                else
                    transform.Translate(movement); // Move towards the next point

                yield return null;
            }
        }
        isAttacking = false;

        eCurState = BossActionType.Idle;

    }


    private void Shoot()
    {
        isAttacking = true;
        Vector3 dir = (player.position - Barrel.position).normalized;
        dir.y = 0;
        dir.z = 0;

        GameObject bulletGo = Instantiate(bulletPrefab, Barrel.position, Quaternion.identity);
        EnemyBullet bulletScript = bulletGo.GetComponent<EnemyBullet>();
        bulletScript.SetSpeed(10.0f);
        bulletScript.Fire(dir);

        isAttacking = false;

    }

    private void ShootTimer()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer < 0.0f && bulletPrefab != null)
        {
            float randomNumber = Random.value;  //Assigns a random value to the random Number
            if (randomNumber < 0.5f)    //50% chacne for a normal shot or random shot
            {
                Debug.LogWarning("SingleShot");
                Shoot();
            }
            else
            {
                Debug.LogWarning("ScatterShot");
                ScatterShot();
            }
            shootTimer = shootInterval;
        }
    }

    private void ScatterShot()
    {
        // Array to store directions
        Vector3[] directions = new Vector3[3];
        Vector3 playerDirection = (player.position - Barrel.position).normalized;
        // Calculate directions

        // Set predetermined directions
        // Calculate directions
        //  Vector3 barrelForward = Barrel.forward;

        // Direction 1: Forward
        directions[0] = playerDirection;

        // Direction 2: 
        directions[1] = Quaternion.Euler(0, 0, 30) * playerDirection;

        // Direction 3: 
        directions[2] = Quaternion.Euler(0, 0, -30) * playerDirection;

        // Fire bullets in each direction 
        foreach (Vector3 dir in directions)
        {
            // Instantiate bullet
            GameObject bulletGo = Instantiate(bulletPrefab, Barrel.position, Quaternion.identity);
            EnemyBullet bulletScript = bulletGo.GetComponent<EnemyBullet>();
            bulletScript.SetSpeed(10);
            bulletScript.Fire(dir);
        }

    }
}



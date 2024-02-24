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
        moving,
        Attacking
    }

    [SerializeField]
    private float movementSpeed = 10.0f;

    //Dash variables
    [SerializeField]
    private float dashDistance = 5.0f;
    [SerializeField]
    private float dashSpeed = 15.0f;

    [SerializeField]
    private float chaseDuration = 5.0f;

    #endregion

    //Intializes the boss variable to idle in the beggining of the game
    private BossActionType eCurState = BossActionType.Idle;

    private void Update()
    {
        //Updates the boss's state    
        switch (eCurState)
        {
            case BossActionType.Idle:
                HandleIdleState();
                break;

            case BossActionType.moving:
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

    }

    private void HandleMovingState()
    {
        // StartCoroutine( MoveTowardsPlayerTimer(chaseDuration));
        MoveTowardsPlayer();
    }

    private void HandleAttackingState()
    {
        float distanceToPlayer = MoveTowardsPlayer();

        if (distanceToPlayer < dashDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            StartCoroutine(DashAttack(direction, dashDistance, dashSpeed));

        }


        eCurState = BossActionType.Idle;
    }

    private IEnumerator IdleState()
    {
        //Makes the boss stay in the idle state for the duation needed.
        //Potentially make it so that the boss only gets damaged when he is in the idle state

        yield return new WaitForSeconds(idleDuration); // Adjust the duration as needed

        eCurState = BossActionType.moving;
    }
    /// <summary>
    /// Handles the logic for moving the boss towards the player. It also returns the position of the player
    /// as a float so that it can be used later in other functions as well
    /// </summary>
    /// <returns></returns>
    /// 
    //private IEnumerator MoveTowardsPlayerTimer(float duration)
    //{
    //    float timer = 0f;

    //    while (timer < duration)
    //    {
    //        MoveTowardsPlayer();    //Calls the function to make the boss follow the player around
    //        timer += Time.deltaTime;
    //        yield return null;
    //    }

    //    // After the specified duration, transition to the idle state
    //   // eCurState = BossActionType.Idle;
    //}

    private float MoveTowardsPlayer()
    {

        Vector3 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * movementSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, player.position) < 3.0f)
        {
            eCurState = BossActionType.Attacking;

        }
        return Vector3.Distance(transform.position, player.position);
    }

    /// <summary>
    /// Performs the dash attack of the boss
    /// It takes in 3 variables, the position of the player, the dash distance and the speed of the dash
    /// </summary>>
    /// <returns></returns>
    private IEnumerator DashAttack(Vector3 direction, float distance, float speed)
    {
        //The distance travelled while dashing
        float distanceTravelled = 0.0f;

        while (distanceTravelled < dashDistance)
        {
            //The amount the dash will move each frame
            float distancePerFrame = dashSpeed * Time.deltaTime;

            //Move the boss itself
            transform.Translate(direction * distancePerFrame);

            //Increments the distance travelled
            distanceTravelled += distancePerFrame;

            yield return null;
        }
        Vector3 awayDirection = (transform.position - player.position).normalized;
        yield return StartCoroutine(MoveAwayFromPlayer(awayDirection, 3.0f, movementSpeed));

        eCurState = BossActionType.Idle;


    }

    private IEnumerator MoveAwayFromPlayer(Vector3 direction, float distance, float speed)
    {
        Debug.LogError("MoveAwayWorks");
        float distanceTravelled = 0.0f;

        while (distanceTravelled < distance)
        {
            float distancePerFrame = speed * Time.deltaTime;
            transform.Translate(direction * distancePerFrame);
            distanceTravelled += distancePerFrame;
            yield return null;

        }
    }
}

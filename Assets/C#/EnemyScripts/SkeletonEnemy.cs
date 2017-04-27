using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyStateController))]
public class SkeletonEnemy : PatrolGroundEnemy
{
    public Transform target;
    public float lookSphereRadius = 15f; //use for draw Gizmo, and detect target
    public float distanceMeleeAttack = 2f;
    public float timeBetweenAttacks = 2f;
    public float attackDamage = 10;
    public Hittable.DamageType attackType = Hittable.DamageType.Neutral;
    public bool isAllowedToAttack;

    [HideInInspector] public float stateTimeElapsed;
    [HideInInspector] public Animator animator;

    public bool isAttacking;
    public bool isWalking;
    public bool isRunning;
    public bool isDancing;
    private bool aiActive;
    private EnemyStateController stateController;


    //some const because I am too lazy typing them  
    private const string IS_TURNING_LEFT = "isTurningLeft";
    private const string IS_TURNING_RIGHT = "isTurningRight";

    public readonly int HASH_IS_RUNNING         = Animator.StringToHash("isRunning");
    public readonly int HASH_IS_WALKING         = Animator.StringToHash("isWalking");
    public readonly int HASH_TRIGGER_SCREAMING  = Animator.StringToHash("TriggerScreaming");
    public readonly int HASH_IDLE_TAG           = Animator.StringToHash("Idle");
    public readonly int HASH_TRIGGER_DEATH      = Animator.StringToHash("TriggerDeath");
    public readonly int HASH_TRIGGER_DANCE      = Animator.StringToHash("TriggerDance");
    public readonly int HASH_TRIGGER_ATTACK_RUN = Animator.StringToHash("TriggerAttackRun");
    public readonly int HASH_IS_DANCING         = Animator.StringToHash("isDancing");

    public const string PLAYER_TAG = "Player";




    // Use this for initialization
    void Start()
    {
        //use this for PatrolAround
        base.__init__();

        //set center: skeleton won't fall back & forth
        rb.centerOfMass = new Vector3(0, -10, 0);

        //set up some variables
        animator = GetComponent<Animator>();
        curPatrolIndex = 0;

        //set up AI
        aiActive = true;
        stateController = GetComponent<EnemyStateController>();
        stateController.SetupStateController(this);
    }



    public override IEnumerator Attack()
    {
        return null;
    }

  
    public bool isCloseEnoughToTarget(Vector3 target, float distance)
    {
        //calculate the distance
        Vector3 distanceVect = target - transform.position;
        float tempdistance = Vector3.Magnitude(distanceVect);

        //simple check.
        return tempdistance <= distance;
    }


    public bool isFarEnoughFromTarget(Vector3 target, float distance)
    {
        //calculate the distance
        Vector3 distanceVect = target - transform.position;
        float tempdistance = Vector3.Magnitude(distanceVect);

        //simple check.
        return tempdistance > distance;
    }


    /**
     * pretty cool animation made by Andrew
     * lol
     */
    public void Scream()
    {
        animator.SetTrigger(HASH_TRIGGER_SCREAMING);
    }

    public override void Movement()
    {

        if (!aiActive)
            return;

        this.stateController.UpdateStateController();
    }


    

    /*
     * MoveToward()
     * purely move Rigibody towards target
     * no animation involved
     */ 
    public void MoveToward(Vector3 target, float speed)
    {

        if (!isFacingTarget(target) && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Detected"))
        {
            RotateTowardsTarget(target);
        }
        else
        {
            transform.LookAt(target);
            Vector3 forward =
                transform.position + transform.forward * Time.deltaTime * speed;
            if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Detected")) {
                rb.MovePosition(forward);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == PLAYER_TAG && target == null && health > 0)
        {
            //assign target
            target = other.transform;
            Scream();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (target == null || other.gameObject.tag != PLAYER_TAG)
            return;

        //a hack way to make sure that target has to be far enough
        //before let target go
        if (isFarEnoughFromTarget(target.position, lookSphereRadius))
        {
            target = null;
        }
    }


    public override void OnDeath()
    {
        aiActive = false;
        animator.SetTrigger(HASH_TRIGGER_DEATH);
    }
    public override void OnDamage(float damage) {
        // Animation for taking damage
        if (health > 0 && !animator.GetCurrentAnimatorStateInfo(1).IsTag("Damage")) {
            animator.SetTrigger("Damage");

        }
    }
    public bool isIdleState()
    {
        return animator.GetCurrentAnimatorStateInfo(0).tagHash == HASH_IDLE_TAG;
    }

  
    /*
     * StartWalking()
     * return true: start successfully
     * return false: when skeleton not ready
     */ 
    public bool StartWalkingAnimation()
    {
        //if walking already, then don't have to do anything
        if (isWalking)
            return true;
         
        //if not walking, wait until Skeleton is ready
        if (!isIdleState())
            return false;

        //if idle state
        animator.SetBool(HASH_IS_WALKING, true);
        isWalking = true;
        return true;
    }


    public void StopWalkingAnimation()
    {
        isWalking = false;
        animator.SetBool(HASH_IS_WALKING, false);
    }

    public bool StartRunningAnimation()
    {
        //if walking already, then don't have to do anything
        if (isRunning)
            return true;

        //if not walking, wait until Skeleton is ready
        if (!isIdleState())
            return false;

        //if idle state
        animator.SetBool(HASH_IS_RUNNING, true);
        isRunning = true;
        return true;
    }

    public void StopRunningAnimation()
    {
        isRunning = false;
        animator.SetBool(HASH_IS_RUNNING, false);
    }

    public void StartDancingAnimation()
    {
        isDancing = true;
        animator.SetTrigger(HASH_TRIGGER_DANCE);
        animator.SetBool(HASH_IS_DANCING, true);
    }

    public void StopDancingAnimation()
    {
        isDancing = false;
        animator.SetBool(HASH_IS_DANCING, false);
    }

    public bool CheckIfCountDownElapsed(float duration)
    {
        stateTimeElapsed += Time.deltaTime;
        return stateTimeElapsed >= duration;
    }

    /*
     * Draw color around this Enemy on Scene
     * based on its current State
     * ex: Dancing-> blue
     */ 
    private void OnDrawGizmos()
    {
        //on draw when it is playing
        if (!Application.isPlaying) return;

        //draw color based on state
        EnemyState currentState = stateController.currentState;
        if (currentState != null)
        {
            Gizmos.color = currentState.sceneGizmoColor;
            Gizmos.DrawWireSphere(transform.position, lookSphereRadius);
        }
    }
}

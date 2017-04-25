using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyStateController))]
public class SkeletonEnemy : PatrolGroundEnemy
{


    public Transform target;
    public float runningSpeed;
    public float turnSmoothing = 15f;
    public float lookSphereRadius = 15f;

    [HideInInspector]
    public Animator animator;

    //public bool isTurning;
    public bool isAttacking;

    public bool isDancing;
    private bool aiActive;
    private EnemyStateController stateController;


    //some const because I am too lazy typing them
    private const string IS_RUNNING = "isRunning";
    private const string IS_WALKING = "isWalking";
    private const string IS_TURNING_LEFT = "isTurningLeft";
    private const string IS_TURNING_RIGHT = "isTurningRight";

    private readonly int HASH_TRIGGER_SCREAMING = Animator.StringToHash("TriggerScreaming");
    private readonly int HASH_IDLE_TAG = Animator.StringToHash("Idle");
    private readonly int HASH_TRIGGER_DEATH = Animator.StringToHash("TriggerDeath");
    private readonly int HASH_TRIGGER_DANCE = Animator.StringToHash("TriggerDance");
    private readonly int HASH_DANCING_TAG = Animator.StringToHash("Dancing");
    public readonly int HASH_IS_DANCING = Animator.StringToHash("isDancing");

    private const string PLAYER_TAG = "Player";




    // Use this for initialization
    void Start()
    {
        //use this for PatrolAround
        base.__init__();

        //set center: skeleton won't fall back & forth
        rb.centerOfMass = new Vector3(0, -10, 0);

        //set up some variables
        animator = GetComponent<Animator>();

        //set up AI
        aiActive = true;
        stateController = GetComponent<EnemyStateController>();
        stateController.SetupStateController(this);
    }



    public override IEnumerator Attack()
    {

        return null;

    }


        void ReactTargetEscape()
    {
            StopTurning();
            StopAttacking();
            //Scream();
        }





        void StopAttacking()
    {
            StopCoroutine(Attack());
            Scream(); //because angry ><
            isAttacking = false;

        }

    /**
     * pretty cool animation made by Andrew
     * lol
     */
    private void Scream()

    {
        animator.SetTrigger(HASH_TRIGGER_SCREAMING);
    }

    public override void Movement()
    {

        if (!aiActive)
            return;

        this.stateController.UpdateStateController();
    }


    protected override void StartPatrolling()
    {
        base.StartPatrolling();
        animator.SetBool(IS_WALKING, true);
    }

    protected override void StopPatrolling()
    {
        base.StopPatrolling();
        animator.SetBool(IS_WALKING, false);
    }


    /**
     * Override base class,
     * add animation
     */
    protected override void StartTurning(Vector3 target)
    {
        //base code
        base.StartTurning(target);

        //determine which side to turn
        TargetSideDirection side = getTargetSideDirection(target);
        if (side == TargetSideDirection.LEFT)
        {
            animator.SetBool(IS_TURNING_RIGHT, false);
            animator.SetBool(IS_TURNING_LEFT, true);
        }
        else
        {
            animator.SetBool(IS_TURNING_LEFT, false);
            animator.SetBool(IS_TURNING_RIGHT, true);
        }

    }


    /**
    * Override base class,
    * add animation
    */
    protected override void StopTurning()
    {
        base.StopTurning();
        animator.SetBool(IS_TURNING_LEFT, false);
        animator.SetBool(IS_TURNING_RIGHT, false);
    }






    //}

    /**
     * check if this guy is doing something
     * later on, there will be more activies, so use this for short 
     * :)
     */
    private bool isDoingSomething()
    {
        return isPatrolling || isResting || isTurning || isAttacking;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == PLAYER_TAG && health > 0 && !isAttacking)
        {
            //assign target
            target = other.transform;

            animator.SetTrigger(HASH_TRIGGER_SCREAMING);

        }



    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == PLAYER_TAG && health > 0)
        {
            target = null;
            isAttacking = false;
        }
    }

    //public override IEnumerator WaitBeforeChangeDirection()
    //{

    //    //add animation
    //    anim.SetBool(IS_WALKING, false);

    //    //face new destination
    //    StartCoroutine(TurnToFaceTarget(curDestination.position));

    //    //use same
    //    return base.WaitBeforeChangeDirection();
    //}


    /**
     * if the target is on the LEFT or RIGHT
     * used for animation
     */
    public TargetSideDirection getTargetSideDirection(Vector3 target)
    {
        //Calculus 1, :-/
        //cos(a,b) = ( vector<a> . vector<b> ) / (...)
        //cos(a,b) < 0: left
        //cos(a,b) > 0: right

        Vector3 targetDirection = target - transform.position;
        float cos_angle = Vector3.Dot(targetDirection, transform.right);

        if (cos_angle > 0)
        {
            return TargetSideDirection.RIGHT;
        }

        return TargetSideDirection.LEFT;

    }

    public override void OnDeath()
    {

        animator.SetTrigger(HASH_TRIGGER_DEATH);
    }

    public bool isIdleState()
    {
        return animator.GetCurrentAnimatorStateInfo(0).tagHash == HASH_IDLE_TAG;
    }

    public void StartDancing()
    {
        isDancing = true;
        animator.SetTrigger(HASH_TRIGGER_DANCE);
        animator.SetBool(HASH_IS_DANCING, true);
    }


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

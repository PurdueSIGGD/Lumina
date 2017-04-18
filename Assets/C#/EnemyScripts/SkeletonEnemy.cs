using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonEnemy : PatrolGroundEnemy {

    //private var
    private Animator anim;
    public Transform target;
    public float runningSpeed;

    //public bool isTurning;
    public bool isAttacking;
    private bool first;

    //some const because I am too lazy typing them
    private const string IS_RUNNING = "isRunning";
    private const string IS_WALKING = "isWalking";
    private const string IS_TURNING_LEFT  = "isTurningLeft";
    private const string IS_TURNING_RIGHT = "isTurningRight";
    private const string TRIGGER_SCREAMING = "TriggerScreaming";

    private const string PLAYER_TAG = "Player";
    

    // Use this for initialization
    void Start()
    {
        //use this for PatrolAround
        base.__init__();

        anim = GetComponent<Animator>();

        //set center: skeleton won't fall back & forth
        rb.centerOfMass = new Vector3(0, -10, 0);

        isTurning = false;
        first = true;
    }



    public override IEnumerator Attack() 
    {
        isAttacking = true;      
        //if not facing target, try to do that
        while (isAttacking)
        {
            while (!isFacingTarget(target.position))
            {
                if (target == null)
                {
                    StopAttacking();
                }
                //rotate slowly
                StartTurning(target.position);
                RotateTowardsTarget(target.position);

                //rotate a bit every FixedUpdate()
                yield return new WaitForFixedUpdate();
            }

            StopTurning();
            //yield break;
        }
        yield break;

        //run towards target
        //isAttacking = true;
        //transform.LookAt(target);

        


       
        
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
        anim.SetTrigger(TRIGGER_SCREAMING);
    }

    public override void Movement()
    {
        //if skeleton do nothing, make it patrol around
        if (!isDoingSomething() && patrolPositions.Length > 0)
        {
            StartCoroutine(PatrolAround());
        }

        //if (target != null && !isFacingTarget(target.position) && !isTurning)
        //{
        //    StartCoroutine(TurnToFaceTarget(target.position));
        //}



    }
   

    protected override void StartPatrolling()
    {
        base.StartPatrolling();
        anim.SetBool(IS_WALKING, true);
    }

    protected override void StopPatrolling()
    {
        base.StopPatrolling();
        anim.SetBool(IS_WALKING, false);
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
            anim.SetBool(IS_TURNING_RIGHT, false);
            anim.SetBool(IS_TURNING_LEFT, true);
        }
        else
        {
            anim.SetBool(IS_TURNING_LEFT, false);
            anim.SetBool(IS_TURNING_RIGHT, true);
        }

    }


    /**
    * Override base class,
    * add animation
    */
    protected override void StopTurning()
    {
        base.StopTurning();
        anim.SetBool(IS_TURNING_LEFT, false);
        anim.SetBool(IS_TURNING_RIGHT, false);
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
        if (other.gameObject.tag == PLAYER_TAG && health > 0)
        {
            //assign target
            target = other.transform;

            //stop all other Coroutines like PatrolAround()
            StopPatrolling();
            StopTurning();

            isAttacking = true;
            anim.SetTrigger(TRIGGER_SCREAMING);
            //start attacking
            //Scream();
            //StartCoroutine(Attack());
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

    public override void OnDeath() {
        // IDK do whatever
    }

}

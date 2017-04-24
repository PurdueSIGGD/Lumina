using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonEnemy : PatrolGroundEnemy {

    //private var
    private Animator anim;
    public Transform target;

    //public bool isTurning;
    public bool isAttacking;
    public bool isScreaming;
    public float timeScreaming = 3f;
    public float runningSpeed;
    private bool first;

    //some const because I am too lazy typing them
    private const string IS_RUNNING = "isRunning";
    private const string IS_WALKING = "isWalking";
    private const string IS_TURNING_LEFT  = "isTurningLeft";
    private const string IS_TURNING_RIGHT = "isTurningRight";
    private const string TRIGGER_SCREAMING = "TriggerScreaming";

    private const string PLAYER_TAG = "Player";

    private const string STATE_SCREAMING = "Base Layer.rig|Detected";


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
        print("start attacking...");
        isAttacking = true;
        //set true
        Scream();
        isScreaming = true;

        //awkard way to wait until Skeleton done screaming
        //but it works
        yield return new WaitForSeconds(timeScreaming);
        isScreaming = false;



        //isAttacking = true;


        //if not facing target, try to do that
        while (!isFacingTarget(target.position))
        {
            //StartTurning(target.position);
            if (target == null)
            {
                ReactTargetEscape();
                yield break;
            }
            //rotate slowly
            isTurning = true;
            RotateTowardsTarget(target.position);

            //rotate a bit every FixedUpdate()
            yield return new WaitForFixedUpdate();
        }

        isTurning = false;

        //run towards target
        if (target == null)
        {
            isAttacking = false;
            StopCoroutine(Attack());
        }
        transform.LookAt(target);
        

        Vector3 distance = target.position - transform.position;

        if (Vector3.Magnitude(distance) < 3f)
        {
            print("Hit!!");
        }
        else
        {
            Vector3 forward =
                transform.position + transform.forward * Time.deltaTime * runningSpeed;
            rb.MovePosition(forward);
        }

       
        print("stop!!");
        isAttacking = false;
        StopCoroutine(Attack());
        
       






        //StopCoroutine(Attack());
        //Scream(); //because angry ><
        //isAttacking = false;


    }

    
    void ReactTargetEscape()
    {
        StopTurning();
        StopAttacking();
        //Scream();
    }

    /**
     * pretty cool animation made by Andrew
     * lol
     */ 
    void Scream()
    {
        anim.SetTrigger(TRIGGER_SCREAMING);
    }

    public override void Movement()
    {
        //if skeleton do nothing, make it patrol around
        //if (!isDoingSomething() && patrolPositions.Length > 0)
        //{
        //    StartCoroutine(PatrolAround());
        //}

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

    void StopAttacking()
    {
        isAttacking = false;
       
    }
   

    bool isAnimatorPlaying()
    {
        return anim.GetCurrentAnimatorStateInfo(0).length >
            anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    bool isAnimatorPlaying(string stateName)
    {
        return isAnimatorPlaying() && anim.GetCurrentAnimatorStateInfo(0).IsName(stateName);
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
            target = other.transform;

            //StopPatrolling();
            
            StartCoroutine(Attack());
        }



    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == PLAYER_TAG && health > 0)
        {
            target = null;
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonEnemy : PatrolGroundEnemy {

    //private var
    private Animator anim;
    public Transform target;

    public bool isTurning;
    private bool first;

    //some const because I am too lazy typing them
    private const string IS_RUNNING = "isRunning";
    private const string IS_WALKING = "isWalking";
    private const string IS_TURNING_LEFT  = "isTurningLeft";
    private const string IS_TURNING_RIGHT = "isTurningRight";

    public enum TargetSideDirection
    {
        LEFT,
        RIGHT
    }

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
        return null;
    }

    public override void Movement()
    {
        //if skeleton do nothing, make it patrol around
        if (!isDoingSomething() && patrolPositions.Length > 0)
        {
            StartCoroutine(PatrolAround());
        }

       
    }

    /**
     * Turn a small degree every FixUpdate to target direction
     * used for animation
     */ 
    private IEnumerator TurnToFaceTarget(Vector3 targetPostion)
    {
       
        
        //determine if Enemy is looking at target
        if (isFacingTarget(targetPostion))
        {
            //make it look it target anyway
            transform.LookAt(targetPostion);
            anim.SetBool(IS_TURNING_RIGHT, false);
            anim.SetBool(IS_TURNING_LEFT, false);

            //stop coroutine
            isTurning = false;
            yield break;
        }

        //start animation
        TargetSideDirection side = getTargetSideDirection(targetPostion);
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
            

        //if not turning to face target, turn to target
        Vector3 targetDir = targetPostion - transform.position;
        isTurning = true;
        float cos_angle = Vector3.Dot(transform.right, targetDir);
        float step = turningSpeed  * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
        transform.rotation = Quaternion.LookRotation(newDir);

        yield return new WaitForFixedUpdate();

        StartCoroutine(TurnToFaceTarget(targetPostion));
        
        
    }

    /**
     * check if this guy is doing something
     * later on, there will be more activies, so use this for short 
     * :)
     */ 
    private bool isDoingSomething()
    {
        return isPatrolling || isResting || isTurning;
    }


    private void OnTriggerEnter(Collider other)
    {

    }


    /**
     * check if this.Enemy is facing target. such as Player, or next destination
     * just for animation
     */ 
    private bool isFacingTarget(Vector3 target)
    {
        Vector3 targetDir = (target - transform.position).normalized;
        float diff = Vector3.Dot(transform.forward, targetDir);

        //if diff ~ 1.0, then it mostly look at target
        //stop Coroutine
        if (diff >= 0.99)
        {          
            return true;
        }
        return false;
    }

    /**
     * override base class, to add animation
     */ 
    public override void StartPatrolling()
    {
        //calculate next destination
        base.StartPatrolling();

        //face new destination
        StartCoroutine(TurnToFaceTarget(curDestination.position));

        //add is walking
        anim.SetBool(IS_WALKING, true);
        
    }

  
    public override IEnumerator WaitBeforeChangeDirection()
    {
       
        //add animation
        anim.SetBool(IS_WALKING, false);

        //use same
        return base.WaitBeforeChangeDirection();
    }


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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*********************************************************************
 * 
 * This class is for Enemies who usually moves around the fixed route
 * Ex: a golem walking around, a skeleton patrolling
 * How to use:
 *  void Start() 
 *      base.__init__()
 *  void Movement()
 *      StartCoroutine(PatrolAround())
 *      
 *      
 **********************************************************************/
[RequireComponent(typeof(Rigidbody))]
public abstract class PatrolGroundEnemy : BaseEnemy {

    public Transform[] patrolPositions; //positions that golem will move around, don't have, golem stand still.
    protected Transform curDestination;   //where golem is heading

    public bool isPatrolling; //if golem is patrolling, don't change direction
    public bool isResting; //when 
    public bool isTurning;
    protected int curPatrolIndex; //index to keep track of where that golem is heading

    public float timeBeforeChangeDirection; //time that golem will wait at the destination before change direction.

    public float turningSpeed;
    protected Rigidbody rb;


    public enum TargetSideDirection
    {
        LEFT,
        RIGHT
    }



    protected void __init__()
    {
        rb = GetComponent<Rigidbody>();
        curPatrolIndex = -1;
       
    }

    /*
    * if patrolPositions[] is assigned
    * 
    * PatrolAround() golem will patrol around specific points
    * waiting for sth to attack
    */
    protected IEnumerator PatrolAround()
    {
        //simple check against null
        if (patrolPositions == null || patrolPositions.Length == 0)
        {
            yield break;
        }
       
        //update new destination      
        CalculateNextDestination();
        
        //turning slowly to face target
        if (!isFacingTarget(curDestination.position))
            StartTurning(curDestination.position);

        while (!isFacingTarget(curDestination.position))
        {
            //rotate slowly
            isTurning = true; 
            RotateTowardsTarget(curDestination.position);

            //rotate a bit every FixedUpdate()
            yield return new WaitForFixedUpdate();
        }

        //turn off isTurning
        StopTurning();

        //start patrol
        StartPatrolling();
        while (isPatrolling)
        {
            //move to new position
            transform.LookAt(curDestination);
            Vector3 forward =
                transform.position + transform.forward * Time.deltaTime * movementSpeed;
            rb.MovePosition(forward);

            //check if near destination
            if (isNearDestination(curDestination.position))
            {
                StartCoroutine(WaitBeforeChangeDirection());
                yield break;
            }

            //move every FixedUpdate()
            yield return new WaitForFixedUpdate();
        }

        StopPatrolling();
        yield break;
    }

    /**
     * rotate a bit toward target
     * helps animation
     */ 
    protected virtual void RotateTowardsTarget(Vector3 target)
    {
        //get final direction
        Vector3 targetDir = target - transform.position;
        Debug.DrawRay(transform.position, targetDir); //debug

        //get next direction
        float step = turningSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);

        //assign new rotation
        transform.rotation = Quaternion.LookRotation(newDir);
    }


    /**
     * Simple first
     * children can animation
     */ 
    protected virtual void StopTurning()
    {
        isTurning = false;
    }

    /**
     * Need target because children will use that to determine which
     * side to run. ex: LEFT, RIGHT
     */ 
    protected virtual void StartTurning(Vector3 target)
    {
        isTurning = true;
    }


    protected virtual void StartPatrolling()
    {
        isPatrolling = true;
    }

    protected virtual void StopPatrolling()
    {
        isPatrolling = false;
    }

    

    /**
     * Advanced FindPath(), look at obstacle, add new position
     * to avoid obstacles
     */
    protected void FindPath()
    {

    }

    /*
     * isNearDestination(): while golem is patrolling
     * return true if golem is near its current destination
     */
    protected bool isNearDestination(Vector3 destination)
    {
        //calculate the distance
        Vector3 distanceVect = destination - transform.position;
        float distance = Vector3.Magnitude(distanceVect);

        //simple check.
        return distance < 5f;
    }


    /*
    * simple code to get next destination
    */
    public virtual void CalculateNextDestination()
    {
        curPatrolIndex = (curPatrolIndex + 1) % patrolPositions.Length;
        curDestination = patrolPositions[curPatrolIndex];
    }

    /*
     * Resting before change direction while 
     * this enemy is Patrolling around
     * Make it virtual: to add animation
     */ 
    public virtual IEnumerator WaitBeforeChangeDirection()
    {
        //arghhh resting...
        StopPatrolling();
        isResting = true;
        
        //rest a bit
        yield return new WaitForSeconds(timeBeforeChangeDirection);

        //start patrol again
        isResting = false;
              
    }
   

    /**
    * check if this.Enemy is facing target. such as Player, or next destination
    * just for animation
    */
    protected bool isFacingTarget(Vector3 target)
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


}

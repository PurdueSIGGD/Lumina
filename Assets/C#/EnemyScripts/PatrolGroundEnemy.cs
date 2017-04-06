using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class is for Enemies who usually moves around the fixed route
 * Ex: a golem walking around, a skeleton patrolling
 * How to use:
 *  void Start() 
 *      base.__init__()
 *  void Movement()
 *      StartCoroutine(PatrolAround())
 */ 
 [RequireComponent(typeof(Rigidbody))]
public abstract class PatrolGroundEnemy : BaseEnemy {

    public Transform[] patrolPositions; //positions that golem will move around, don't have, golem stand still.
    protected Transform curDestination;   //where golem is heading

    public bool isPatrolling; //if golem is patrolling, don't change direction
    public bool isResting; //when 
    protected int curPatrolIndex; //index to keep track of where that golem is heading

    public float timeBeforeChangeDirection; //time that golem will wait at the destination before change direction.

    protected Rigidbody rb;
    //protected Animator anim;

    //some const because I am too lazy typing them
    protected const string IS_RUNNING = "isRunning";


    protected void __init__()
    {
        rb = GetComponent<Rigidbody>();
        curPatrolIndex = -1;
        //anim = GetComponent<Animator>();
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

        //if is (not patrolling) or isResting
        //update new destination
        if (!isPatrolling)
        {
            StartPatrolling();
        }

        //start patrol
        while (isPatrolling)
        {
            //update FixedUpdate()
            yield return new WaitForFixedUpdate();

            //move to new position
            transform.LookAt(curDestination);
            Vector3 forward =
                transform.position + transform.forward * Time.deltaTime * movementSpeed;
            rb.MovePosition(forward);

            //check if near destination
            if (isNearDestination(curDestination.position))
            {
                StartCoroutine(WaitBeforeChangeDirection());
            }

            

        }


    }

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
    * simple code to start patrol 
    * add animation later
    */
    public virtual void StartPatrolling()
    {
        isPatrolling = true;
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
        isResting = true;
        isPatrolling = false;

        //rest a bit
        yield return new WaitForSeconds(timeBeforeChangeDirection);

        //start patrol again
        isResting = false;
              
    }
   

    public virtual void StopPatrolling()
    {
        isPatrolling = false;
    }


}

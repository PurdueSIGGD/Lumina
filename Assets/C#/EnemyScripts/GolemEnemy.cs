using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//use Unity Random
using Random = UnityEngine.Random;

[RequireComponent (typeof(Rigidbody))]
public class GolemEnemy : BaseEnemy {

    public Transform target;    //target to attack, ex: player, animal
    public bool isAttacking;    //if golem is attacking
    public float timeBetweenAttacks;    //time between 2 attacks

    public float meleeRange;    //range that golem will smash target
    public float throwRange;    //range that golem will throw stuff at target, does not use right now :( 

    public GameObject[] rocks; //rocks that golem will use to throw, if don't have rocks, it will stare at you angrily  
    private Rigidbody rb;

    public Transform[] patrolPositions; //positions that golem will move around, don't have, golem stand still.
    private Transform curDestination;   //where golem is heading
    public bool isPatrolling; //if golem is patrolling, don't change direction
    private int curPatrolIndex; //index to keep track of where that golem is heading
    public float timeBeforeChangeDirection; //time that golem will wait at the destination before change direction.

 
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        curPatrolIndex = -1;

        //check if golem has rock
        if (rocks == null || rocks.Length == 0)
        {
            Debug.Log("Please put in some rocks for golem :|");
        }
    }


    public override IEnumerator Attack()
    {
        isAttacking = true;

        //prepare
        yield return new WaitForSeconds(timeBetweenAttacks);

        //if target escape while golem is prepare, prevent null exception 
        try
        {
            float distanceFromTarget =
            Vector3.Magnitude(target.transform.position - transform.position);
            //decide which attack to use
            if (distanceFromTarget < meleeRange)
                Smash();
            else ThrowRock();

            //if isAttacking, attack again
            if (isAttacking) StartCoroutine(Attack());
            else StopCoroutine(Attack());

        }

        catch (NullReferenceException)
        {
            StopCoroutine(Attack());
        }
        
        
        

    }

    /*
     * Main method that will be updated every frame
     */
    public override void Movement()
    {
        //golem stay in 1 place because it is lazy and love to sleep
        if (target != null)
        {
            transform.LookAt(target); 

        }
        //no enemy, golem patrol around
        else if (!isPatrolling && patrolPositions.Length > 0)
        {
            StartCoroutine( PatrolAround() );
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        //update this later, so that golem can attack other as well
        if (other.tag != "Player") return;

        //stop patrol and start attack
        StopPatrol();
        target = other.transform;
        isAttacking = true;
        StartCoroutine(Attack());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == target)
        {
            target = null;
            isAttacking = false;
        }
    }

    /*
     * Smash() 
     * One of golem melee attack
     */ 
    private void Smash()
    {
        //incase target fast-escape
        if (target == null) return;

        //check if golem has rock
        if (rocks == null || rocks.Length == 0)
        {
            Debug.Log("Please put in some rocks for golem :|");
            return;
        }

        //simple smash using rock
        Vector3 pos = transform.position + transform.forward + transform.up;
        GameObject rock = Instantiate(rocks[Random.Range(0, rocks.Length)], 
             pos,
            Quaternion.identity);

        //add Rigid body to rock     
        Rigidbody rockRb = rock.GetComponent<Rigidbody>();
        if (rockRb == null)
        {
            rock.AddComponent<Rigidbody>();
            rockRb = rock.GetComponent<Rigidbody>();
        }

        //add force
        rockRb.AddForce(Vector3.down * 10, ForceMode.Impulse);

        //for keep things clean, destroy after 2 seconds
        Destroy(rock, 2f);
    }

    /*
     * ThrowRock()
     * golem calculate force and throw in fixed direction
     * 
     * improved: calculate angle better. 
     * For example, target in on higher place, like mountain
     * and golem is on ground
     * 
     * In this function: golem only throw in 1 fixed angle.
     * 
     */ 
    private void ThrowRock()
    {
        //in case target escape when golem ready to throw rock
        if (target == null) return;

        //check if golem has rock
        if (rocks == null || rocks.Length == 0)
        {
            Debug.Log("Please put in some rocks for golem :|");
            return;
        }

        //get the rock
        GameObject rock = Instantiate( 
            rocks[Random.Range(0, rocks.Length)], 
            transform.localPosition + transform.up,
            Quaternion.identity);

       
        //add Rigid body to rock in curve direction        
        Rigidbody rockRb = rock.GetComponent<Rigidbody>();
        if (rockRb == null)
        {
            rock.AddComponent<Rigidbody>();
            rockRb =  rock.GetComponent<Rigidbody>();
        }

        //calculate direction of rock
        Vector3 direction = transform.up + transform.forward;

        //calcuate throw force
        float constant = 0.7f; //because distance ~ constant * force

        float distanceFromTarget = 
            Vector3.Magnitude(target.transform.position - transform.position);

        float throwForce = constant * distanceFromTarget;
        
        //add force to rock
        rockRb.AddForce(direction * throwForce , ForceMode.Impulse);

        //for keep things clean, destroy after 2 seconds
        Destroy(rock, 2f);
    }

    /*
     * if patrolPositions[] is assigned
     * 
     * PatrolAround() golem will patrol around specific points
     * waiting for sth to attack
     */
    private IEnumerator PatrolAround()
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
            isPatrolling = true;
            curPatrolIndex = (curPatrolIndex + 1) % patrolPositions.Length;
            curDestination = patrolPositions[curPatrolIndex];           
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
                //rest a bit
                yield return new WaitForSeconds(timeBeforeChangeDirection);

                //set isPatrolling to false
                StopPatrol();                             
            }
            
        }


    }

    /*
     * isNearDestination(): while golem is patrolling
     * return true if golem is near its current destination
     */ 
    private bool isNearDestination(Vector3 destination)
    {
        //calculate the distance
        Vector3 distanceVect = destination - transform.position;
        float distance = Vector3.Magnitude(distanceVect);

        //simple check.
        return distance < 3f;       
    }

    /*
     * simple check
     * may add animation later.
     */ 
    private void StopPatrol()
    {
        isPatrolling = false;
    }

   
    

}

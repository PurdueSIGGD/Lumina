
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    private bool isWalking; //if golem is walking, don't change direction
    private int curDestination; //destination that golem is heading
    public float timeBeforeChangeDirection; //time that golem will wait at the destination before change direction.

 
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        curDestination = -1;
        PatrolAround();
    }


    public override IEnumerator Attack()
    {
        isAttacking = true;

        //prepare
        yield return new WaitForSeconds(timeBetweenAttacks);

        //if target escape while golem is prepare, prevent null exception
        if (target == null) StopCoroutine(Attack());

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

    /*
     * Main method that will be updated every frame
     */
    public override void Movement()
    {
       //golem stay in 1 place because it is lazy and love to sleep
       if (target != null )
        {
            //transform.LookAt(target); 
            
        }
        
        else
        {
            //PatrolAround();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
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
     * PatrolAround() golem will patrol around specific points
     * waiting for sth to attack
     */
    private void PatrolAround()
    {
        //if there is no assigned positions, golem stand still 
        if (patrolPositions == null || patrolPositions.Length == 0)
        {
            return;
        }
        StartCoroutine( WaitForSomeTime());
        curDestination = (curDestination + 1) % patrolPositions.Length;
        transform.position = patrolPositions[curDestination].position;
    }

    private IEnumerator WaitForSomeTime()
    {
        yield return new WaitForSeconds(timeBeforeChangeDirection);
        StopCoroutine(WaitForSomeTime());
    }



    

}

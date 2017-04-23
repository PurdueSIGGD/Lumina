using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//use Unity Random
using Random = UnityEngine.Random;

public class GolemEnemy : PatrolGroundEnemy {

    public Transform target;    //target to attack, ex: player, animal
    public bool isAttacking;    //if golem is attacking
    public float timeBetweenAttacks;    //time between 2 attacks
    public float smashDamage = 25;
    public float rockDamage = 25;
    public float meleeRange;    //range that golem will smash target
    public float throwRange;    //range that golem will throw stuff at target, does not use right now :( 

    public GameObject[] rocks; //rocks that golem will use to throw, if don't have rocks, it will stare at you angrily  


    private void Start()
    {
        //must have this for PatrolGroundEnemy
        base.__init__();

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
        if (!other.isTrigger || other.tag != "Player") return;

        //stop patrol and start attack
        if (health > 0) {
            StopPatrolling();
            target = other.transform;
            isAttacking = true;
            StartCoroutine(Attack());
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.isTrigger && other.transform == target)
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
        Hittable h;
        if (health > 0 && (h = target.GetComponent<Hittable>())) {
            h.Hit(smashDamage);
        }
    /*
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
        Destroy(rock, 2f);*/
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
        Projectile proj = rock.GetComponent<Projectile>();
        proj.creator = transform;
        proj.damage = this.rockDamage;
       
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



    public override void OnDeath() {
        // IDK do whatever
        StopCoroutine(PatrolAround());
        StopCoroutine(Attack());
    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof(Rigidbody))]
public class GolemEnemy : BaseEnemy {

    public Transform target;    //target to attack, ex: player, animal
    public bool isAttacking;    //if golem is attacking
    public float timeBetweenAttacks;    //time between 2 attacks
    public float meleeRange;    //range that golem will smash target
    public float throwRange;    //range that golem will throw stuff at target 
    public GameObject[] rocks;

    private Rigidbody rb;
 
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
        
        //attack
        if (distanceFromTarget < 2f)       
            Smash();            
        else ThrowRock();

        //if isAttacking, attack again
        if (isAttacking) StartCoroutine(Attack());
        else StopCoroutine(Attack());

    }

    public override void Movement()
    {
       //golem stay in 1 place because it is lazy and love to sleep
       if (target != null )
        {
            transform.LookAt(target);           
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

    

}

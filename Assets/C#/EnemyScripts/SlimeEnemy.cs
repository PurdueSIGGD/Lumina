﻿using UnityEngine;
using System.Collections;

public class SlimeEnemy : BaseEnemy {

	public Transform target; 				//The player, for the slime to face and move toward
	public int thrust;						//The amount of force used to propel the slime forward
	public bool isAttacking;				//If the enemy is currently attacking, return true
    public float timeBetweenAttacks;        //How fast the slime jumps at you
    public float attackRange;
    public float damage = 5;
	private Vector3 startPos;				//The slimes starting position
	private Rigidbody rb;					//The slimes rigidbody which allows us to propel it
	private float changeDirectionCount = 0;	//The time counter for the slime to change direction while the player isn't around
    float forceMultiplier = 1;
    float lastHit;
    public float hitCooldown = 1;
    public Animator myAnim;

    public RandomAudioSource death;
    //Variable initialization 
    void Start(){
        lastHit = 0;
		startPos = transform.position;
		rb = GetComponent<Rigidbody> ();
		rb.constraints = RigidbodyConstraints.FreezeRotation;
	}
		
	/* The attack method is an IEnumarator to allow the slime to wait before executing the attack
	 * The slime will then propel itself towards the player
	 * It will wait a couple of seconds before moving again
	 */
	public override IEnumerator Attack(){
        isAttacking = true;
        yield return new WaitForSeconds(timeBetweenAttacks); //Wait a single second before attack
        if (health >= 0)
        {
            myAnim.SetTrigger("StartAttack");
            rb.AddForce(transform.forward * thrust * 1.5f + Vector3.up * 2 *  thrust);
            yield return new WaitForSeconds(.9f);
            // Jump back
            rb.AddForce(transform.forward * -1 * thrust + Vector3.up * 2 * thrust / 2);
            // If still attacking, attack again
            if (isAttacking) StartCoroutine(Attack());
            else StopCoroutine(Attack());
        } else {
            isAttacking = false;
            StopCoroutine(Attack());
        }

      
    }

	/* This is a simple movement method, 
	 * it looks in the players direction and moves that way constantly
	 */
	public override void Movement(){
		if (target != null) {
			if (isAttacking) {

                if (!(Vector3.Distance(target.position, transform.position) < attackRange))
                {
                    forceMultiplier = .1f;
                } else
                {
                    forceMultiplier = 1;
                }

                // They will start moving slower if they want to attack you! Good way of knowing when they spotted you
                transform.position = transform.position + (transform.forward * Time.deltaTime * forceMultiplier * movementSpeed);
                if (/* You want to stop attacking, say you are mid air or low health*/false) {
                    isAttacking = false;
                }
            } else {
                if (/* You want to start attacking, as in you are on the floor or health regenerated*/false) {
                    isAttacking = true;
                }
            } 

            
            transform.LookAt(target);
        }

        else if (target == null) {

            StopAllCoroutines();
            myAnim.SetBool("Attacking", false);
            changeDirectionCount += Time.deltaTime;
			transform.position = transform.position + (transform.forward * Time.deltaTime * movementSpeed);
			if (changeDirectionCount > 4f) {
				transform.rotation = Quaternion.Euler(0, Random.Range (-360, 360), 0);
				changeDirectionCount = 0;
			}
		}

	}

	/* This access the slime's Sphere Collider where there is a trigger 
	 * The trigger is the range of the slime's attack and will call the Attack coroutine 
	 * when triggered
	 */
	public void OnTriggerEnter(Collider col){
        if (!col.isTrigger && col.tag == "Player" && !isAttacking)
        {
            target = col.transform;
            isAttacking = true;
            myAnim.SetTrigger("StartAttacking");
            myAnim.SetBool("Attacking", true);
            StartCoroutine(Attack());
            //Debug.Log("PLAYER FOUND");
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (!col.isTrigger && col.transform == target)
        {

            StopAllCoroutines();
            myAnim.SetBool("Attacking", false);
            target = null;
            isAttacking = false;
        }
    }
    public void OnCollisionEnter(Collision col) {
        Hittable h;
        if (Time.time - lastHit > hitCooldown && col.transform == target && health > 0 && (h = target.GetComponent<Hittable>())) {
            lastHit = Time.time;
            h.Hit(damage);
        }
    }
    public override void OnDeath() {
        // IDK do whatever
        StopAllCoroutines();
        rb.constraints = RigidbodyConstraints.None;
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        myAnim.SetBool("Attacking", false);
        myAnim.SetBool("Death", true);
    }
    public override void OnDamage(float damage, DamageType type) {

    }
}

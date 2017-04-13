using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarklingEnemy : BaseEnemy {

    public Transform target;                //The player, for the darkling to face and move toward
    public int thrust;                      //The amount of force used to propel the slime forward
    public bool isAttacking;				//If the enemy is currently attacking, return true
    public float timeBetweenAttacks;        //How fast the slime jumps at you
    public float attackRange;

    public int teleDistance;                //The distance the darkling will teleport 
    public GameObject teleParticles;        //Particle system that spawns when the Darkling teleports
    Coroutine telePattern;
    public float damage = 10;

    private Vector3 startPos;               //The slimes starting position
    private Rigidbody rb;                   //The slimes rigidbody which allows us to propel it
    private float changeDirectionCount = 0;	//The time counter for the slime to change direction while the player isn't around
    float forceMultiplier = 1;


    //Variable initialization 
    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        telePattern = StartCoroutine(MovementPattern());
    }

    /* The attack method is an IEnumarator to allow the slime to wait before executing the attack
	 * The slime will then propel itself towards the player
	 * It will wait a couple of seconds before moving again
	 */
    public override IEnumerator Attack()
    {
        isAttacking = true;

        yield return new WaitForSeconds(timeBetweenAttacks); //Wait a single second before attack
        Hittable h;
        if (target == null) {
            //They moved out of range
            StopCoroutine(Attack());
        } else
        if (Vector3.Distance(target.position, transform.position) < attackRange && health > 0 && (h = target.GetComponent<Hittable>())) {
            h.Hit(damage);
        }
        
        //rb.AddForce(transform.forward * thrust * forceMultiplier + Vector3.up * thrust / 2 * forceMultiplier);

        // If still attacking, attack again
        if (isAttacking) StartCoroutine(Attack());
        else StopCoroutine(Attack());
    }

    /* This is a simple movement method, 
	 * it looks in the players direction and moves that way constantly
	 */
    public override void Movement()
    {
        if (target != null)
        {
            if (isAttacking)
            {

                if (!(Vector3.Distance(target.position, transform.position) < attackRange))
                {
                    forceMultiplier = 0;
                }
                else
                {
                    forceMultiplier = 1;
                }

                // They will start moving slower if they want to attack you! Good way of knowing when they spotted you
                transform.position = transform.position + (transform.forward * Time.deltaTime * movementSpeed);

                if (/* You want to stop attacking, say you are mid air or low health*/false)
                {
                    isAttacking = false;
                }
            }
            else
            {
                if (/* You want to start attacking, as in you are on the floor or health regenerated*/false)
                {
                    isAttacking = true;
                }
            }


            transform.LookAt(target);
        }

        else if (target == null)
        {
            
            transform.position = transform.position + (transform.forward * Time.deltaTime * movementSpeed);
            changeDirectionCount += Time.deltaTime;
            transform.position = transform.position + (transform.forward * Time.deltaTime * movementSpeed);
            if (changeDirectionCount > 4f)
            {
                transform.rotation = Quaternion.Euler(0, Random.Range(-360, 360), 0);
                changeDirectionCount = 0;
            }

        }

        

    }

    /* This access the slime's Sphere Collider where there is a trigger 
	 * The trigger is the range of the slime's attack and will call the Attack coroutine 
	 * when triggered
	 */
    public void OnTriggerEnter(Collider col)
    {
        if (!col.isTrigger && col.tag == "Player" && !isAttacking)
        {
            target = col.transform;
            isAttacking = true;
            StartCoroutine(Attack());
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (!col.isTrigger && col.transform == target)
        {
            target = null;
            isAttacking = false;
        }
    }

    public void Teleport()
    {
        // TODO: try to make sure they don't teleport under you, because it will launch you. 
        transform.position = transform.position + transform.forward * teleDistance;
        GameObject g = Instantiate(teleParticles, transform.position, Quaternion.identity);
        Destroy(g, 3);
    }

    public IEnumerator MovementPattern()
    {
        //This coroutine randomizes the bats upward thrust for flying giving it a more eratic flight pattern for realism
        //The Movement () method just makes the bat fly forward now
        //Called from start() so as to not start the coroutine every frame
        while (health > 0)
        {
            yield return new WaitForSeconds(4f);
            if (health > 0) {
                Teleport();
            }
            //yield return new WaitForSeconds(4f);
        }
    }

    public override void OnDeath() {
        // IDK do whatever
        StopCoroutine(MovementPattern());

        rb.constraints = RigidbodyConstraints.None; //So they can fall and die or something
    }
}

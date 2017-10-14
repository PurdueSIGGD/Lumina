using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/******************************************************************************
 * 
 * DarklingAirEnemy
 * 
 * a ghost-type enemy that teleport, and area is near ground
 * implements Pluggable AI
 * 
 * Behaves like darkling Enemy, but I want to keep his code
 * 
 ******************************************************************************/

[RequireComponent(typeof(EnemyStateController))]
public class DarklingAirEnemy : AirEnemy {

    public enum DarklingType
    {
        Idle, //darkling will stay in 1 place
        Wander //darkling will wander around
    }
   
    public Transform target;
    public Transform eyes;
    public GameObject darklingModel; //use for making it disappear

   
    public float lookSphereRadius = 15f;    //use for draw Gizmo, and detect target
    public float timeBetweenAttacks = 2f;
    public float timeBetweenTele = 4f;   //for now, it will teleport every 4s, regardless of which state it is in
    public float timeBeforeAppear = 0.5f;
    public float distanceRunAfterAttack = 20f;
    public float distanceMeleeAttack = 4f;
    public float distanceMeleeZone = 15f; //if target is in this zone, then switch to melee attack state
    public float distanceFlyMin = 20f;
    
    public float attackDamage = 10;
    public Hittable.DamageType attackType = DamageType.Denim;
    
    
    public float teleportDistance = 6f;  
    public GameObject teleParticles;     //Particle system that spawns when the Darkling teleports

    [HideInInspector] public bool isAllowedToAttack;
    [HideInInspector] public SaveTransform startTransform;
    [HideInInspector] public bool hasDoneAttacking;
    [HideInInspector] public bool isAllowedToTeleport;
    [HideInInspector] public float flyTimeElapsed; //measure fly duration
    [HideInInspector] public float teleTimeElapsed; //measure teleport time duration
    [HideInInspector] public float attackTimeElapsed; //measure time between attacks
    [HideInInspector] public Animator animator;
    [HideInInspector] public Vector3 destination;
    [HideInInspector] public bool destinationPending;   //true if Darkling is finding a destination
    [HideInInspector] public bool nextDestinationDirectionPick; //true if Darkling has chosen a direction for new destination

    public bool isAttacking;
    public bool isChasing;
    public bool isTeleporting;
    public bool isRunningAway;
    private bool aiActive;

    
    private EnemyStateController stateController;
    public EnemyState darklingIdleStartState;
    public EnemyState darklingWanderStartState;
    public DarklingType darklingType = DarklingType.Idle; //default enemy type



    public readonly int HASH_TELE_START = Animator.StringToHash("TriggerTeleportStart");
    public readonly int HASH_TELE_END  = Animator.StringToHash("TriggerTeleportEnd");  
    public readonly int HASH_MELEE_ATTACK = Animator.StringToHash("TriggerMeleeAttack");
    public const string PLAYER_TAG = "Player";
    public const string TAG_TELE_START = "Teleport_Start";
    public const string TAG_TELE_END = "Teleport_End";

    public RandomAudioSource deathSound;

    private void Start()
    {
        //call AirEnemy setup
        base.Setup();

        //set variable
        animator = GetComponent<Animator>();

        //set up AI
        SetupAI();

    }

    private void SetupAI()
    {
        //get enemy state controller
        stateController = GetComponent<EnemyStateController>();
        
        //SetFlyHeightFromGround(flyHeight);
        aiActive = true;
        stateController.SetupStateController(this);
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        //so darkling know where to return
        this.startTransform = new SaveTransform(this.transform);

        //assign type for darkling
        switch(darklingType)
        {
            case DarklingType.Idle:
                stateController.currentState = darklingIdleStartState;
                break;

            case DarklingType.Wander:
                stateController.currentState = darklingWanderStartState;
                break;
            
            //if not assign, leave it there
            default:
                break;

        }
    }

    public override IEnumerator Attack()
    {
        return null;
    }

    public override void Movement()
    {
        if (!aiActive)
            return;

        stateController.UpdateStateController();
    }

   

    public void StartTeleAnimationStart()
    {
        animator.SetTrigger(HASH_TELE_START);
    }

    public void StartTeleAnimationStop()
    {
        animator.SetTrigger(HASH_TELE_END);
    }

    public void StartMeleeAttackAnimation()
    {
        animator.SetTrigger(HASH_MELEE_ATTACK);
    }

    public bool CheckIfFlyCountDownElapsed(float duration)
    {
        flyTimeElapsed += Time.deltaTime;
        return flyTimeElapsed >= duration;
    }

    public bool CheckIfTeleCountDownElapsed(float duration)
    {
        teleTimeElapsed += Time.deltaTime;
        return teleTimeElapsed >= duration;
    }

    public bool CheckIfAttackCountDownElapsed(float duration)
    {
        attackTimeElapsed += Time.deltaTime;
        return attackTimeElapsed >= duration;
    }

    /*
     * Make this Ghost Enemy ignore all collision 
     */
    private void OnCollisionEnter(Collision collision)
    {
        Collider collider = GetComponent<Collider>();
        Collider otherCollider = collision.gameObject.GetComponent<Collider>();

      
        Physics.IgnoreCollision(collider, otherCollider);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (target != null || health <= 0)
            return;

        if (other.gameObject.tag == PLAYER_TAG)
            target = other.transform;
    }


    private void OnTriggerExit(Collider other)
    {
        if (target == null || other.gameObject.tag != PLAYER_TAG)
            return;

        //a hack way to make sure that target has to be far enough
        //before let target go
        if (isFarEnoughFromTarget(target.position, lookSphereRadius))
        {
            target = null;
        }
    }

    public override void OnDeath()
    {
        this.GetComponentInChildren<ParticleSystem>().Stop();

        Collider[] colliders = this.GetComponents<Collider>();
        foreach (Collider c in colliders) {
            c.isTrigger = true; // disable collision
        }
        deathSound.PlayOnce();
        animator.SetBool("Death", true);
        
    }
    public override void OnDamage(float damage, DamageType type) {
    
        //trigger the animation, use another layer
        
        //if damage type is Ice, slow down and cannot run away after attack.
       if (type == DamageType.Ice)
        {
            print("Darkling Air: Ice ice!");
        }
        
    }

    /*
     * Draw color around this Enemy on Scene
     * based on its current State
     * ex: Dancing-> blue
     */
    private void OnDrawGizmos()
    {
        //on draw when it is playing
        if (!Application.isPlaying) return;

        //draw color based on state
        EnemyState currentState = stateController.currentState;
        if (currentState != null)
        {
            Gizmos.color = currentState.sceneGizmoColor;
            Gizmos.DrawWireSphere(transform.position, lookSphereRadius);
        }
    }

}

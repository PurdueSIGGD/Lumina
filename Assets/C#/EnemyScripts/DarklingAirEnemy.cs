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


    public Transform target;
    public Transform eyes;
    public GameObject darklingModel; //use for making it disappear
    public float lookSphereRadius = 15f;    //use for draw Gizmo, and detect target
    public float timeBetweenAttacks = 2f;
    public float timeBetweenTeleMin = 4f;   //for now, it will teleport every 4s, regardless of which state it is in
    public float timeBetweenTeleMax = 10f;
    public float timeBeforeAppear = 0.5f;
    public float distanceFlyMin = 30f;
    public bool isAllowedToAttack;
    
    public float teleportDistance = 6f;  
    public GameObject teleParticles;     //Particle system that spawns when the Darkling teleports

    [HideInInspector] public bool isAllowedToTeleport;
    [HideInInspector] public float flyTimeElapsed; //for state to measure its time.
    [HideInInspector] public float teleTimeElapsed; //for state to measure its time.
    [HideInInspector] public Animator animator;
    [HideInInspector] public Vector3 destination;
    [HideInInspector] public bool destinationPending;   //true if Darkling is finding a destination
    [HideInInspector] public bool nextDestinationDirectionPick; //true if Darkling has chosen a direction for new destination

    public bool isWalking;
    public bool isTeleporting;
    private bool aiActive;
    private EnemyStateController stateController;


    public readonly int HASH_TELE_START = Animator.StringToHash("TriggerTeleportStart");
    public readonly int HASH_TELE_STOP  = Animator.StringToHash("TriggerTeleportStop");
    public const string PLAYER_TAG = "Player";
   
    private void Start()
    {
        //call AirEnemy setup
        base.Setup();
        //set variable
        animator = GetComponent<Animator>();
        stateController = GetComponent<EnemyStateController>();

        //set up AI
        SetFlyHeightFromGround(flyHeight);
        aiActive = true;
        stateController.SetupStateController(this);
        rb.constraints = RigidbodyConstraints.FreezeRotation;
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
        animator.SetTrigger(HASH_TELE_STOP);
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

    /*
     * Make this Ghost Enemy ignore all collision 
     */
    private void OnCollisionEnter(Collision collision)
    {
        Collider collider = GetComponent<Collider>();
        Collider otherCollider = collision.gameObject.GetComponent<Collider>();
        Physics.IgnoreCollision(collider, otherCollider);
    }

    public override void OnDeath()
    {
        
    }
    public override void OnDamage(float damage) {
        throw new NotImplementedException();
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

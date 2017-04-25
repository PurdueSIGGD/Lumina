using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/******************************************************************************
 * 
 * DarklingGroundEnemy
 * 
 * a ghost-type enemy that teleport, and area is near ground
 * implements Pluggable AI
 * 
 * Behaves like darkling Enemy, but I want to keep his code
 * 
 ******************************************************************************/

[RequireComponent(typeof(EnemyStateController))]
public class DarklingGroundEnemy : BaseEnemy {


    public Transform target;
    public float lookSphereRadius = 15f;    //use for draw Gizmo, and detect target
    public float timeBetweenAttacks = 2f;
    public bool isAllowedToAttack;
    public float teleportDistance = 6f;
    public float flyHeight = 10f;
    public GameObject teleParticles;     //Particle system that spawns when the Darkling teleports

    [HideInInspector] public float stateTimeElapsed;
    [HideInInspector] public Animator animator;

    public bool isWalking;
    public bool isTeleporting;
    private bool aiActive;
    private EnemyStateController stateController;


    public readonly int HASH_TELE_START = Animator.StringToHash("TriggerTeleportStart");
    public readonly int HASH_TELE_STOP  = Animator.StringToHash("TriggerTeleportStop");
    public const string PLAYER_TAG = "Player";


    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override IEnumerator Attack()
    {
        return null;
    }

    public override void Movement()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Physics.IgnoreCollision()
    }

    public override void OnDeath()
    {
        
    }

  
}

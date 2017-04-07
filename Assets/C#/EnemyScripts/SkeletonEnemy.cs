using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonEnemy : PatrolGroundEnemy {

    //private var
    private Animator anim;
    private Transform target;

    private bool isTurning;

    //some const because I am too lazy typing them
    private const string IS_RUNNING = "isRunning";
    private const string IS_WALKING = "isWalking";
    private const string IS_TURNING_LEFT  = "isTurningLeft";
    private const string IS_TURNING_RIGHT = "isTurningRight";

    // Use this for initialization
    void Start()
    {
        //use this for PatrolAround
        base.__init__();

        anim = GetComponent<Animator>();

        //set center: skeleton won't fall back & forth
        rb.centerOfMass = new Vector3(0, -4, 0);
    }

    public override IEnumerator Attack()
    {
        return null;
    }

    public override void Movement()
    {
        //if skeleton do nothing, make it patrol around
        if (!isPatrolling && !isResting && patrolPositions.Length > 0)
        {
            StartCoroutine(PatrolAround());
        }

        //transform.Rotate(transform.up * 3 *Time.deltaTime);
        //try Vector3.RotateTowards(); 
    }


    void TurnToFaceTarget(Vector3 target)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

    }

    public override void StartPatrolling()
    {
        anim.SetBool(IS_WALKING, true);
        base.StartPatrolling();
    }

    public override IEnumerator WaitBeforeChangeDirection()
    {
       
        //add animation
        anim.SetBool(IS_WALKING, false);

        //use same
        return base.WaitBeforeChangeDirection();
    }

   
}

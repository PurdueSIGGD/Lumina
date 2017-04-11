using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonEnemy : PatrolGroundEnemy {

    public Transform tempTar;

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
        rb.centerOfMass = new Vector3(0, -10, 0);

        isTurning = false;
    }

    public override IEnumerator Attack()
    {
        return null;
    }

    public override void Movement()
    {
        //if skeleton do nothing, make it patrol around
        //if (!isPatrolling && !isResting && patrolPositions.Length > 0)
        //{
        //    StartCoroutine(PatrolAround());
        //}

        //find if target: left || right
        
        

        Vector3 targetDir = tempTar.position - transform.position;

        //cos(a,b) = ( vector<a> . vector<b> ) / (...)
        //cos(a,b) < 0: left
        //cos(a,b) > 0: right
        float cos_angle = Vector3.Dot(transform.right, targetDir);
        if (cos_angle > 0)
        {
            anim.SetBool(IS_TURNING_LEFT, false);
            anim.SetBool(IS_TURNING_RIGHT, true);
        } else
        {
            anim.SetBool(IS_TURNING_RIGHT, false);
            anim.SetBool(IS_TURNING_LEFT, true);
        }
        
        
        float step = movementSpeed * Time.deltaTime;

        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
        transform.rotation = Quaternion.LookRotation(newDir);
        

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

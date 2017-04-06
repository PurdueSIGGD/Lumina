using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonEnemy : PatrolGroundEnemy {

    //private var
    private Animator anim;
    private Transform target;


    // Use this for initialization
    void Start()
    {
        //use this for PatrolAround
        base.__init__();

        anim = GetComponent<Animator>();
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

             
    }

    private void OnTriggerEnter(Collider other)
    {

    }

    public override void StartPatrolling()
    {
        anim.SetBool(IS_RUNNING, true);
        base.StartPatrolling();
    }

    public override IEnumerator WaitBeforeChangeDirection()
    {
       
        //add animation
        anim.SetBool(IS_RUNNING, false);

        //use same
        return base.WaitBeforeChangeDirection();
    }
}

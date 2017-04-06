using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonEnemy : PatrolGroundEnemy {

    private Transform target;

    public GameObject objectToRotate;



    // Use this for initialization
    void Start()
    {
        //use this for PatrolAround
        base.__init__();

    }

    public override IEnumerator Attack()
    {
        return null;
    }

    public override void Movement()
    {
        if (!isPatrolling && patrolPositions.Length > 0)
        {
            //StartCoroutine(PatrolAround());
        }

       transform.RotateAround(objectToRotate.transform.position, transform.up, 10 * Time.deltaTime);
        
        


    }

    private void OnTriggerEnter(Collider other)
    {

    }




}

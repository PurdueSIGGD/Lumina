using System;
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

    private Rigidbody rb;
    private SphereCollider sphereCollider;
    
   
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
    }


    public override IEnumerator Attack()
    {
        throw new NotImplementedException();
    }

    public override void Movement()
    {
        transform.position += transform.forward * Time.deltaTime * movementSpeed;
    }

    

}

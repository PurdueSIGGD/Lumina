using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class BlobEnemy : BaseEnemy {

    public BlobAmmution[] ammunitions;  //toxin for Blob to throw.
    public Transform target;    //target to attack
    public int numberOfAmmoShoot;   //number of blob-toxin that Blob will shoot each time

    private Rigidbody rb;   //rigidbody for moving, physics, stuff...

    void Start()
    {
        //get all necessary component
        rb = GetComponent<Rigidbody>();

        //ask for more 
        if (ammunitions.Length == 0)
        {
            Debug.Log("please put in some ammo for Blob :|");
        }
    }


    public override IEnumerator Attack()
    {
        throw new NotImplementedException();
    }

    public override void Movement()
    {
        Vector3 newPos = transform.position + Vector3.forward * Time.deltaTime * movementSpeed;
        rb.MovePosition(newPos);
    }

    private void OnTriggerEnter(Collider other)
    {
        //just attack player first
        if (other.tag != "Player") return;

        StartCoroutine(Attack());
    }




}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {

	Rigidbody playerPhysics;

	// Use this for initialization
	void Start () {
		playerPhysics = GetComponentInParent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/*
	* Setting the movement and applying the force to the player
	*/
	public void SetMovement(){
		playerPhysics.AddForce(new Vector3(Input.GetAxis ("Horizontal")*10,0,0));
		playerPhysics.AddForce(new Vector3(0,0,Input.GetAxis ("Vertical")*20));
	}
}

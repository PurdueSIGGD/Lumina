using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

	Rigidbody playerphysics;

	// Use this for initialization
	void Start () {
		playerphysics = GetComponentInParent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		SetMovement ();
	}

	/*
	* Setting the movement and applying the force to the player
	*/
	void SetMovement(){
		playerphysics.AddForce(new Vector3(Input.GetAxis ("Horizontal")*10,0,0));
		playerphysics.AddForce(new Vector3(0,0,Input.GetAxis ("Vertical")*20));
	}
}

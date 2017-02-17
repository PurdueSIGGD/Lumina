using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

	Rigidbody playerPhysics;
	BoxCollider playerCollider;
	float distToGround;

	// Use this for initialization
	void Start () {
		playerPhysics = GetComponentInParent<Rigidbody> ();
		playerCollider = GetComponentInParent<BoxCollider> ();
		distToGround = playerCollider.bounds.extents.y;
	}
	
	// Update is called once per frame
	void Update () {
		SetMovement ();
	}

	/*
	* Setting the movement and applying the force to the player
	*/
	void SetMovement(){
		playerPhysics.AddForce(new Vector3(Input.GetAxis ("Horizontal")*10,0,0));
		playerPhysics.AddForce(new Vector3(0,0,Input.GetAxis ("Vertical")*20));
		if (IsGrounded()) {
			playerPhysics.AddForce (new Vector3 (0, Input.GetAxis ("Jump") * 150, 0));
		}
	}

	/**
	 * Checks to see if player is touching the ground. Raycasts below the player with a margin of 0.2 (units?)
	 * below the bottom of the player's bounding box in order to account for irregularities in the ground, where
	 * ground may not be seen otherwise.
	 */
	bool IsGrounded(){
		return Physics.Raycast (this.transform.position, Vector3.down, distToGround + 0.2f);
	}
}

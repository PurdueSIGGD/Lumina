using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {

	BoxCollider playerCollider;
	float distToGround;
	Rigidbody playerPhysics;
	float lastJump;//the time since the last jump
	public bool isJumping;


	// Use this for initialization
	void Start () {
		playerPhysics = GetComponentInParent<Rigidbody> ();
		playerCollider = GetComponentInParent<BoxCollider> ();
		distToGround = playerCollider.bounds.extents.y;
		lastJump = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/*
	* Setting the movement and applying the force to the player
	* fb - the foward and backward movement from the axis
	* lr - the left and right movement from the keyboards
	*/
	public void SetMovement(float lr, float fb){
		playerPhysics.AddForce(new Vector3(lr*10,0,0));
		playerPhysics.AddForce(new Vector3(0,0,fb*20));
		if (isJumping) {
			if (IsGrounded()) {
				playerPhysics.AddForce (new Vector3 (0, 300, 0));
				Debug.Log ("You have Jumped!!");
			} else {
				isJumping = false;
			}
		}
	}

	/**
	 * checking to see if the person is airborne
	 * 
	 * */

	bool IsAirborne(){
		RaycastHit[] hits = Physics.BoxCastAll (this.transform.position - (Vector3.down * playerCollider.bounds.extents.y), playerCollider.bounds.extents, Vector3.down);
		bool hitValid = false;
		foreach (RaycastHit hit in hits) {
			Collider col = hit.collider;
			print ("Collider is: " + col);
			if (!col.CompareTag ("Player")) {
				print ("Collider was not player");
				hitValid = true;
				break;
			}
		}
		if (hitValid) {
			return false;
		}
		return true;

	}

	/**
	 * Checks to see if player is touching the ground. Raycasts below the player with a margin of 0.2 (units?)
	 * below the bottom of the player's bounding box in order to account for irregularities in the ground, where
	 * ground may not be seen otherwise.
	 * 
	 * added in a time period that a person has to wait before using the raycast again
	 */
	public bool IsGrounded(){ 
		float elaps = Time.realtimeSinceStartup - lastJump;

		if (elaps < 0.3f ) {
			return false;
		}
		lastJump = Time.realtimeSinceStartup;
		return Physics.Raycast (this.transform.position, Vector3.down, distToGround + 0.2f);
		}
		
}

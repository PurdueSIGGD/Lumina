using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {
	public static int SPRINT_MAX = 3;
	public static int SPRINT_COOLDOWN = 3;
	public static float MAX_X_SPEED = 2;
	public static float MAX_Z_SPEED = 5;

	CapsuleCollider playerCollider;
	Rigidbody playerPhysics;

	float distToGround;
	float lastJump; //the time since the last jump
	float sprintTime; //the elapsed time player has been sprinting
	float sprintRecharge; //the amount of time left before player can sprint again

	public bool isJumping;
	public bool isSprinting;

	// Use this for initialization
	void Start () {
		playerPhysics = GetComponentInParent<Rigidbody> ();
		playerCollider = GetComponentInParent<CapsuleCollider> ();

		distToGround = playerCollider.bounds.extents.y;
		lastJump = 0;
		sprintTime = 0;
		sprintRecharge = 0;

		isJumping = false;
		isSprinting = false;
	}

	/*
	* Setting the movement and applying the force to the player
	* fb - the foward and backward movement from the axis
	* lr - the left and right movement from the keyboards
	*/
	public void SetMovement(float lr, float fb, bool sprintPressed){
		UpdateCooldowns ();
		ApplyHorizontalMovement (lr, fb, sprintPressed);
		if (isJumping) {
			if (IsGrounded()) {
				playerPhysics.AddForce (new Vector3 (0, 300, 0));
			} else {
				isJumping = false;
			}
		}
	}

	private void ApplyHorizontalMovement(float x, float z, bool sprintPressed){
		ApplySprint (sprintPressed);
		if (isSprinting) {
			print ("SPRINTING");
		}
		float sprintModifier = isSprinting ? 1.5f : 1f;
		if (!(Math.Abs(playerPhysics.velocity.x) >= MAX_X_SPEED*sprintModifier)) {
			playerPhysics.AddForce (new Vector3 (x * 10 * sprintModifier, 0, 0));
		}
		if (!(Math.Abs (playerPhysics.velocity.z) >= MAX_Z_SPEED*sprintModifier)) { 
			playerPhysics.AddForce (new Vector3 (0, 0, z * 20 * sprintModifier));
		}
	}

	/**
	 * Checks to see if player is touching the ground. Raycasts below the player with a margin of 0.2 (units?)
	 * below the bottom of the player's bounding box in order to account for irregularities in the ground, where
	 * ground may not be seen otherwise.
	 * 
	 * added in a time period that a person has to wait before using the raycast again
	 */
	private bool IsGrounded(){ 
		float elaps = Time.realtimeSinceStartup - lastJump;

		if (elaps < 0.3f ) {
			return false;
		}
		lastJump = Time.realtimeSinceStartup;
		return Physics.Raycast (this.transform.position, Vector3.down, distToGround + 0.2f);
	}

	private bool CanSprint(){
		return sprintTime < SPRINT_MAX && !(sprintRecharge > 0);
	}

	private void ApplySprint(bool sprintPressed){
		if (sprintPressed) {
			if (!isSprinting && CanSprint()) {
				isSprinting = true;
			}
			if (isSprinting && !CanSprint()) {
				isSprinting = false;
				sprintRecharge = (sprintTime / SPRINT_MAX) * SPRINT_COOLDOWN;
				sprintTime = 0;
			}
		} else {
			if (isSprinting) {
				isSprinting = false;
				sprintRecharge = (sprintTime / SPRINT_MAX) * SPRINT_COOLDOWN;
				sprintTime = 0;
			}
		}
	}

	private void UpdateCooldowns() {
		if (isSprinting) {
			sprintTime += Time.deltaTime;
		}
		if (sprintRecharge > 0) {
			sprintRecharge -= Time.deltaTime;
			if (sprintRecharge < 0) {
				sprintRecharge = 0;
			}
		}
	}
}
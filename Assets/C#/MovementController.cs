using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {
	public static int SPRINT_MAX = 3;
	public static int SPRINT_COOLDOWN = 3;
	public static float MAX_X_SPEED = 4;
	public static float MAX_Z_SPEED = 6;

	CapsuleCollider playerCollider;
	Rigidbody playerPhysics;

	float distToGround; //distance from the center of the player to the bottom of their hitbox
	float lastJump; //the time since the last jump
	float sprintTime; //the elapsed time player has been sprinting
	float sprintRecharge; //the amount of time left before player can sprint again
	Vector3 recoilVec;//vector used to set and maintain the rotation of the player and the camera

	public bool isJumping;
	public bool isSprinting;
	public GameObject cameraObj;

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

	/**
	*Getting the mouse movements and move your camera
	*/
	public void MoveCamera(float x, float y){
		cameraObj.transform.Rotate(-y*5+(recoilVec.x*-1),recoilVec.y,recoilVec.z);
		gameObject.transform.Rotate (0, x * 5, 0);
		if (recoilVec.x > 0) {
			recoilVec = new Vector3 (recoilVec.x - 10 * Time.deltaTime, recoilVec.x, recoilVec.z);
		} else {
			recoilVec = new Vector3 (0,recoilVec.y,recoilVec.z);
		}
	}

	/**
	 * Separate function for movement in the horizontal direction.
	 */
	private void ApplyHorizontalMovement(float x, float z, bool sprintPressed){
		ApplySprint (sprintPressed);
		if (isSprinting) {
			print ("SPRINTING");
		}
		float sprintModifier = isSprinting ? 1.5f : 1f;
		if (!(Math.Abs(playerPhysics.velocity.x) >= MAX_X_SPEED*sprintModifier)) {
			playerPhysics.AddRelativeForce (new Vector3 (x * 10 * sprintModifier, 0, 0));
		}
		if (!(Math.Abs (playerPhysics.velocity.z) >= MAX_Z_SPEED*sprintModifier)) { 
			playerPhysics.AddRelativeForce (new Vector3 (0, 0, z * 20 * sprintModifier));
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

	/**
	 * Whether or not the player is allowed to sprint
	 */
	private bool CanSprint(){
		return sprintTime < SPRINT_MAX && !(sprintRecharge > 0);
	}

	/**
	 * Updates whether the player is sprinting or not based on if the key is pressed
	 * and if the player is allowed to sprint
	 */
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

	/**
	 * Updates the sprint time and the recharge time to sprint
	 */
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
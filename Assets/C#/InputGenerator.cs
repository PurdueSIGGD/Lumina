using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputGenerator : MonoBehaviour {

	public MovementController playerMovement;
	Rigidbody playerPhysics;

	float jumpInput;

	public bool isAttacking;
	bool doneAttacking;
	public bool isCasting;
	bool doneCasting;
	public bool isInteracting;
	bool doneInteracting;
	public float attackTimeSet;
	float attackTime;
	public float castTimeSet;
	public float castTime;

	bool countDown;//when a button is pressed for a time

	// Use this for initialization
	void Start () {
		playerPhysics = GetComponentInParent<Rigidbody> ();
		isAttacking = false;
		isCasting = false;
		attackTimeSet = 0.5f;
		castTimeSet = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
		ButtonStates ();
		playerMovement.SetMovement(Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"), Input.GetAxis ("Sprint") > 0);
		playerMovement.MoveCamera (Input.GetAxis ("Mouse X"),Input.GetAxis ("Mouse Y"));
		if((jumpInput = Input.GetAxis ("Jump")) > 0){
			playerMovement.isJumping = true;
		}
		if (Time.timeScale != 0 || Input.GetAxis ("Cancel") != 0) {
			Cursor.lockState = CursorLockMode.Locked;
		} else {
			Cursor.lockState = CursorLockMode.None;
		}
	}



	/**
	* Gets the potential state changes for a player when they press certain buttons. how this is used
	* in the furture remains to be determined. 
	*/
	void ButtonStates(){
		if (Input.GetAxis ("Interact") > 0) {
			if (!isInteracting) {
				isInteracting = true;
				Debug.Log ("Trying to interact");
			}
		}else if(Input.GetAxis ("Interact") == 0){
			isInteracting = false;
		}

		if(Input.GetAxis ("Fire1") > 0){
			if (!isAttacking) {
				isAttacking = true;
				attackTime = attackTimeSet;
				countDown = true;
				Debug.Log ("Trying to attack");
				//Attacking method
			}
		}else if(Input.GetAxis ("Fire1") == 0){
			isInteracting = false;
		}

		if(Input.GetAxis ("Fire2") > 0){
			if (!isCasting) {
				isCasting = true;
				castTime = castTimeSet;
				countDown = true;
				Debug.Log ("Trying to Cast");
				//Casting method
			}
		}

		attackTime -= Time.deltaTime;
		castTime -= Time.deltaTime;

		if(countDown){
			int donePress = 0;

			if(attackTime <= 0){
				isAttacking = false;
				donePress++;
			}
			if(castTime <= 0){
				isCasting = false;
				donePress++;
			}

			if(donePress >= 2){
				countDown = false;
			}
		}

	}


}

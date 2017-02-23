using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputGenerator : MonoBehaviour {

	public MovementController playerMovement;
	Rigidbody playerPhysics;

	float jumpInput;

	// Use this for initialization
	void Start () {
		playerPhysics = GetComponentInParent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		playerMovement.SetMovement(Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"), Input.GetAxis ("Sprint") > 0);
		playerMovement.MoveCamera (Input.GetAxis ("Mouse X"),Input.GetAxis ("Mouse Y"));
		if((jumpInput = Input.GetAxis ("Jump")) > 0){
			playerMovement.isJumping = true;
		}
	}



	/**
	* Gets the potential state changes for a player when they press certain buttons. how this is used
	* in the furture remains to be determined. 
	*/
	void ButtonStates(){
		if (Input.GetAxis ("Interact") > 0) {
			Debug.Log ("Trying to interact");
		}
	}


}

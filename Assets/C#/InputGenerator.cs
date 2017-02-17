using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

	public MovementController playerMovement;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		playerMovement.SetMovement();
	}



	/*
	
	Gets the potential state changes for a player when they press certain buttons. how this is used
	in the furture remain to be determined. 
	*/

	void ButtonStates(){
		if (Input.GetAxis ("Interact") > 0) {
			Debug.Log ("Trying to interact");
		}
	}
}

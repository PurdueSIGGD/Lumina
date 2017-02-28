using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputGenerator : MonoBehaviour {

	public MovementController playerMovement;
    public InventoryController playerInventory;

	Rigidbody playerPhysics;

	float jumpInput;


	// Use this for initialization
	void Start () {
		playerPhysics = GetComponentInParent<Rigidbody> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		ButtonStates ();
        CursorStates();
		
        
	}

    /**
     * We update the button lock states here
     */
    void CursorStates()
    {
        // If we are paused, mouse will appear
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
        playerInventory.Interact(Input.GetAxis("Interact") > 0);
		

		if(Input.GetAxis ("Fire1") > 0){
            //Debug.Log("WeaponController1 True");

        }
        else if(Input.GetAxis ("Fire1") == 0){
            //Debug.Log("WeaponController1 false");

        }

        if (Input.GetAxis ("Fire2") > 0){
			//Debug.Log ("WeaponController1 True");

		} else if (Input.GetAxis("Fire2") == 0)
        {
            //Debug.Log("WeaponController2 false");
        }


        playerMovement.SetMovement(Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"), Input.GetAxis ("Sprint") > 0);
		playerMovement.MoveCamera (Input.GetAxis ("Mouse X"),Input.GetAxis ("Mouse Y"));
		if((jumpInput = Input.GetAxis ("Jump")) > 0){
			playerMovement.isJumping = true;
		}

	}


}

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

        // Might need to explain myself here. This is crap for animation
        // For animation, we have 4 layers at the moment. Each with their corresponding bone masks
        // 0: Both Hands (animations that take both, like reloading or clapping)
        // 1: Right Hand
        // 2: Left Hand
        // 3: Movement (Movement like shaking when running, or taking a hit from an enemy)
        // 
        // Layer 0 overrides all movement, so I have to manually disable the other right/left animations while one is running by setting their weight to zero
        // Once all that is over, each hand has a path they can take to "recover" from both hand movements, like coming back up from below.
        // I signal these methods by calling "DoneWithBoth" to be true.
        if (playerMovement.viewmodelAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !playerMovement.viewmodelAnimator.IsInTransition(0)) {
            // We want to reset the weights here when we are done, so we check to see if we are in the idle position (when both hands are finished, they will return to idle which is an empty state)
            this.playerMovement.viewmodelAnimator.SetLayerWeight(1, 1);
            this.playerMovement.viewmodelAnimator.SetLayerWeight(2, 1);
            this.playerMovement.viewmodelAnimator.SetBool("DoneWithBoth", true);
        } 
        if (Input.GetAxis ("Fire1") > 0){
            // And if we want to initiate some two-hand movement here, we verify we are in idle
            if (playerMovement.viewmodelAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !playerMovement.viewmodelAnimator.IsInTransition(0)) {
                // Stop the done with both flag (so they can't recover)
                this.playerMovement.viewmodelAnimator.SetBool("DoneWithBoth", false);
                // Send the double-hand movement we would like, in this case I have a method that uses both hands in a punch movement
                this.playerMovement.viewmodelAnimator.SetTrigger("Both_Punch");
                this.playerMovement.viewmodelAnimator.SetLayerWeight(1, 0);
                this.playerMovement.viewmodelAnimator.SetLayerWeight(2, 0);
            }
           

            //Debug.Log("WeaponController1 True");

        } else if(Input.GetAxis ("Fire1") == 0){
            //Debug.Log("WeaponController1 false");
            
        }

        if (Input.GetAxis ("Fire2") > 0){
            this.playerMovement.viewmodelAnimator.SetBool("LMagicAttack", true);

            //Debug.Log ("WeaponController1 True");

        } else if (Input.GetAxis("Fire2") == 0)
        {
            this.playerMovement.viewmodelAnimator.SetBool("LMagicAttack", false);

            //Debug.Log("WeaponController2 false");
        }


        playerMovement.SetMovement(Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"), Input.GetAxis ("Sprint") > 0);
		playerMovement.MoveCamera (Input.GetAxis ("Mouse X"),Input.GetAxis ("Mouse Y"));
		if((jumpInput = Input.GetAxis ("Jump")) > 0){
			playerMovement.isJumping = true;
		}

	}


}

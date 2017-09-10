﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputGenerator : MonoBehaviour {

    
    public UIController uiController;
    

    public MovementController playerMovement;
    public InventoryController playerInventory;
	       
    public WeaponController leftPlayerWeaponController;
    public WeaponController rightPlayerWeaponController;

    Rigidbody playerPhysics;

	float jumpInput;


    /// <summary>
    /// Set the running status of game
    /// </summary>
    public bool isGamePausing { get; private set; }

    private bool cursorsEnabled;

    // Use this for initialization
    void Start () {
		playerPhysics = GetComponentInParent<Rigidbody> ();
        isGamePausing = false;
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
        if (!cursorsEnabled) { 
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
        rightPlayerWeaponController.Attack(Input.GetAxis("Fire1") > 0);
        if (Input.GetAxis("RightCycleWeapon") > 0) rightPlayerWeaponController.SwitchWeapon();
        leftPlayerWeaponController.Attack(Input.GetAxis("Fire2") > 0);
        if (Input.GetAxis("LeftCycleWeapon") > 0) leftPlayerWeaponController.SwitchWeapon();


        // Might need to explain myself here. This is crap for animation
        // For animation, we have 4 layers at the moment. Each with their corresponding bone masks
        // 0: Both Hands (animations that take both, like reloading or clapping)
        // 1: Right Hand
        // 2: Left Hand
        // 3: Movement (Movement like shaking when running, or taking a hit from an enemy)
        // 4: Right Hand Movement (Camera movement specifically from right hand movement)
        // 5: Left Hand Movement (Camera movement specifically from left hand movement)
        // 
        // Layer 0 overrides all movement, so I have to manually disable the other right/left animations while one is running by setting their weight to zero
        // Once all that is over, each hand has a path they can take to "recover" from both hand movements, like coming back up from below.
        // I signal these methods by calling "DoneWithBoth" to be true.
        /*
        if (playerMovement.viewmodelAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !playerMovement.viewmodelAnimator.IsInTransition(0)) {
            // If the Both Hands layer is done with its shit, and not in transition
            // We want to reset the weights here when we are done, so we check to see if we are in the idle position (when both hands are finished, they will return to idle which is an empty state)
            this.playerMovement.viewmodelAnimator.SetLayerWeight(1, 1);
            this.playerMovement.viewmodelAnimator.SetLayerWeight(2, 1);
            this.playerMovement.viewmodelAnimator.SetBool("DoneWithBoth", true);
        }
       

        if (Input.GetAxis ("Fire1") > 0) {            
            // In order to signal the both hands animation, we have to stop the single hands animation
            // So we call DoneWithBoth to be false, and they will start ending their own animation
            this.playerMovement.viewmodelAnimator.SetBool("DoneWithBoth", false);
            // We don't have to wait until they are done with their transitions if we want an instant snap
            if (playerMovement.viewmodelAnimator.GetCurrentAnimatorStateInfo(1).IsTag("WaitUntilBothDone") &&
           playerMovement.viewmodelAnimator.GetCurrentAnimatorStateInfo(2).IsTag("WaitUntilBothDone") &&
           !playerMovement.viewmodelAnimator.IsInTransition(0)) {
                // We set the weight to be zero
                this.playerMovement.viewmodelAnimator.SetLayerWeight(1, 0);
                this.playerMovement.viewmodelAnimator.SetLayerWeight(2, 0);

                // Send the double-hand movement we would like, in this case I have a method that uses both hands in a punch movement
                this.playerMovement.viewmodelAnimator.SetBool("Punching", true);
            }


            //Debug.Log("WeaponController1 True");

        } else if(Input.GetAxis ("Fire1") == 0){
            //Debug.Log("WeaponController1 false");
            this.playerMovement.viewmodelAnimator.SetBool("Punching", false);
        }

        if (Input.GetAxis ("Fire2") > 0){
            this.playerMovement.viewmodelAnimator.SetBool("LMagicAttack", true);

            //Debug.Log ("WeaponController1 True");

        } else if (Input.GetAxis("Fire2") == 0)
        {
            this.playerMovement.viewmodelAnimator.SetBool("LMagicAttack", false);

            //Debug.Log("WeaponController2 false");
        }*/


        playerMovement.SetMovement(Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"), Input.GetAxis ("Sprint") > 0);

        //if game is pausing, stop moving the camera
        if (!cursorsEnabled)
            playerMovement.MoveCamera (Input.GetAxis ("Mouse X"),Input.GetAxis ("Mouse Y"));

        //if relate to UI
        //roundabout way to doing it, optimize later.
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            uiController.ToggleUI(KeyCode.Tab);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            uiController.ToggleUI(KeyCode.I);
        }

        //jump
        if ((jumpInput = Input.GetAxis ("Jump")) > 0){
			playerMovement.isJumping = true;
		}


        //if (!playerPause) return;


		//if (Input.GetAxis ("Pause") > 0) {
		//	playerPause.changeState = true;
		//}

  //      else {
		//	if(playerPause.changeState){
		//		if (playerPause.getPause()) {
		//			playerPause.setPause (false);
		//			playerPause.closePauseOpenHUD ();
		//			playerPause.changeState = false;
		//		} else {
		//			playerPause.setPause (true);
		//			playerPause.closeHUDOpenPause ();
		//			playerPause.changeState = false;
		//		}
		//	}
		//}
			

	}
    public void EnableCursors() {
        cursorsEnabled = true;
    }
    public void DisableCursors() {
        cursorsEnabled = false;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        isGamePausing = true;
        EnableCursors();
    }

    public void ResumeGame()
    {
        isGamePausing = false;
        Time.timeScale = 1;
        DisableCursors();
    }

}

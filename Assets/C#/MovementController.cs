﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovementController : MonoBehaviour {
	public static int SPRINT_MAX = 7;
	public static int SPRINT_COOLDOWN = 2;
	public static float MAX_X_SPEED = 4;
	public static float MAX_Z_SPEED = 6;
    public static float JUMP_POWER = 400;


    CapsuleCollider playerCollider;
	Rigidbody playerPhysics;

	float distToGround; //distance from the center of the player to the bottom of their hitbox
	float lastJump; //the time since the last jump
	float sprintTime; //the elapsed time player has been sprinting
	float sprintRecharge; //the amount of time left before player can sprint again
	Vector3 rotationVector;//vector used to set and maintain the rotation of the player and the camera

    private Vector3 outsideLocation; // The location that we will return to when exiting a dungeon
    public bool disableMovement = false; // Used for scripted events like exiting the boat, or entering a dungeon

	public bool isJumping;
	public bool isSprinting;
    public bool canMove = true;
    public bool blocked;
	public GameObject cameraObj;
    public Animator viewmodelAnimator;

    public int[] ignorePlayerCollisionLayers;
    private Camera playerCam;

    private Door.FadeType fadeType; //The fade type that we are expected to do next

    private StatsController myStats;
    private InventoryController myInventory;
    private WeaponController rightWeaponController;
    private WeaponController leftWeaponController;

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this.gameObject);
		playerPhysics = GetComponentInParent<Rigidbody> ();
		playerCollider = GetComponentInParent<CapsuleCollider> ();
        playerCam = transform.GetComponentsInChildren<Camera>()[0];
        myStats = this.GetComponent<StatsController>();
        myInventory = this.GetComponent<InventoryController>();
        rightWeaponController = myInventory.rightWeaponController;
        leftWeaponController = myInventory.leftWeaponController;
        
        // We load even if we just reset
        GameSaveManager.LoadGameStats(myStats, myInventory, rightWeaponController, leftWeaponController);
        // Do we have a position to go to? If so, go there
        Vector3 location;
        if ((location = GameSaveManager.GetPlayerLocation()) != Vector3.zero) {
            // Location location location
            transform.position = location;
            transform.localEulerAngles = GameSaveManager.GetPlayerRotation();
        }

		distToGround = playerCollider.bounds.extents.y/2.5f;
        //print(distToGround);
		lastJump = 0;
		sprintTime = 0;
		sprintRecharge = 0;
        fadeType = 0;

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
		bool couldMove = ApplyHorizontalMovement (lr, fb, sprintPressed);
		if (isJumping) {
            if (!canMove) return;
            if (IsGrounded()) {
				playerPhysics.AddForce (new Vector3 (0, JUMP_POWER, 0));
				lastJump = 0;
			} else {
				isJumping = false;
			}
		}
        
        bool walking = (lr != 0 || fb != 0) && IsGrounded() && couldMove;
        viewmodelAnimator.SetBool("Walking", walking && !disableMovement);
        viewmodelAnimator.SetBool("Running", this.isSprinting && walking && !disableMovement);
    }

	/**
	*Getting the mouse movements and move your camera
	*/
	public void MoveCamera(float x, float y){
        if (!canMove) return;

        float newDeltaX = -y * 5 + (rotationVector.x * -1);
        float newX = cameraObj.transform.rotation.eulerAngles.x + newDeltaX;
        //We do some fancy math to ensure 0 < newX < 360, nothing more
        newX =(newX+360) % 360;
        //Ensure it doesn't go past our top or low bounds
        if ((newX > 0 && newX < 90) || (newX < 360 && newX > 270)) {
            // Camera rotation
            cameraObj.transform.Rotate(newDeltaX, rotationVector.y, rotationVector.z);
        } else {
            // We don't want you to look all the way behind you, that's weird
        }

        // Body rotate
        gameObject.transform.Rotate (0, x * 5, 0);

    }

	/**
	 * Separate function for movement in the horizontal direction.
     * 
     * Returns true if able to move, false if no movement
	 */
	private bool ApplyHorizontalMovement(float x, float z, bool sprintPressed){
        //if (!canMove) return false;

		ApplySprint (sprintPressed);

		float sprintModifier = isSprinting ? 1.5f : 1f;
        float airborneModifier = IsGrounded() ? 1 : .75f;

        playerPhysics.MovePosition(transform.position + (transform.TransformDirection(new Vector3(x, 0, z)) * Time.deltaTime * sprintModifier * airborneModifier * 10));
        
        return true;
    }

	/**
	 * Checks to see if player is touching the ground. Raycasts below the player with a margin of 0.2 (units?)
	 * below the bottom of the player's bounding box in order to account for irregularities in the ground, where
	 * ground may not be seen otherwise.
	 * 
	 * added in a time period that a person has to wait before using the raycast again
	 */
	private bool IsGrounded(){ 
		if (lastJump < 0.5f ) {
			return false;
		}
		return Physics.Raycast (this.transform.position, Vector3.down, distToGround);
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
	 * Updates the sprint time and the recharge time to sprint and the time since the last jump
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
		lastJump += Time.deltaTime;
	}

    /**
     * Broadcasted by stats controller 
     */
    void Death() {
        canMove = false;
        cameraObj.transform.eulerAngles = new Vector3(0, cameraObj.transform.eulerAngles.y, cameraObj.transform.eulerAngles.z);
        // Stop friggin flying across the friggin place
        playerPhysics.isKinematic = true;
    }
    void NotDeath() {
        
        playerPhysics.isKinematic = false;
        canMove = true;
    }
    void PrepareToEnterDungeon() {
        SaveGame();
        outsideLocation = transform.position;
        StopMoving();
    }
    void StopMoving() {
        isSprinting = false;
        disableMovement = true;
        viewmodelAnimator.SetBool("Walking", false);
        viewmodelAnimator.SetBool("Running", false);
    }
    
    void EnterDungeon() {
        // We expect the screen to be faded to black, so we can do crazy movements

        transform.position = new Vector3(0, 30, 0);
        disableMovement = false;
        // change skybox settings to be all black
        playerCam.clearFlags = CameraClearFlags.Color;
        playerCam.backgroundColor = new Color(0.08f, 0.08f, 0.08f);
        this.SendMessage("DungeonFeet"); // For footsteps

        foreach (WeaponController w in GetComponents<WeaponController>()) {
            w.clearSwitchCooldown();
        }
    }
    void SaveGame()
    {
        GameSaveManager.SaveGame(myStats, myInventory, rightWeaponController, leftWeaponController);
    }
    void ExitDungeon() {

        //print(outsideLocation);
        transform.position = outsideLocation;
        transform.localEulerAngles = GameSaveManager.GetPlayerRotation();
        // Play whatever animations
        playerCam.clearFlags = CameraClearFlags.Skybox;
        disableMovement = false;

        this.SendMessage("WorldFeet"); // For footsteps
        foreach (WeaponController w in GetComponents<WeaponController>()) {
            w.clearSwitchCooldown();
        }
    }
    void SwitchMap(BoatDoor.SceneChoice sceneChoice) {
        playerCam.clearFlags = CameraClearFlags.Skybox;
        disableMovement = false;
        transform.position = sceneChoice.spawnPosition;
        foreach (WeaponController w in GetComponents<WeaponController>()) {
            w.clearSwitchCooldown();
        }
    }
    /**
     * When we have to prepare to fade back in or out after a scene is loaded
     */
    void PrepSceneSwitchFade(Door.FadeType typeToFade) {
        this.fadeType = typeToFade;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        SceneSelectionCanvas canvas = GameObject.FindObjectOfType<SceneSelectionCanvas>();
        if (canvas) {
            if (fadeType == Door.FadeType.Dark) {
                canvas.SendMessage("FadeFromBlack");
            } else if (fadeType == Door.FadeType.Light) {
                canvas.SendMessage("FadeFromWhite");
                SaveGame(); //I am assuming we are either outside or on a new map, so that's happening
            }
        }
        fadeType = 0;
    }

    private bool canMoveTo(Vector3 pos) {
        if (disableMovement) {
            return false;
        }
        //RenderVolume(pos + Vector3.up * (playerCollider.height / 2), pos - Vector3.up * (playerCollider.height / 2), playerCollider.radius, Vector3.forward, playerCollider.radius);
        //RaycastHit[] hits = Physics.CapsuleCastAll(pos + Vector3.up * (playerCollider.height / 2), pos - Vector3.up * (playerCollider.height / 2), playerCollider.radius/2, Vector3.forward);

        //RaycastHit[] hits = Physics.BoxCastAll(pos, Vector3.one, Vector3.forward/100000);
        Vector3 dir = pos - transform.position;
        float dist = Vector3.Distance(pos, transform.position);
        int layermask = 0;
        // Make sure we ignore anything that we won't run into
        foreach (int layer in this.ignorePlayerCollisionLayers) {
            layermask += 1 << layer;
        }
        layermask = ~layermask;
        RaycastHit[] hitsForward = Physics.RaycastAll(new Ray(transform.position + Vector3.forward * 0.5f, dir), dist + 0.5f, layermask);
        RaycastHit[] hitsRight = Physics.RaycastAll(new Ray(transform.position + Vector3.right * 0.5f, dir), dist + 0.5f, layermask);
        RaycastHit[] hitsBack = Physics.RaycastAll(new Ray(transform.position + Vector3.back * 0.5f, dir), dist + 0.5f, layermask);
        RaycastHit[] hitsLeft = Physics.RaycastAll(new Ray(transform.position + Vector3.left * 0.5f, dir), dist + 0.5f, layermask);

        Debug.DrawLine(transform.position + Vector3.forward * 0.5f, transform.position + Vector3.forward * 0.5f + dir);
        if (hitsIn(hitsForward) || hitsIn(hitsRight) || hitsIn(hitsBack) || hitsIn(hitsLeft)) {
            blocked = true;
            return false;
        }
       
        blocked = false;
        return true;
    }

    private bool hitsIn(RaycastHit[] hits) {
        foreach (RaycastHit hit in hits) {
            if (!hit.collider.isTrigger && hit.collider.gameObject != this.gameObject) {
                //print(hit.transform);
                //blocked = true;
                return true;
            }

        }
        return false;
    }
    
}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {
    private static Vector3 EQUIPPED_WEAPON_SCALE = new Vector3(0.008f, 0.008f, 0.008f);
    private static float WEAPON_SWITCH_TIME = .3f;

    public Transform weaponBone;
    public Transform cameraBone;
    public Animator viewmodelAnimator;
    public StatsController myStats;

    public Weapon[] weapons = new Weapon[2];
    public int weaponIndex;
    public int weaponCount;

    private float switchCooldown;
    public bool weaponBusy; // If our weapon is reloading, or drawing
    private bool canEquip; //Set to true if we are not in progress of anything

    public string controllerSide;

    // ____ Variables for pending switching ____
    private string pendingPackType; // String determining what kind of type we want to do, "" if nothing
    private Weapon pendingOldWeapon;
    private Weapon pendingNewWeapon;

	// Use this for initialization
	void Start () {
        pendingPackType = "";
        weaponIndex = 0;
        weaponCount = 0;

        viewmodelAnimator.SetBool("DoneWithBoth", true);
        viewmodelAnimator.SetInteger(controllerSide + "EquippedWeapon", 0);
        viewmodelAnimator.SetInteger(controllerSide + "AttackNum", 0);

    }

    // Update is called once per frame
    void Update () {
        int layerIndex = controllerSide == "R" ? 1 : 2;

        weaponBusy = !(viewmodelAnimator.GetCurrentAnimatorStateInfo(layerIndex).IsTag("Idle") && !viewmodelAnimator.IsInTransition(layerIndex));

        
        if (pendingPackType != "" && (viewmodelAnimator.GetCurrentAnimatorStateInfo(layerIndex).IsTag("TransferDone"))) {
            // If we are done throwing or packing or whatever
            if (pendingPackType == "Pack" && pendingOldWeapon) { //We make sure the old weapon exists
                // Packing
                // Hide old weapon

                pendingOldWeapon.transform.localScale = Vector3.zero;
                if (pendingOldWeapon is Magic) {
                    // Set the mesh to be scale 0
                    ((Magic)pendingOldWeapon).mesh.localScale = Vector3.zero;

                } 
            } else if (pendingPackType == "Drop" && pendingOldWeapon) { 
                // Throwing
                // Drop last item
                pendingOldWeapon.transform.parent = null;
                pendingOldWeapon.transform.position += cameraBone.transform.right * 2; //Throw to the right
                pendingOldWeapon.transform.localScale = Vector3.one;
                if (pendingOldWeapon is Magic) {
                    // Set the mesh to be proper scale
                    ((Magic)pendingOldWeapon).mesh.localScale = Vector3.one;
                }
                pendingOldWeapon.transform.localEulerAngles += new Vector3(0, 180, 0);
                pendingOldWeapon.GetComponent<Rigidbody>().isKinematic = false;
                pendingOldWeapon.GetComponent<Rigidbody>().AddForce(cameraBone.transform.right * 10);
                pendingOldWeapon.GetComponent<Collider>().isTrigger = false;
                MoveToLayer(pendingOldWeapon.transform, 0); //Default layer
                pendingOldWeapon.setLookObj(null);
                pendingOldWeapon.setPlayerAnim(null);
                pendingOldWeapon.setControllerSide("");

                
            }
            //print("Adding new item");
            // Set this weapon to be active
            pendingNewWeapon.transform.parent = weaponBone;

            pendingNewWeapon.transform.localScale = EQUIPPED_WEAPON_SCALE;
            if (pendingNewWeapon is Magic) {
                // Set the mesh to be scale 0
                ((Magic)pendingNewWeapon).mesh.localScale = Vector3.zero;
                ((Magic)pendingNewWeapon).statsController = myStats;
            } 
            pendingNewWeapon.transform.localPosition = Vector3.zero;
            pendingNewWeapon.transform.localEulerAngles = Vector3.zero;
            pendingNewWeapon.GetComponent<Rigidbody>().isKinematic = true;
            pendingNewWeapon.GetComponent<Collider>().isTrigger = true;
            MoveToLayer(pendingNewWeapon.transform, 8); //Viewmodel layer for camera rendering
            pendingNewWeapon.setPlayerAnim(viewmodelAnimator);
            pendingNewWeapon.setLookObj(cameraBone);
            pendingNewWeapon.setControllerSide(controllerSide);

            // Tell it to set animator information
            SwitchWeaponFinished();

            
        }
    }

    public void Attack(bool mouseDown) {
        // We have a current weapon
        // And the weapon is not busy
        if (weaponCount > 0) {
            if (weaponBusy && controllerSide == "R") mouseDown = false;
            // Attack
            weapons[weaponIndex].Attack(mouseDown);
        }
    }

    public void SwitchWeapon() {
        if (Time.timeSinceLevelLoad - switchCooldown < WEAPON_SWITCH_TIME 
            || weaponCount <= 1
            || pendingPackType != "") {
            return;
        }
        switchCooldown = Time.timeSinceLevelLoad;

        if (weaponIndex == 1) {
            pendingOldWeapon = weapons[1];
            pendingNewWeapon = weapons[0];
            SwitchWeapon(0, "Pack");
        } else {
            pendingOldWeapon = weapons[0];
            pendingNewWeapon = weapons[1];
            SwitchWeapon(1, "Pack"); 
        }
    }
    /**
     * Called when we switch from one held weapon to another
     * Should be called by input controller
     * 
     * packType: either "Pack" or "Drop" depending on what you want to do with the old weapon
     */
    public void SwitchWeapon(int newIndex, string packType) {
        if (weaponCount == 0
            || pendingPackType != "") {
            return;
        }

        int layerIndex = controllerSide == "R" ? 1 : 2;
        if (!viewmodelAnimator.GetCurrentAnimatorStateInfo(layerIndex).IsTag("Idle")) return;

        //print("Switching, weapon index: " + newIndex + " weapon count: " + weaponCount);

        weaponIndex = newIndex;
        //We pack an item if switching from one to another, throw if we are dropping our current weapon

        pendingPackType = packType;
        viewmodelAnimator.SetTrigger(controllerSide + packType);
        viewmodelAnimator.SetInteger(controllerSide + "EquippedWeapon", pendingNewWeapon.animationType);
        


    }
    public void SwitchWeaponFinished() {
        //pendingNewWeapon.transform.localScale = EQUIPPED_WEAPON_SCALE;
        // Each animation is 1/2 second long, so I set the speed based off of that so it syncs
        // i.e. if a weapon's AttackSpeed is 1, I set the AttackSpeed to .5f, so it lasts 1 sec instead of half a second
        if (pendingNewWeapon is Magic) {
            // We cheat here because there is a tiny overlap where we have to compensate the magic
            // So we make the speed a bit slower
            viewmodelAnimator.SetFloat(controllerSide + "AttackSpeed", 1 / (2 * (pendingNewWeapon.timeToAttack + 0.1f)));
            viewmodelAnimator.SetFloat(controllerSide + "CooldownSpeed", 1 / (2 * (pendingNewWeapon.timeToCooldown + 0.1f)));
        } else {
            viewmodelAnimator.SetFloat(controllerSide + "AttackSpeed", 1 / (2 * pendingNewWeapon.timeToAttack));
            viewmodelAnimator.SetFloat(controllerSide + "CooldownSpeed", 1 / (2 * pendingNewWeapon.timeToCooldown));

        }
        

        pendingPackType = "";
    }
    /**
     * Called when we pick up a new weapon
     * Should be called by inventory controller
     */
    public void EquipWeapon(Weapon newWeapon) {
      
        if (
            weaponBusy || // Can't equip if reloading
            pendingPackType != ""
            ) { //Can't equip if currently moving weapons around
            return;
        }

        pendingNewWeapon = newWeapon;

        

        //print("Trying to equip");
        if (weaponCount >= weapons.Length) {
            // If we already have plenty of weapons
            // We drop one and equip the other
            
            pendingOldWeapon = weapons[weaponIndex];
            weapons[weaponIndex] = newWeapon;
            SwitchWeapon(weaponIndex, "Drop");
        } else {
            // We can't get enough weapons! Adding a new one
            // And packing the other
            pendingOldWeapon = weapons[weaponIndex];
            weapons[weaponCount] = newWeapon;
            weaponIndex = weaponCount;
            weaponCount++;
            // Weapon doesn't look like a real word anymore
            SwitchWeapon(weaponIndex, "Pack");
        }

        
        
    }

    public void MoveToLayer(Transform root, int layer) {
        // If we set our particels to be viewmodel, they get in the way
        // So we have a layer set to never be a viewmodel
        if (root.tag == "NeverViewmodel") return;
        root.gameObject.layer = layer;
        foreach (Transform child in root) {
           MoveToLayer(child, layer);
        }
    }

    void Death() {
        foreach (Weapon w in weapons) {
            if (!w) continue;
            w.Attack(false);
            // Drop last item
            w.transform.parent = null;
            //w.transform.position += cameraBone.transform.right * 2; //Throw to the right
            w.transform.localScale = Vector3.one;
            if (w is Magic) {
                // Set the mesh to be proper scale
                ((Magic)w).mesh.localScale = Vector3.one;
            }
            //w.transform.localEulerAngles += new Vector3(0, 180, 0);
            w.GetComponent<Rigidbody>().isKinematic = false;
            //w.GetComponent<Rigidbody>().AddForce(cameraBone.transform.right * 10);
            w.GetComponent<Collider>().isTrigger = false;
            MoveToLayer(w.transform, 0); //Default layer
            w.setLookObj(null);
            w.setPlayerAnim(null);
            w.setControllerSide("");
        }
        this.weaponCount = 0;
        this.weaponIndex = 0;
        this.weaponBusy = false;
        this.pendingNewWeapon = null;
        this.pendingOldWeapon = null;
        this.pendingPackType = "";
        viewmodelAnimator.SetLayerWeight(1, 0); // right hand
        viewmodelAnimator.SetLayerWeight(2, 0); // left hand
        viewmodelAnimator.SetLayerWeight(3, 0); // movement
        viewmodelAnimator.SetLayerWeight(4, 0); // right hand camera
        viewmodelAnimator.SetLayerWeight(5, 0); // left hand camera
        viewmodelAnimator.SetTrigger("Death");


    }
    void NotDeath() {
        viewmodelAnimator.SetTrigger("NotDeath");
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {
    private static Vector3 EQUIPPED_WEAPON_SCALE = new Vector3(0.008f, 0.008f, 0.008f);
    private static float WEAPON_SWITCH_TIME = .3f;

    public Transform weaponBone;
    public Transform cameraBone;
    public Animator viewmodelAnimator;

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
            } else if (pendingPackType == "Drop" && pendingOldWeapon) { 
                // Throwing
                // Drop last item
                pendingOldWeapon.transform.parent = null;
                pendingOldWeapon.transform.position += cameraBone.transform.right * 2; //Throw to the right
                pendingOldWeapon.transform.localScale = Vector3.one;
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
        viewmodelAnimator.SetFloat(controllerSide + "AttackSpeed", 1 / (2 * pendingNewWeapon.timeToAttack));
        viewmodelAnimator.SetFloat(controllerSide + "CooldownSpeed", 1 / (2 * pendingNewWeapon.timeToCooldown));

        pendingPackType = "";
    }
    /**
     * Called when we pick up a new weapon
     * Should be called by inventory controller
     */
    public void EquipWeapon(Weapon newWeapon) {
        if (newWeapon is Magic) {
            pendingNewWeapon = newWeapon;

            weapons[0] = newWeapon;
            weaponCount = 1;
            // Override for magic, we haven't filled out the animator workflow for this to work yet
            // Set this weapon to be active
            
            viewmodelAnimator.SetInteger(controllerSide + "EquippedWeapon", pendingNewWeapon.animationType);
            pendingNewWeapon = newWeapon;
            pendingNewWeapon.transform.parent = weaponBone;
            //pendingNewWeapon.transform.localScale = Vector3.zero;
            pendingNewWeapon.transform.localPosition = Vector3.zero;
            pendingNewWeapon.transform.localEulerAngles = Vector3.zero;
            pendingNewWeapon.GetComponent<Rigidbody>().isKinematic = true;
            pendingNewWeapon.GetComponent<Collider>().isTrigger = true;
            pendingNewWeapon.setPlayerAnim(viewmodelAnimator);
            pendingNewWeapon.setLookObj(cameraBone);
            pendingNewWeapon.setControllerSide(controllerSide);

            pendingNewWeapon.transform.GetChild(0).gameObject.SetActive(false);

            SwitchWeaponFinished();
            return;
        }
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
        root.gameObject.layer = layer;
        foreach (Transform child in root) {
            MoveToLayer(child, layer);
        }
    }
}
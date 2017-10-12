using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class WeaponController : MonoBehaviour {
    private static Vector3 EQUIPPED_WEAPON_SCALE = new Vector3(0.008f, 0.008f, 0.008f);
    private static float WEAPON_SWITCH_TIME = .3f;
    private static float TIME_UNTIL_MAGIC_DROP = 0.46f;
    private static float TIME_UNTIL_MAGIC_EQUIP = 0.55f;

    public GameObject[] weaponTypes;

    public Transform weaponBone, weaponBone2;
    public Transform cameraBone;
    public Animator viewmodelAnimator;
    public StatsController myStats;
    public WeaponController otherWeapon;

    //public GameObject firstWeaponToEquip; // Prefab reference that we can instantiate

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

    private bool bothHands;
    private bool disableAttacks;

    //GUI bag
    private InventoryBagPanel guiBag;

	// Use this for initialization
	void Start () {

        pendingPackType = "";
        weaponIndex = 0;
        weaponCount = 0;

        viewmodelAnimator.SetBool("DoneWithBoth", true);
        viewmodelAnimator.SetInteger(controllerSide + "EquippedWeapon", 0);
        viewmodelAnimator.SetInteger(controllerSide + "AttackNum", 0);

        /*if (firstWeaponToEquip) {
            GameObject newWeapon = GameObject.Instantiate(firstWeaponToEquip);
            EquipWeapon(newWeapon.GetComponent<Weapon>());
        }*/

        //get Inventory Bag connected to
        UIController uiController = GetComponent<InputGenerator>().uiController;
        InventoryPanel inventoryPanel = uiController.inventoryCanvas.inventoryPanel;

        //viewmodelAnimator.SetInteger("ArrowAmmo", myStats.arrowCount);

        //right: weapon
        //left: magic
        if (controllerSide == "R")
        {
            guiBag = inventoryPanel.weaponPanel;
        }
        else
        {
            guiBag = inventoryPanel.magicPanel;
        }
    }

    // Update is called once per frame
    void Update () {
        // If we are both hands, layerindex is 0
        // Otherwise, L = 2, R = 1
        int layerIndex = (bothHands) ? 0 : ( controllerSide == "R" ? 1 : 2 );
        //print(pendingPackType);
        weaponBusy = !(viewmodelAnimator.GetCurrentAnimatorStateInfo(layerIndex).IsTag("Idle") && !viewmodelAnimator.IsInTransition(layerIndex));

        //if (controllerSide == "R") print(pendingPackType + " " + viewmodelAnimator.GetCurrentAnimatorStateInfo(layerIndex).IsTag("TransferDone"));
        if (pendingNewWeapon is Magic) {
            //print("Pending new weapon: " + ((Magic)pendingNewWeapon).idleParticles.isPlaying + " " + viewmodelAnimator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime);
            if (viewmodelAnimator.GetCurrentAnimatorStateInfo(layerIndex).IsTag("TransferDone") && viewmodelAnimator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime > TIME_UNTIL_MAGIC_EQUIP) {
                // Show new weapon particles

                ((Magic)pendingNewWeapon).playParticles();
                
            }
        }
        if (pendingOldWeapon is Magic) {
            //print("Pending old weapon: " + ((Magic)pendingOldWeapon).idleParticles.isPlaying);
            if (viewmodelAnimator.GetCurrentAnimatorStateInfo(layerIndex).IsTag("MagicDrop") && viewmodelAnimator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime > TIME_UNTIL_MAGIC_DROP) {
                // Hide particles
                ((Magic)pendingOldWeapon).pauseParticles();
                ((Magic)pendingNewWeapon).pauseParticles();
            }
        }
        if (pendingPackType != "" && (!pendingNewWeapon.bothHands && viewmodelAnimator.GetCurrentAnimatorStateInfo(layerIndex).IsTag("TransferDone") || (pendingNewWeapon.bothHands && this.ReadyForBoth() && otherWeapon.ReadyForBoth()))) {
            //print(viewmodelAnimator.GetCurrentAnimatorStateInfo(layerIndex).IsTag("TransferDone"));
            //print(this.ReadyForBoth() && otherWeapon.ReadyForBoth());
            //print("done throwing or whatever");
            // If we are done throwing or packing or whatever
            if (pendingPackType == "Pack" && pendingOldWeapon) { //We make sure the old weapon exists
                // Packing
                // Hide old weapon

                pendingOldWeapon.transform.localScale = Vector3.zero;
                if (pendingOldWeapon is Magic) {
                    // Set the mesh to be scale 0
                    ((Magic)pendingOldWeapon).mesh.localScale = Vector3.zero;
                    ((Magic)pendingOldWeapon).pauseParticles(); // Pause particles before packing

                }
                pendingOldWeapon.gameObject.SetActive(false);
            } else if (pendingPackType == "Drop" && pendingOldWeapon) {
                // Throwing
                // Drop last item
                pendingOldWeapon.gameObject.SetActive(true);
                pendingOldWeapon.transform.parent = null;
                pendingOldWeapon.transform.position += cameraBone.transform.right * 2; //Throw to the right
                pendingOldWeapon.transform.localScale = Vector3.one;
                if (pendingOldWeapon is Magic) {
                    // Set the mesh to be proper scale
                    ((Magic)pendingOldWeapon).mesh.localScale = Vector3.one;

                    ParticleSystem.MainModule shootMain = ((Magic)pendingOldWeapon).shootParticles.main;
                    shootMain.simulationSpace = ParticleSystemSimulationSpace.World;

                    ParticleSystem.MainModule idleMain = ((Magic)pendingOldWeapon).idleParticles.main;
                    idleMain.simulationSpace = ParticleSystemSimulationSpace.World;
                }
                pendingOldWeapon.transform.localEulerAngles += new Vector3(0, 180, 0);
                pendingOldWeapon.GetComponent<Rigidbody>().isKinematic = false;
                pendingOldWeapon.GetComponent<Rigidbody>().AddForce(cameraBone.transform.right * 10);
                pendingOldWeapon.GetComponent<Collider>().enabled = true;
                MoveToLayer(pendingOldWeapon.transform, 10); // Ignore Mobs layer (so we don't collide again)
                pendingOldWeapon.setLookObj(null);
                pendingOldWeapon.setPlayerAnim(null);
                pendingOldWeapon.setControllerSide("");
                if (!pendingOldWeapon.GetComponent<DestroyOnLevelLoad>()) pendingOldWeapon.gameObject.AddComponent<DestroyOnLevelLoad>();


                //print(pendingOldWeapon.bothHands);





            }
            if (pendingOldWeapon && pendingOldWeapon.bothHands) {
                // Put arrow back
                Transform arrow = weaponBone2.Find("Arrow");
                arrow.parent = pendingOldWeapon.transform;
                arrow.localScale = Vector3.zero;
                MoveToLayer(arrow, 1); //Regular layer for camera rendering

                //print("Old weapon is both hands");
                if (!pendingNewWeapon.bothHands) {
                    this.otherWeapon.BothHandsDone();
                    BothHandsDone();
                }
            }
            if (pendingNewWeapon.bothHands) {
                //print("Old weapon is not both hands");
                //print(pendingNewWeapon);
                Transform arrow = pendingNewWeapon.transform.Find("Arrow");
                arrow.parent = weaponBone2;
                arrow.localScale = EQUIPPED_WEAPON_SCALE;
                arrow.localPosition = Vector3.zero;
                arrow.localEulerAngles = new Vector3(-90, 0, 0);
                MoveToLayer(arrow, 8); //Viewmodel layer for camera rendering

            }

            if (pendingNewWeapon.bothHands) {
               // print("New weapon is both hands");
                viewmodelAnimator.SetInteger(controllerSide + "EquippedWeapon", pendingNewWeapon.animationType);
                viewmodelAnimator.SetTrigger("BothHandsStart");
                this.otherWeapon.BothHandsForceStop();
                this.BothHandsForceStop();
            } else {
               // print("New weapon is not both hands");
               // this.otherWeapon.BothHandsDone();
                //BothHandsDone();
            }
            //print("Adding new item " + pendingNewWeapon);
            // Set this weapon to be active
            pendingNewWeapon.transform.parent = weaponBone;
            pendingNewWeapon.gameObject.SetActive(true);

            pendingNewWeapon.transform.localScale = EQUIPPED_WEAPON_SCALE;
            pendingNewWeapon.playerStats = myStats;
            if (pendingNewWeapon is Magic) {

                // Set the mesh to be scale 0
                ((Magic)pendingNewWeapon).mesh.localScale = Vector3.zero;
                ((Magic)pendingNewWeapon).pauseParticles(); // pause particles before equipping
            } 
            if (pendingNewWeapon.storedAmmo > 0) {
                if (pendingNewWeapon is Magic) {
                    myStats.UpdateMagic(pendingNewWeapon.storedAmmo);
                    pendingNewWeapon.storedAmmo = 0;
                } else if (pendingNewWeapon is ProjectileWeapon) {
                    myStats.UpdateArrows((int)pendingNewWeapon.storedAmmo);
                    pendingNewWeapon.storedAmmo = 0;
                    viewmodelAnimator.SetInteger("ArrowAmmo", myStats.arrowCount);
                }
            }
            pendingNewWeapon.transform.localPosition = Vector3.zero;
            pendingNewWeapon.transform.localEulerAngles = Vector3.zero;
            pendingNewWeapon.GetComponent<Rigidbody>().isKinematic = true;
            pendingNewWeapon.GetComponent<Collider>().enabled = false;
            MoveToLayer(pendingNewWeapon.transform, 8); //Viewmodel layer for camera rendering
            pendingNewWeapon.setPlayerAnim(viewmodelAnimator);
            pendingNewWeapon.setLookObj(cameraBone);
            pendingNewWeapon.setControllerSide(controllerSide);
            if (pendingNewWeapon.GetComponent<DestroyOnLevelLoad>()) Destroy(pendingNewWeapon.gameObject.GetComponent<DestroyOnLevelLoad>());


            // Tell it to set animator information
            SwitchWeaponFinished();

            
        }
    }

    public void Attack(bool mouseDown) {
        // We have a current weapon
        // And the weapon is not busy
        if (weaponCount > 0 && !disableAttacks) {
            
            if (weaponBusy && controllerSide == "R") {
                if (weapons[weaponIndex] is ChargedProjectileWeapon) {
                    // Some logic since we have to hold down the trigger
                    if (!((ChargedProjectileWeapon)weapons[weaponIndex]).isAttacking) {
                        mouseDown = false;
                    }
                } else {
                    mouseDown = false;
                }
            }
            if (weapons[weaponIndex] is ProjectileWeapon) {
                // ammo
                
            }
            // Attack
            weapons[weaponIndex].Attack(mouseDown);
        }
    }
    public void clearSwitchCooldown() {
        // Called when switching scenes
        switchCooldown = 0;
    }
    public void SwitchWeapon() {
        if (Time.timeSinceLevelLoad - switchCooldown < WEAPON_SWITCH_TIME 
            || weaponCount <= 1
            || pendingPackType != "") {
            //print("falling out 3");
            return;
        }
        switchCooldown = Time.timeSinceLevelLoad;
        if (weapons[weaponIndex] is ProjectileWeapon) {
            weapons[weaponIndex].Attack(false);
        }
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
        //print("pending pack:" + pendingPackType + " " + weaponCount);
        if (weaponCount == 0
            || pendingPackType != "") {
            print("falling out 2");
            return;
        }
        //print("foo");
        int layerIndex = (bothHands) ? 0 : (controllerSide == "R" ? 1 : 2);
        if (!viewmodelAnimator.GetCurrentAnimatorStateInfo(layerIndex).IsTag("Idle")) return;
        //print("Switching, weapon index: " + newIndex + " weapon count: " + weaponCount);

        weaponIndex = newIndex;
        //We pack an item if switching from one to another, throw if we are dropping our current weapon
        //print(bothHands);

        pendingPackType = packType;

        if (pendingNewWeapon is Magic) {
            ((Magic)pendingNewWeapon).pauseParticles();
            // Set local simulation space
            ParticleSystem.MainModule shootMain = ((Magic)pendingNewWeapon).shootParticles.main;
            shootMain.simulationSpace = ParticleSystemSimulationSpace.Custom;
            shootMain.customSimulationSpace = transform.Find("viewmodelHolder/player_viewmodel");

            ParticleSystem.MainModule idleMain = ((Magic)pendingNewWeapon).idleParticles.main;
            idleMain.simulationSpace = ParticleSystemSimulationSpace.Custom;
            idleMain.customSimulationSpace = transform.Find("viewmodelHolder/player_viewmodel");

        }

            // If we were already both hands
            if (bothHands) {
            viewmodelAnimator.SetTrigger("B" + packType);
        } else if (!pendingNewWeapon.bothHands) {
            // Glitch where it sets pack when new weapon is both hands
            //print("foooo " + controllerSide + packType);
            viewmodelAnimator.SetTrigger(controllerSide + packType);
        }
       
        

        // If a two handed weapon, we need to tell the other weapon controllers to stop 
        if (pendingNewWeapon.bothHands) {
            bothHands = true;
            BothHandsRequestStop();
            otherWeapon.BothHandsRequestStop();
        } else {
            // If we are not switching to both hands, we tell the animation type. If we are both hands, we do it later.
            viewmodelAnimator.SetInteger(controllerSide + "EquippedWeapon", pendingNewWeapon.animationType);
        }
        
    }
    public void SwitchWeaponFinished() {
        //print("SwitchWeaponFinished");
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
            print("falling out 1");
            return;
        }

        pendingNewWeapon = newWeapon;

        

        //print("Trying to equip");
        if (weaponCount >= weapons.Length) {
            // If we already have plenty of weapons
            // We drop one and equip the other

            //update GUI
            guiBag.Drop(weapons[weaponIndex]);

            //update models, db
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
    /**
     * Equipping a weapon without the fancy aniamtions and such, assumed to be not the currently shown weapon
     */
    public void EquipWeaponInstantly(Weapon weaponToEquip, int index) {
        weaponCount++;
        weapons[index] = weaponToEquip;
        weaponToEquip.transform.localScale = Vector3.zero;
        if (weaponToEquip is Magic) {
            // Set the mesh to be scale 0
            ((Magic)weaponToEquip).mesh.localScale = Vector3.zero;
            ((Magic)weaponToEquip).pauseParticles(); // Pause particles before packing

        }
        weaponToEquip.gameObject.SetActive(false);
    }
    void BothHandsRequestStop() {
        foreach (Weapon w in weapons) {
            if (!w) continue;
            w.Attack(false);
        }
        viewmodelAnimator.SetBool("DoneWithBoth", false);
        if (weapons[weaponIndex] is Magic) {
            disableAttacks = true;

            // Stop the particle effect
            ((Magic)weapons[weaponIndex]).pauseParticles();

        }

    }
    void BothHandsForceStop() {
        
        this.viewmodelAnimator.SetLayerWeight(1, 0);
        this.viewmodelAnimator.SetLayerWeight(2, 0);
    }
    bool ReadyForBoth() {
        int layerIndex = controllerSide == "R" ? 1 : 2;
        return viewmodelAnimator.GetCurrentAnimatorStateInfo(layerIndex).IsTag("WaitUntilBothDone") && !viewmodelAnimator.IsInTransition(layerIndex); 
        //If we are locked and loaded, ready to move from two different hand animations to one
    }
    void BothHandsDone() {
        bothHands = false;
        this.viewmodelAnimator.SetLayerWeight(1, 1);
        this.viewmodelAnimator.SetLayerWeight(2, 1);
        viewmodelAnimator.SetBool("DoneWithBoth", true);
        if (weapons[weaponIndex] is Magic) {
            disableAttacks = false;
            this.viewmodelAnimator.ResetTrigger("LPack"); // So if we are just spawning and have a two handed weapon, it won't cause a ruckus

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
        bothHands = true;
        foreach (Weapon w in weapons) {
            if (!w) continue;
            w.Attack(false);
            // Drop last item
            w.transform.parent = null;
            //w.transform.position += cameraBone.transform.right * 2; //Throw to the right
            w.transform.localScale = Vector3.one;
            w.gameObject.SetActive(true);
            if (w is Magic) {
                // Set the mesh to be proper scale
                ((Magic)w).mesh.localScale = Vector3.one;
            }
            //w.transform.localEulerAngles += new Vector3(0, 180, 0);
            w.GetComponent<Rigidbody>().isKinematic = false;
            //w.GetComponent<Rigidbody>().AddForce(cameraBone.transform.right * 10);
            w.GetComponent<Collider>().enabled = true;
            MoveToLayer(w.transform, 10); // Ignore Mobs layer (so we don't collide again)
            w.setLookObj(null);
            w.setPlayerAnim(null);
            w.setControllerSide("");
            if (!w.GetComponent<DestroyOnLevelLoad>()) w.gameObject.AddComponent<DestroyOnLevelLoad>();
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
        bothHands = false;
        viewmodelAnimator.SetTrigger("NotDeath");
    }

}
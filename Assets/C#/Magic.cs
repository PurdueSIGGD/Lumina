using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : Weapon {
	float holdTime;
	float releaseTime; //Time since mouse was released.
	//float magicUsage;
	bool attacking;
	bool onCooldown;
	//TODO: StatsController sC;

	public void Start() {
		holdTime = 0F;
		releaseTime = 0F;
		attacking = false;
		//magicUsage = 1F; //The amount of mana used per frame of attacking
		onCooldown = false;
	}

	public override void Attack(bool mouseDown) {
        if (attacking) {
            if (!mouseDown) { //TODO: || sC.getMagic < magicUsage
                attacking = false;
                releaseTime = 0;
                onCooldown = true;
                getPlayerAnim().SetBool(getControllerSide() + "MagicAttack", false); //TODO: Check for accuracy
            } else {
                holdTime += Time.deltaTime;
                if (holdTime > timeToAttack) {
                    //TODO: sC.UpdateMagic(magicUsage)
                    //print("Shoooooot");
                    RaycastHit[] hits = Physics.RaycastAll(getLookObj().transform.position, getLookObj().transform.forward);
                    foreach (RaycastHit hit in hits) {
                        if (hit.distance <= range && hit.collider.gameObject.tag != "Player") {
                            // Push physics, regardless of hittable
                            Rigidbody r;
                            if (r = hit.collider.GetComponent<Rigidbody>()) {
                                // Play around with a good factor here
                                r.AddForceAtPosition(getLookObj().forward * 300 * Time.deltaTime, getLookObj().position);
                            }
                            // Hit with hittable
                            Hittable hittable = hit.collider.GetComponentInParent<Hittable>();
                            if (hittable != null) {
                                hittable.Hit(damage, getLookObj().transform.forward, damageType);
                            }
                        }
                    }
                }
            }
        } else if (mouseDown) {
            if (onCooldown) {
                if (releaseTime + Time.deltaTime >= timeToCooldown) {
                    onCooldown = false;
                } else {
                    releaseTime += Time.deltaTime;
                }
            } else {
                getPlayerAnim().SetBool(getControllerSide() + "MagicAttack", true);
                attacking = true;
                holdTime = 0;
            }
        } else {
            releaseTime += Time.deltaTime;
        }

	}

	
}

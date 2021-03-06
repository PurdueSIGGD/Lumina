﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamMagic : Magic {
    private static int layerIndex = 2;
    public float width = 1; // Width of the beam
	float holdTime;
	float releaseTime; //Time since mouse was released.
    
	//float magicUsage;
	bool attacking;
	bool onCooldown;
    

    public float magicDraw = 1; //Magic per second this attack takes

    public override string getBlurb() {
		return "Damage: " + System.Math.Round((baseDamage * condition/maxCondition), 2) + "/s, Cost: " + magicDraw + "/s";
    }

	public void Start() {
        

        holdTime = 0F;
		releaseTime = 0F;
		attacking = false;
		//magicUsage = 1F; //The amount of mana used per frame of attacking
		onCooldown = false;
        idleParticles.Play();

    }
    public void Update() {
        if (!getPlayerAnim() && !idleParticles.isPlaying) {
            // If not being used, make sure to play when idle
            idleParticles.Play();
        }
    }

    public override void MagicAttack(bool mouseDown) {
        // Particle controls
        if (attacking && mouseDown && holdTime > timeToAttack) {
            if (!shootParticles.isPlaying) {
                shootParticles.Play();
            }
            if (idleParticles.isPlaying) {
                idleParticles.Stop();
            }
		} else if ((!mouseDown || !playerStats || playerStats.GetMagic() <= 0) && (!getPlayerAnim() || !getPlayerAnim().GetCurrentAnimatorStateInfo(layerIndex).IsTag("TransferDone"))) { // Transfer done layer, because we don't want it playing when transitioning
            if (shootParticles.isPlaying) {
                shootParticles.Stop();
            }
            if (!idleParticles.isPlaying) {
                idleParticles.Play();
            }
        }

        if (getPlayerAnim() && getPlayerAnim().GetCurrentAnimatorStateInfo(2).IsTag("Idle") && !getPlayerAnim().IsInTransition(2)) {
            // Overrides
            releaseTime = 0;
            onCooldown = false;
            attacking = false;
        }
        if (attacking) {
            if (!mouseDown || playerStats.GetMagic() <= 0) {
                attacking = false;
                releaseTime = 0;
                onCooldown = true;
                getPlayerAnim().SetBool(getControllerSide() + "MagicAttack", false); //TODO: Check for accuracy
                
            } else {
                holdTime += Time.deltaTime;
                if (holdTime > timeToAttack) {
                    if (!shootParticles.isPlaying) { 
                        shootParticles.Play();
                    }
                    if (idleParticles.isPlaying) { 
                        idleParticles.Stop();
                    }
                    playerStats.UpdateMagic(-1 * magicDraw * Time.deltaTime);
                    //print("Shoooooot");
                    RaycastHit[] hits = Physics.CapsuleCastAll(getLookObj().transform.position, getLookObj().transform.position + getLookObj().transform.forward * range, width, getLookObj().transform.forward);
                    foreach (RaycastHit hit in hits) {
                        if (hit.distance <= range && 
                            hit.collider.gameObject.tag != "Player" &&
                            !hit.collider.isTrigger ) {
                            //Debug.Log(hit.transform.gameObject);
                            // Push physics, regardless of hittable
                            Rigidbody r;
                            if (r = hit.collider.GetComponent<Rigidbody>()) {
                                // Play around with a good factor here
                                r.AddForceAtPosition(getLookObj().forward * 300 * Time.deltaTime, getLookObj().position);
                            }
                            // Hit with hittable
                            Hittable hittable = hit.collider.GetComponentInParent<Hittable>();
                            if (hittable != null && hittable.gameObject.tag != "Item" && hittable.gameObject.tag != "Player") { //Sometimes may hit our item that we are holding
                                //print(hit.collider);
								hittable.Hit(baseDamage * condition/maxCondition, getLookObj().transform.forward, damageType);
                            }
                        }
                    }

                }
            }
		} else if (mouseDown && playerStats.GetMagic() > 0) {
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : Weapon {
	float holdTime;
	float releaseTime; //Time since mouse was released.
	//float magicUsage;
	bool attacking;
	bool onCooldown;
	Camera thecamera;
	//TODO: StatsController sC;

	public void Start() {
		holdTime = 0F;
		releaseTime = 0F;
		attacking = false;
		//magicUsage = 1F; //The amount of mana used per frame of attacking
		onCooldown = false;
		thecamera = GetComponentInParent<Camera> ();
	}

	public override void Attack(float deltaTime, bool mouseDown) {
		if (attacking) {
			if (!mouseDown) { //TODO: || sC.getMagic < magicUsage
				attacking = false;
				releaseTime = 0;
				onCooldown = true;
				anim.SetBool ("active", false); //TODO: Check for accuracy
			} else {
				holdTime += deltaTime;
				//TODO: sC.UpdateMagic(magicUsage)
				RaycastHit[] hits = Physics.RaycastAll(thecamera.transform.position, thecamera.transform.forward);
				foreach (RaycastHit hit in hits) {
					Hittable hittable = hit.collider.GetComponentInParent<Hittable> ();
					if (hittable != null && hit.distance <= range) {
						hittable.Hit (damage, thecamera.transform.forward, damageType);
					}
				}
			}
		} else if (mouseDown) {
			if (onCooldown) {
				if (releaseTime + deltaTime >= cooldownLength) {
					onCooldown = false;
				} else {
					releaseTime += deltaTime;
				}
			} else {
				attacking = true;
				holdTime = 0;
				anim.SetBool ("active", true); //TODO: Check for accuracy
			}
		}
	}
}

﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingWeapon : Weapon {

	bool isAttacking;
	bool hasRaycasted;

	// Use this for initialization
	void Start () {
		isAttacking = false;
		hasRaycasted = false;
	}

    public override void Attack(bool mouseDown)
    {
		if (isAttacking) {
			setTimeSincePress(getTimeSincePress() + Time.deltaTime);
			if (!hasRaycasted && getTimeSincePress() >= timeToAttack) {
                //print("Hitting now " + getLookObj());
                // Apply ItemStats damage
                this.DamageCondition(1);
                RaycastHit[] hits = Physics.RaycastAll(getLookObj().transform.position, getLookObj().transform.forward);
				foreach (RaycastHit hit in hits) {
                    if (hit.distance <= range &&
                        !hit.collider.isTrigger &&
                        hit.collider.gameObject.tag != "Player") {
                        // Push physics, regardless of hittable
                        Rigidbody r;
                        if (r = hit.collider.GetComponent<Rigidbody>()) {
                            //print("Adding force");
                            // Play around with a good factor here
                            r.AddForceAtPosition(getLookObj().forward * 100, getLookObj().position);
                        }
                        // Hit with hittable
                        Hittable hittable = hit.collider.GetComponentInParent<Hittable>();
                        if (hittable != null) {
                            hittable.Hit(baseDamage * (getCondition()/100), getLookObj().transform.forward, damageType);
                        }
                    }
                    
				}
				hasRaycasted = true;
			} else if (hasRaycasted && getTimeSincePress() > timeToAttack + timeToCooldown) {
				isAttacking = false;
				hasRaycasted = false;
                setTimeSincePress(0);
			}
		} else if (mouseDown && !isAttacking) {
            isAttacking = true;
			getPlayerAnim().SetTrigger(getControllerSide() + "Attack");
            getPlayerAnim().SetInteger(getControllerSide() + "AttackNum", UnityEngine.Random.Range(0, 2));
        }
    }
}

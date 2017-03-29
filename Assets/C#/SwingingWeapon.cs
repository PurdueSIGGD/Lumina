using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingWeapon : Weapon {

	public Camera thecamera;
	bool isAttacking;
	bool hasRaycasted;

	// Use this for initialization
	void Start () {
		isAttacking = false;
		hasRaycasted = false;
		thecamera = GetComponentInParent<Camera> ();

	}

    public override void Attack(float deltaTime, bool mouseDown)
    {
		if (isAttacking) {
			timeSincePress += deltaTime;
			if (!hasRaycasted && timeSincePress >= timeToAttack) {
				RaycastHit[] hits = Physics.RaycastAll(thecamera.transform.position, thecamera.transform.forward);
				foreach (RaycastHit hit in hits) {
					Hittable hittable = hit.collider.GetComponentInParent<Hittable> ();
					if (hittable != null && hit.distance <= range) {
						hittable.Hit (damage, thecamera.transform.forward, damageType);
					}
				}
				hasRaycasted = true;
			} else if (hasRaycasted && timeSincePress > timeToAttack + cooldownLength) {
				isAttacking = false;
				hasRaycasted = false;
				timeSincePress = 0;
			}
		} else if (mouseDown) {
			isAttacking = true;
		}
    }
}

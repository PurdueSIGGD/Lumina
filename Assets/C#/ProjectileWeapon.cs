using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon {

	bool isShooting;

	public Projectile projectilePrefab;
	public float launchSpeed;

    public override void Attack(float deltaTime, bool mouseDown)
    {
		if (isShooting) {
			timeSincePress += deltaTime;
			if (timeSincePress >= timeToAttack) {
				//Spawn Projectile
				Projectile projectile = Instantiate<Projectile>(projectilePrefab, lookObj.transform.position, lookObj.transform.rotation);
				projectile.damage = damage;
				projectile.damageType = damageType;
				projectile.GetComponent<Rigidbody> ().velocity = lookObj.transform.forward * launchSpeed;
			} else if (timeSincePress > timeToAttack + cooldownLength) {
				isShooting = false;
				timeSincePress = 0;
			}
		} else if (mouseDown) {
			isShooting = true;
			anim.SetTrigger ("shoot"); //TODO: Make sure this matches up later
		}
	}

	public void Start() {
		isShooting = false;
	}
}

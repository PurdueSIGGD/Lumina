using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon {

	bool isShooting;

	public Projectile projectilePrefab;
	public float launchSpeed;

    public override void Attack(bool mouseDown)
    {
		if (isShooting) {
            setTimeSincePress(getTimeSincePress() + Time.deltaTime);
            if (getTimeSincePress() >= timeToAttack) {
				//Spawn Projectile
				Projectile projectile = Instantiate<Projectile>(projectilePrefab, getLookObj().transform.position, getLookObj().transform.rotation);
				projectile.damage = baseDamage;
				projectile.damageType = damageType;
				projectile.GetComponent<Rigidbody> ().velocity = getLookObj().transform.forward * launchSpeed;
			} else if (getTimeSincePress() > timeToAttack + timeToCooldown) {
				isShooting = false;
                setTimeSincePress(0);
			}
		} else if (mouseDown) {
			isShooting = true;
            getPlayerAnim().SetTrigger (getControllerSide() + "Attack"); //TODO: Make sure this matches up later
		}
	}

	public void Start() {
		isShooting = false;
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon {

	bool isShooting;

	Camera thecamera;
	public Projectile projectilePrefab;
	public float launchSpeed;

    public override void Attack(float deltaTime, bool mouseDown)
    {
		if (isShooting) {
			timeSincePress += deltaTime;
			if (timeSincePress >= timeToAttack) {
				//Spawn Projectile
				Projectile projectile = Instantiate<Projectile>(projectilePrefab, thecamera.transform.position, thecamera.transform.rotation);
				projectile.damage = damage;
				projectile.damageType = damageType;
				projectile.GetComponent<Rigidbody> ().velocity = thecamera.transform.forward * launchSpeed;
			} else if (timeSincePress > timeToAttack + cooldownLength) {
				isShooting = false;
				timeSincePress = 0;
			}
		} else if (mouseDown) {
			isShooting = true;
			anim.SetTrigger ("shoot"); //TODO: Make sure this matches up later
		}
	}

    // Use this for initialization
    void Start ()
	{
		thecamera = GetComponentInParent<Camera> ();
	}
}

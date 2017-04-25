using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This is a class that we override for any weapon that spawns a projectile
 * So all your new weapon class would have to do is handle the calling of SpawnProjectile(). You can have it rapid fire if thats all your weapon does
 */ 

public abstract class ProjectileWeapon : Weapon {

    public Projectile projectilePrefab;
    public Transform shootPoint; //Transform where you want the arrow to spawn and shoot from
    public float launchSpeed = 1;

   void Update() {
        // We have to update arrows constantly because it may change
        if (getPlayerAnim()) getPlayerAnim().SetInteger("ArrowAmmo", playerStats.arrowCount);
    }
    /**
     * Spawns the projectile you prefer
     * Note: Also damages condition
     * 
     */
    public void SpawnProjectile() {
        // Apply ItemStats damage
        this.DamageCondition(1);
        //Spawn Projectile
        Vector3 newRotation = getLookObj().eulerAngles;
        //newRotation += new Vector3(90, 0, 0);
        Projectile projectile = Instantiate<Projectile>(projectilePrefab, shootPoint.transform.position, Quaternion.Euler(newRotation));

        //projectile.transform.LookAt(((getLookObj().transform.position = getLookObj().transform.forward) - getLookObj().transform.position));
        projectile.creator = playerStats.transform;
        projectile.damage = (baseDamage * (getCondition() / 100));
        projectile.damageType = damageType;
        projectile.GetComponent<Rigidbody>().velocity = getLookObj().transform.forward * launchSpeed * getTimeSincePress();
        playerStats.UpdateArrows(-1);

        setTimeSincePress(0);
    }
}
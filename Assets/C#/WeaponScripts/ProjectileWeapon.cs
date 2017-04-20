using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon {
    public float width = .03f;

    public Projectile projectilePrefab;
    public float launchSpeed = 1;
    bool isAttacking;
    bool hasRaycasted;

    // Use this for initialization
    void Start() {
        isAttacking = false;
        hasRaycasted = false;
    }

    public override void Attack(bool mouseDown) {
        if (isAttacking) {
            setTimeSincePress(getTimeSincePress() + Time.deltaTime);
            if (!hasRaycasted && getTimeSincePress() >= timeToAttack) {
                //print("Hitting now " + getLookObj());
                // Apply ItemStats damage
                this.DamageCondition(1);
                //Spawn Projectile
                Projectile projectile = Instantiate<Projectile>(projectilePrefab, getLookObj().transform.position, getLookObj().transform.rotation);
                projectile.damage = baseDamage;
                projectile.damageType = damageType;
                projectile.GetComponent<Rigidbody>().velocity = getLookObj().transform.forward * launchSpeed;
                hasRaycasted = true;
            } else if (hasRaycasted && getTimeSincePress() > timeToAttack + timeToCooldown) {
                isAttacking = false;
                hasRaycasted = false;
                setTimeSincePress(0);
            }
        } else if (mouseDown && !isAttacking) {
            isAttacking = true;
            getPlayerAnim().SetTrigger(getControllerSide() + "Attack");
            //getPlayerAnim().SetInteger(getControllerSide() + "AttackNum", UnityEngine.Random.Range(0, 2));
        }
    }
}
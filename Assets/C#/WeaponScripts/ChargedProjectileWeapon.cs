using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedProjectileWeapon : ProjectileWeapon {

    public bool isAttacking;
    public bool hasShot;
    static float maxChargeTime = 1; //The max amount of seconds we charge for
    // Use this for initialization
    void Start() {
        isAttacking = false;
        hasShot = false;
    }

    public override void Attack(bool mouseDown) {
        //print("attac");
        //print(mouseDown + " " + isAttacking);
        if (mouseDown && isAttacking) {
            //print(getTimeSincePress());
            if (getTimeSincePress() < maxChargeTime)
            {
                setTimeSincePress(getTimeSincePress() + Time.deltaTime);
            } else if (getTimeSincePress() > maxChargeTime)
            {
                setTimeSincePress(maxChargeTime);
            }

        } else if (mouseDown && !isAttacking && playerStats.arrowCount > 0) {
            isAttacking = true;
            hasShot = false;
            getPlayerAnim().SetBool("RBowHold", true);
            myAnim.SetBool("Pulling", true);
            //getPlayerAnim().SetInteger(getControllerSide() + "AttackNum", UnityEngine.Random.Range(0, 2));
        } else if (!mouseDown && isAttacking && !hasShot) {
            hasShot = true;
            isAttacking = false;
            myAnim.SetBool("Pulling", false);
            getPlayerAnim().SetBool("RBowHold", false);
            // Actually shoot
            this.SpawnProjectile();

        }
    }
}
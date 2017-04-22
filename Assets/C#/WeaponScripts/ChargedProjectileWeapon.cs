using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedProjectileWeapon : Weapon {
    public float width = .03f;

    public Projectile projectilePrefab;
    public Transform shootPoint; //Transform where you want the arrow to spawn and shoot from
    public float launchSpeed = 1;
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

        } else if (mouseDown && !isAttacking) {
            isAttacking = true;
            hasShot = false;
            getPlayerAnim().SetBool("RBowHold", true);
            myAnim.SetBool("Pulling", true);
            //getPlayerAnim().SetInteger(getControllerSide() + "AttackNum", UnityEngine.Random.Range(0, 2));
        } else if (!mouseDown && isAttacking && !hasShot)
        {
            //print("bowholdfalse");
            hasShot = true;
            myAnim.SetBool("Pulling", false);
            getPlayerAnim().SetBool("RBowHold", false);

            isAttacking = false;
            //print("Hitting now " + getLookObj());
            // Apply ItemStats damage
            this.DamageCondition(1);
            //Spawn Projectile
            Vector3 newRotation = getLookObj().eulerAngles;
            //newRotation += new Vector3(90, 0, 0);
            Projectile projectile = Instantiate<Projectile>(projectilePrefab, shootPoint.transform.position, Quaternion.Euler(newRotation));

            //projectile.transform.LookAt(((getLookObj().transform.position = getLookObj().transform.forward) - getLookObj().transform.position));
            projectile.damage = baseDamage;
            projectile.damageType = damageType;
            projectile.GetComponent<Rigidbody>().velocity = getLookObj().transform.forward * launchSpeed * getTimeSincePress();
            hasShot = true;


            setTimeSincePress(0);

        }
    }
}
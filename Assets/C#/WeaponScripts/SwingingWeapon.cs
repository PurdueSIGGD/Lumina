using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SwingingWeapon : Weapon {
    public float width = .03f;


    bool isAttacking;
	bool hasRaycasted;

    public GameObject hitParticles; // Prefab reference for particles to spawn

	// Use this for initialization
	void Start () {
		isAttacking = false;
		hasRaycasted = false;
	}
    public override string getBlurb() {
        return "Damage: " + System.Math.Round(baseDamage * (getCondition() / 100), 1) + ", Speed: " + (timeToAttack + timeToCooldown) + ", Range: " + range;
    }
    RaycastHit[] getHitObjects() {
        return Physics.CapsuleCastAll(getLookObj().transform.position, getLookObj().transform.position + getLookObj().transform.forward * range, width, getLookObj().transform.forward);
    }

    public override void Attack(bool mouseDown)
    {
		if (isAttacking) {
			setTimeSincePress(getTimeSincePress() + Time.deltaTime);
			if (!hasRaycasted && getTimeSincePress() >= timeToAttack) {
                //print("Hitting now " + getLookObj());
                // Apply ItemStats damage
                RaycastHit[] hits = getHitObjects();
                
                processHits(hits);
                RaycastHit[] hitsAgain = getHitObjects(); //we do it again, in case some gibs/other stuff spawns that frame
                IEnumerable<RaycastHit> diff = hitsAgain.Except(hits);
                processHits(diff.ToArray<RaycastHit>());
				hasRaycasted = true;
			} else if (hasRaycasted && getTimeSincePress() > timeToAttack + timeToCooldown) {
				isAttacking = false;
				hasRaycasted = false;
                setTimeSincePress(0);
			}
		} else if (mouseDown && !isAttacking) {
            isAttacking = true;
			getPlayerAnim().SetTrigger(getControllerSide() + "Attack");
            int maxRange = 0;
            if (animationType == 1) {
                maxRange = 3;
            } else if (animationType == 2) {
                maxRange = 2;
            }
            getPlayerAnim().SetInteger(getControllerSide() + "AttackNum", UnityEngine.Random.Range(0, maxRange));
        }
    }

    private void processHits(RaycastHit[] hits) {
        bool damageCondition = false;
        Vector3 firstHit = Vector3.zero;
        foreach (RaycastHit hit in hits) {
            if (hit.distance <= range &&
                !hit.collider.isTrigger &&
                hit.collider.gameObject.tag != "Player") {
                RaycastHit modifiableHit = hit;
                
                if (hit.point == Vector3.zero) {
                    // The capsule cast sets it at zero for the initial hit. So we're gonna spherecast and find the correct point
                    RaycastHit[] hitsAgain = Physics.RaycastAll(new Ray(getLookObj().position, getLookObj().forward * range));
                    foreach (RaycastHit hitAgain in hitsAgain) {
                        if (hitAgain.transform.GetComponent<Collider>() == hit.transform.GetComponent<Collider>()) {
                            // We found our hit
                            modifiableHit = hitAgain;
                            break;
                        }
                    }
                }
                if (firstHit == Vector3.zero) {
                    firstHit = modifiableHit.point;
                }
                // Push physics, regardless of hittable
                Rigidbody r;
                if (r = modifiableHit.collider.GetComponent<Rigidbody>()) {
                    // Play around with a good factor here

                    firstHit = modifiableHit.point;
                    r.AddForceAtPosition(baseDamage * getLookObj().forward * 10, getLookObj().position);
                    r.AddForce(Vector3.up * r.mass * 350);
                }
                // Hit with hittable
                Hittable hittable = modifiableHit.collider.GetComponentInParent<Hittable>();
                if (hittable != null) {
                    //print("hit " + hit);
                    firstHit = modifiableHit.point;
                    damageCondition = true;
                    hittable.Hit(baseDamage * (getCondition() / 100), getLookObj().transform.forward, damageType);
                }
            }
        }
        if (firstHit != Vector3.zero) {
            // Spawn particles
            Debug.DrawLine(getLookObj().position, firstHit, Color.green, 10);
            GameObject particles = GameObject.Instantiate(hitParticles, firstHit + (getLookObj().transform.position - firstHit) * 0.3f, Quaternion.Euler(1 * (getLookObj().transform.position - firstHit)));
            particles.transform.LookAt(getLookObj());
        }
        if (damageCondition) {
            this.DamageCondition(1);
        }
    }
}

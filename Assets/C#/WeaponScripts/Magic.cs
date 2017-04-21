using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : Weapon {
    public ParticleSystem idleParticles;
    public ParticleSystem shootParticles;
    public Transform mesh;
    public float width = 1; // Width of the beam
	float holdTime;
	float releaseTime; //Time since mouse was released.
    
	//float magicUsage;
	bool attacking;
	bool onCooldown;

    public StatsController statsController;

    float magicDraw = 1; //Magic per second this attack takes

	public void Start() {
        //idleParticles = this.GetComponent<ParticleSystem>();
        //shootParticles = this.GetComponentInChildren<ParticleSystem>();
        //print(shootParticles.isPlaying);
        //print(idleParticles.isPlaying);
        //shootParticles.Play();
        idleParticles.Play();
        

        holdTime = 0F;
		releaseTime = 0F;
		attacking = false;
		//magicUsage = 1F; //The amount of mana used per frame of attacking
		onCooldown = false;
	}

	public override void Attack(bool mouseDown) {
        // Particle controls
        if (attacking && mouseDown && holdTime > timeToAttack) {
            if (!shootParticles.isPlaying) {
                shootParticles.Play();
            }
            if (idleParticles.isPlaying) {
                idleParticles.Stop();
            }
		} else if (!mouseDown || statsController.GetMagic() <= 0) {
            if (shootParticles.isPlaying) {
                shootParticles.Stop();
            }
            if (!idleParticles.isPlaying) {
                idleParticles.Play();
            }
        }

        if (getPlayerAnim() && getPlayerAnim().GetCurrentAnimatorStateInfo(2).IsTag("Idle") && !getPlayerAnim().IsInTransition(2)) {
            // Overrides
            releaseTime = 0;
            onCooldown = false;
            attacking = false;
        }
        if (attacking) {
			if (!mouseDown || statsController.GetMagic() <= 0) {
                attacking = false;
                releaseTime = 0;
                onCooldown = true;
                getPlayerAnim().SetBool(getControllerSide() + "MagicAttack", false); //TODO: Check for accuracy
                
            } else {
                holdTime += Time.deltaTime;
                if (holdTime > timeToAttack) {
                    if (!shootParticles.isPlaying) { 
                        shootParticles.Play();
                    }
                    if (idleParticles.isPlaying) { 
                        idleParticles.Stop();
                    }
                    statsController.UpdateMagic(-1 * magicDraw * Time.deltaTime);
                    //print("Shoooooot");
                    RaycastHit[] hits = Physics.CapsuleCastAll(getLookObj().transform.position, getLookObj().transform.position + getLookObj().transform.forward * range, width, getLookObj().transform.forward);
                    foreach (RaycastHit hit in hits) {
                        if (hit.distance <= range && 
                            hit.collider.gameObject.tag != "Player" &&
                            !hit.collider.isTrigger ) {
                            // Push physics, regardless of hittable
                            Rigidbody r;
                            if (r = hit.collider.GetComponent<Rigidbody>()) {
                                // Play around with a good factor here
                                r.AddForceAtPosition(getLookObj().forward * 300 * Time.deltaTime, getLookObj().position);
                            }
                            // Hit with hittable
                            Hittable hittable = hit.collider.GetComponentInParent<Hittable>();
                            if (hittable != null && hit.collider.gameObject.tag != "Item") { //Sometimes may hit our item that we are holding
                                print(hit.collider);
                                hittable.Hit(baseDamage, getLookObj().transform.forward, damageType);
                            }
                        }
                    }

                }
            }
		} else if (mouseDown && statsController.GetMagic() > 0) {
            if (onCooldown) {
                
                if (releaseTime + Time.deltaTime >= timeToCooldown) {
                    onCooldown = false;
                } else {
                    releaseTime += Time.deltaTime;
                }
            } else {
                getPlayerAnim().SetBool(getControllerSide() + "MagicAttack", true);
                attacking = true;
                holdTime = 0;
            }
        } else {
            releaseTime += Time.deltaTime;
        }

	}

    public void pauseParticles() {
        if (shootParticles.isPlaying) {
            shootParticles.Stop();
        }
        if (idleParticles.isPlaying) {
            idleParticles.Stop();
        }
    }
    public void playParticles() {
        idleParticles.Play();
    }



}

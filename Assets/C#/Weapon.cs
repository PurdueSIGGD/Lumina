using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : ItemStats {

    public Hittable.DamageType damageType;
	public float timeToAttack; //Time inbetween pressing button and actually attacking
	public float cooldownLength; //Time between end of attack and next attack
	public float timeSincePress;
	public float range;
	public float damage;
	public Animator anim;
	public GameObject lookObj;
    public abstract void Attack(float deltaTime, bool mouseDown);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : ItemStats {

	public enum WeaponCategory {Melee, Projectile, Magic};
    public Hittable.DamageType damageType;
	public float weaponSpeed; //Time inbetween pressing button and actually attacking
	public float coolDown; //Time between end of attack and next attack
	public float range;
	public float damage;
    public abstract void Attack(bool mouseDown);

}

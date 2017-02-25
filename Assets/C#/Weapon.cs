using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : ItemStats {

	enum WeaponCategory {Melee,Projectile,Magic};
	public Hittable.DamageType damageType = Hittable.DamageType.Neutral;
	float weaponSpeed = 1.0f;
	float coolDown = 1.0f;
	float range = 1.0f;
	float damage = 1.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}

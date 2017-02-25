using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : ItemStats {

	enum WeaponCategory {Melee,Projectile,Magic};
	enum DamageType {Fire,Water,Electricity,Leaf,Steel,Ghost};
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

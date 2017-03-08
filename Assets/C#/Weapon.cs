using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : ItemStats {

	public enum damageType{wimpy,normal,glad,umbra};

	float minDamage;
	float maxDamage;

	public damageType type;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	override public void Damage(){}

	override public void Upgrade(){}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemStats : MonoBehaviour {

	int tier;
	float condition; //condition durability of the item
	int factor;//I have no Idea what kind of variable that is


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public abstract void Upgrade();

	public abstract void Damage();
}

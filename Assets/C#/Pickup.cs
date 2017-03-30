using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {


	public enum pickUpType{magic,health,upgradeHealth,upgradeMagic,upgradeLight,upgradeKit};

	public float amount;

	public pickUpType itemType;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

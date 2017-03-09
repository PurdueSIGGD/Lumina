using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStats : MonoBehaviour {

	public enum Tier {Simple,Moderate,Epic};

	public Tier tier = Tier.Simple;
	public float condition = 100.0f;
	public float minCondition = 1.0f;
	public float maxCondition = 100.0f;
	public float factor = 1.0f;

	void Start() {
		condition = Random.Range (minCondition, maxCondition);
	}

	/**
	 * 
	 * Increases the damage factor and applies damage to the item.
	 * 
	 * Paramters:
	 * damage: the amount of damage to give (percentage of max condition)
	 * 
	 **/
	void DamageCondition(float damage) {
		factor += damage/maxCondition;
		condition -= damage * factor;
	}

	/**
	 * 
	 * Upgrades the condition of the item.
	 * 
	 * Parameters:
	 * upgradeFactor: the amount to upgrade by (relative to maximum percentage of 
	 * 
	 **/
	void Upgrade(float upgradeFactor) {
		condition += upgradeFactor;
	}

	/**
	 * 
	 * Upgrades the condition of the item by a 10% increase.
	 * 
	 **/
	void Upgrade() {
		Upgrade (10.0f);
	}


}

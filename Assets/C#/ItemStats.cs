using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStats : MonoBehaviour {

	public enum Tier {Simple,Moderate,Epic};

	public Tier tier = Tier.Simple;
	public float condition = 100.0f;
	public float minCondition = 1.0f; // In percent (%)
	public float maxCondition = 100.0f; // In percent (%)
    private float factor = 1.0f;
    public float getCondition() { return condition; }
    

	void Start() {
		//condition = Random.Range (minCondition, maxCondition);
	}

	/**
	 * 
	 * Increases the damage factor and applies damage to the item.
	 * 
	 * Paramters:
	 * damage: the amount of damage to give (percentage of max condition)
	 * 
	 **/
	public void DamageCondition(float damage) {
        if (condition > minCondition) {
            factor += damage / maxCondition;
            condition -= damage * factor;
        } else {
            condition = minCondition;
        }
	}

    /**
	 * 
	 * Upgrades the condition of the item.
	 * 
	 * Parameters:
	 * upgradeFactor: the amount to upgrade by (relative to maximum percentage of 
	 * 
	 **/
    public void Upgrade(float upgradeFactor) {
		condition += upgradeFactor;
	}

    /**
	 * 
	 * Upgrades the condition of the item by a 10% increase.
	 * 
	 **/
    public void Upgrade() {
		Upgrade (10.0f);
	}


}

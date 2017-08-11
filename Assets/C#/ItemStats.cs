using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStats : MonoBehaviour {

    //basic information
	public enum Tier {Simple,Moderate,Epic};
    public Tier tier = Tier.Simple;
	public float condition = 100.0f;
	public float minCondition = 1.0f; // In percent (%)
	public float maxCondition = 100.0f; // In percent (%)
    private float factor = 1.0f;
    public float getCondition() { return condition; }
    
    //meta information
    [Header("Bag Item")]
    public string displayName; //name to display in bag/inventory

    [Multiline] public string description; //description in help panel

    public Sprite sprite;   //sprite to use in bag


    void Start() {
        // Condition should be set by the spawning class
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


    #region Comparing

    /// <summary>
    /// Help compare 2 ItemStats
    /// make it easier to control GUI
    /// </summary>
    /// <returns>True if same</returns>
    public bool compareTo(ItemStats item)
    {
        if (item == null)
            return false;

        return (this.displayName == item.displayName)
            && (this.tier == item.tier)
            && (this.condition == item.condition);
    }

    public static bool operator ==(ItemStats i1, ItemStats i2)
    {
        if (object.ReferenceEquals(i1, null))
        {

            return object.ReferenceEquals(i2, null);
        }

        if (object.ReferenceEquals(i2, null))
        {

            return object.ReferenceEquals(i1, null);
        }

        return i1.compareTo(i2);
    }

    public static bool operator !=(ItemStats i1, ItemStats i2)
    {
        return !(i1 == i2);
    }

    /// <summary>
    /// function that Dictionary will use to compare Key
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public override bool Equals(object other)
    {
        ItemStats otherItem = other as ItemStats;

        return this.compareTo(otherItem);
    }

    public override int GetHashCode()
    {
        return this.displayName.GetHashCode();
    }

    #endregion
}

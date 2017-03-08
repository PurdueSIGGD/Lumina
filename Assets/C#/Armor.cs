using UnityEngine;
using UnityEditor;

public class Armor: ItemStats{

	public enum armorType{head,chestplate}

	public armorType type;
	float minArmor;
	float maxArmor;

	void Start(){
	}

	override public void Damage(){
	}

	override public void Upgrade(){
	}

	public void takeDamage(){
		
	}
}


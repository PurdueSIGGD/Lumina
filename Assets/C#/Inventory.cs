using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
	
	bool canpickup;
	int upgradekit;
	int upgradePotions;

	Armor helmet;
	Armor chestPlate;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void pickUpItem(Armor item){
		switch(item.type){
		case Armor.armorType.head:
				helmet = item;
				break;
		case Armor.armorType.chestplate:
			chestPlate = item;
			break;
		}
	}

	void useUpgradeKit(ItemStats i){
	}

	void useUpgradePotion(){
	}

	void OnTriggerEnter(Collider c){
		
	}
}

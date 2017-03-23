using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour {

    private float interactCooldown;
	//Vector3 cameraAim;
	//public GameObject cameraObj;
	public StatsController sC;
	public GameObject cam;//camera
	float upgradekits;
	float upgradeHealth;
	float upgradeMagic;
	bool canpickup;
	RaycastHit[] hitObjs;//the hopefully raycast of an item that it find
	Armor helmet;
	Armor chestPlate;
	Armor Get;
	Pickup Pick;



	void Start () {
		interactCooldown = 1;

	}

	void Update () {
		hitObjs = Physics.RaycastAll (this.transform.position,cam.transform.forward,50f);
		for(int i = 0; i < hitObjs.Length ;i++){
			string itemTag = hitObjs[i].collider.gameObject.tag;
			if (itemTag.CompareTo ("Armor") == 0) {
				Get = hitObjs [i].collider.gameObject.GetComponent<Armor>();
				Debug.Log ("Seeing Armor "+Get.type);
			}
		}
	}

	/*public void itemScan(){
		hitObjs = Physics.RaycastAll (this.transform.position,transform.forward,30f);
		for(int i = 0; i < hitObjs.Length ;i++){
			string itemTag = hitObjs[i].collider.gameObject.tag;
			if (itemTag.CompareTo ("Armor") == 0) {
				Get = hitObjs [i].collider.gameObject.GetComponent<Armor>();
				Debug.Log ("Seeing Armor "+Get.type);
			}
		}
	}*/

	public void Interact(bool value)
    {
        // If value is true, pick up
        if (value)
        {
			if (Time.timeSinceLevelLoad - interactCooldown > 1) {
				interactCooldown = Time.timeSinceLevelLoad;
			} else {
				return;
			}
			Debug.Log ("Interacting");
			for (int i = 0; i < hitObjs.Length; i++) {
				Get = hitObjs [i].collider.gameObject.GetComponentInParent<Armor> ();
				if (Get == null)
					continue;
				pickUpItem (Get);
				Debug.Log ("player is Equipping Armor ");
				Destroy (Get.gameObject);
			}

        } 

    }

	public void pickUpItem(Armor item){
		switch(item.type){
		case Armor.ArmorPiece.helmet:
			helmet = item;
			break;
		case Armor.ArmorPiece.chestplate:
			chestPlate = item;
			break;
		}
	}

    private void OnTriggerEnter(Collider other)
    {
			if(other.gameObject.GetComponentInParent<Pickup>()!= null){
			Pick = other.gameObject.GetComponentInParent<Pickup> ();
			Debug.Log ("Player picked up " + Pick.itemType);
			switch(Pick.itemType){
			case Pickup.pickUpType.upgradeKit:
				upgradekits += Pick.amount;
				break;
			case Pickup.pickUpType.upgradeHealth:
				upgradeHealth += Pick.amount;
				break;
			case Pickup.pickUpType.upgradeMagic:
				upgradeMagic += Pick.amount;
				break;
			}
			Destroy (Pick.gameObject);
		}
    }

	void useUpgradeKit(ItemStats i){
		//make sure to decrease durability on an item
		i.condition -= (i.condition*.05f);
		if (i.condition < i.minCondition)
			i.condition = i.minCondition;
	}

	void useUpgradePotion(Pickup p){
		switch (p.itemType) {
		case Pickup.pickUpType.upgradeHealth:
			sC.UpgradeMaxHealth();
			break;
		case Pickup.pickUpType.upgradeMagic:
			sC.UpgradeMaxMagic();
			break;
		}
	}

	public List<Armor> GetEquippedArmor() {
		List<Armor> armor = new List<Armor> ();
		armor.Add (chestPlate);
		armor.Add (helmet);
		return armor;
	}
}

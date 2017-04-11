using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour {
    private static float GRAB_DISTANCE = 4;


    public WeaponController rightWeaponController;
    public WeaponController leftWeaponController;

    public Animator viewmodelAnimator;
    private float interactCooldown;
	public StatsController sC;
	public GameObject cam;//camera
	float upgradekits;
	float upgradeHealth;
	float upgradeMagic;
	bool canpickup;
	RaycastHit[] hitObjs;//the hopefully raycast of an item that it find
	Armor helmet;
	Armor chestPlate;
	Pickup Pick;
    ItemStats get;


	void Start () {
		interactCooldown = .3f;

	}

	void Update () {
		hitObjs = Physics.RaycastAll (cam.transform.position,cam.transform.forward, GRAB_DISTANCE);
        //Debug.DrawLine(cam.transform.position, cam.transform.position + cam.transform.forward);
        //print(hitObjs.Length);
		for(int i = 0; i < hitObjs.Length ;i++){
			string itemTag = hitObjs[i].collider.gameObject.tag;
			if (itemTag == "Item") {
				get = hitObjs [i].collider.gameObject.GetComponent<ItemStats>();
				//if (get) Debug.Log ("Seeing item "+ get.gameObject.name);
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
			if (Time.timeSinceLevelLoad - interactCooldown <= 1) {
				return;
			}
			//Debug.Log ("Interacting");
			for (int i = 0; i < hitObjs.Length; i++) {
                get = hitObjs [i].collider.GetComponent<ItemStats> ();
				if (get == null)
					continue;
				pickUpItem (get);
                //Debug.Log ("player is Equipping Armor ");
                //Destroy (get.gameObject);
                // Reset timer
                interactCooldown = Time.timeSinceLevelLoad;

            }
        } 

    }

	public void pickUpItem(ItemStats item){
        Transform lastItem = null;
        if (item is Armor) { 
            switch (((Armor)item).type){
		    case Armor.ArmorPiece.helmet:
                if (helmet) lastItem = helmet.transform;
			    helmet = (Armor)item;
			    break;
		    case Armor.ArmorPiece.chestplate:
                if (chestPlate) lastItem = chestPlate.transform;
			    chestPlate = (Armor)item;
			    break;
		    }
            if (lastItem) {
                //Drop last item
                lastItem.GetComponent<Rigidbody>().isKinematic = true;
                lastItem.parent = null;
                lastItem.GetComponent<Collider>().isTrigger = false;
            }
            // Set our newest item as our currently equipped item
            item.transform.parent = this.transform;
            item.transform.localPosition = Vector3.zero;
            item.GetComponent<Rigidbody>().isKinematic = true;
            item.GetComponent<Collider>().isTrigger = true;
        }
        if (item is Magic) {
            //print("Picked up magic");
            leftWeaponController.EquipWeapon((Weapon)item);
        } else  if (item is Weapon) {
            //print("Picked up weapon");
            rightWeaponController.EquipWeapon((Weapon)item);
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
			case Pickup.pickUpType.health:
				sC.UpdateHealth(Pick.amount);
				break;
			case Pickup.pickUpType.magic:
				sC.UpdateMagic(Pick.amount);
				break;
			}
			Destroy (Pick.gameObject);
		}
    }

	void useUpgradeKit(ItemStats i){
        i.Upgrade(10);
		

		//The upgrades of your armor are more effective depending on the condition of your armor
		if (i is Armor) {
			((Armor)i).flatDamageBlock += (2.5f * i.getCondition());
			((Armor)i).percentDamageBlock += (1.25f * i.getCondition());
			return;
		}

		
    }



	void useUpgradePotion(Pickup p){
		switch (p.itemType) {
		case Pickup.pickUpType.upgradeHealth:
			sC.UpgradeMaxHealth();
			break;
		case Pickup.pickUpType.upgradeMagic:
			sC.UpgradeMaxMagic();
			break;
		case Pickup.pickUpType.upgradeLight:
			sC.UpgradeMaxLightt();
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

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
	float upgradePotions;
	bool canpickup;
	RaycastHit[] hitObjs;//the hopefully raycast of an item that it find
	public Armor helmet;
	public Armor chestPlate;
	Pickup pick;
    ItemStats get;


	void Start () {
		interactCooldown = .3f;

	}

	void Update () {
		hitObjs = Physics.RaycastAll (cam.transform.position,cam.transform.forward, GRAB_DISTANCE);
        //Debug.DrawLine(cam.transform.position, cam.transform.position + cam.transform.forward);
        //print(hitObjs.Length);
		for(int i = 0; i < hitObjs.Length ;i++){
            ItemStats its;
            Usable usb;
            if (its = hitObjs[i].collider.GetComponent<ItemStats>()) {
                // Show GUI info here using its
            } else if (usb = hitObjs[i].collider.GetComponentInParent<Usable>()) {
                // Show usetext using usb
                Debug.Log(usb.getInfoText());
            }

			//string itemTag = hitObjs[i].collider.gameObject.tag;
			//if (itemTag == "Item") {
				//get = hitObjs [i].collider.gameObject.GetComponent<ItemStats>();
				//if (get) Debug.Log ("Seeing item "+ get.gameObject.name);
			//}
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
                if (hitObjs[i].collider.GetComponent<ItemStats>()) {
                    get = hitObjs[i].collider.GetComponent<ItemStats>();
                    if (get == null)
                        continue;
                    pickUpItem(get);
                    //Debug.Log ("player is Equipping Armor ");
                    //Destroy (get.gameObject);
                    // Reset timer
                    interactCooldown = Time.timeSinceLevelLoad;
                } else if (hitObjs[i].collider.GetComponentInParent<Usable>()) {
                    Usable itemToUse = hitObjs[i].collider.GetComponentInParent<Usable>();
                    itemToUse.Use();

                    interactCooldown = Time.timeSinceLevelLoad;

                } else {
                    continue;
                }



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
				sC.pM.setEquipDescription (sC.pM.helmet,helmet);
			    break;
		    case Armor.ArmorPiece.chestplate:
                if (chestPlate) lastItem = chestPlate.transform;
			    chestPlate = (Armor)item;
				sC.pM.setEquipDescription (sC.pM.chestplate,chestPlate);
			    break;
		    }
            if (lastItem) {
                //Drop last item
                lastItem.position = transform.position + transform.forward * 3 + Vector3.up;
                lastItem.GetComponent<Rigidbody>().isKinematic = false;
                lastItem.parent = null;
                lastItem.GetComponent<Collider>().isTrigger = false;
                lastItem.gameObject.SetActive(true);
            }
            // Set our newest item as our currently equipped item
            item.gameObject.SetActive(false);
            item.transform.parent = this.transform;
            item.transform.localPosition = Vector3.zero;
            item.GetComponent<Rigidbody>().isKinematic = true;
            item.GetComponent<Collider>().isTrigger = true;
        }
        if (item is Magic) {
            //print("Picked up magic");
            leftWeaponController.EquipWeapon((Weapon)item);
			sC.pM.setEquipDescription (sC.pM.weapL1,leftWeaponController.weapons[0]);
			sC.pM.setEquipDescription (sC.pM.weapL2,leftWeaponController.weapons[1]);
        } else  if (item is Weapon) {
            //print("Picked up weapon");
            rightWeaponController.EquipWeapon((Weapon)item);
			sC.pM.setEquipDescription (sC.pM.weapR1,rightWeaponController.weapons[0]);
			sC.pM.setEquipDescription (sC.pM.weapR2,rightWeaponController.weapons[1]);
        }

	}

    private void OnTriggerEnter(Collider other)
    {
			if(other.gameObject.GetComponentInParent<Pickup>()!= null){
			pick = other.gameObject.GetComponentInParent<Pickup> ();
			//Debug.Log ("Player picked up " + pick.itemType);
            bool deletes = true;
			switch(pick.itemType){
			    case Pickup.pickUpType.upgradeKit:
				    upgradekits += pick.amount;
					sC.pM.setUpgradeKAmount (upgradekits);
				    break;
				case Pickup.pickUpType.upgradePotion:
					upgradePotions++;
					sC.pM.setUpgradePAmount (upgradePotions);
                    break;
                case Pickup.pickUpType.Magic:
                    deletes = (sC.UpdateMagic(pick.amount) != pick.amount);
					sC.pM.setMagicAmount (pick.amount);
                    break;
                case Pickup.pickUpType.Health:
                    deletes = (sC.UpdateHealth(pick.amount) != pick.amount);
					sC.pM.setHealthAmount (pick.amount);
                    break;
                case Pickup.pickUpType.Arrow:
                    deletes = (sC.UpdateArrows((int)(pick.amount)) != pick.amount);
                    break;
			}
			if (deletes) Destroy (pick.gameObject);
		}
    }

	public void useUpgradeKit(ItemStats i){
        i.Upgrade(10);
		

		//The upgrades of your armor are more effective depending on the condition of your armor
		if (i is Armor) {
			((Armor)i).flatDamageBlock += (2.5f * i.getCondition());
			((Armor)i).percentDamageBlock += (1.25f * i.getCondition());
			upgradekits-=10;
			sC.pM.setUpgradeKAmount (upgradekits);
			if(((Armor)i).type == Armor.ArmorPiece.helmet){
				sC.pM.setEquipDescription (sC.pM.helmet,i);
			}else{
				sC.pM.setEquipDescription (sC.pM.chestplate,i);
			}
			return;
		}

		
    }



	public void useUpgradePotion(StatsController.StatType p){
		switch (p) {
		case StatsController.StatType.Health:
				sC.UpgradeMaxHealth ();
				upgradePotions--;
				sC.pM.setUpgradePAmount (upgradePotions);
				break;
		case StatsController.StatType.Magic:
                sC.UpgradeMaxMagic();
                upgradePotions--;
				sC.pM.setUpgradePAmount (upgradePotions);
                break;
		case StatsController.StatType.Light:
                sC.UpgradeMaxLightt();
                upgradePotions--;
				sC.pM.setUpgradePAmount (upgradePotions);
                break;
		}
	}

	public List<Armor> GetEquippedArmor() {
		List<Armor> armor = new List<Armor> ();
		if (chestPlate) armor.Add (chestPlate);
		if (helmet) armor.Add (helmet);
		return armor;
	}

	public float getUpgradePotions(){
		return upgradePotions;
	}

	public float getUpgradeKits(){
		return upgradekits;
	}
}

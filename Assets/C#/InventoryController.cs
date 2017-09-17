using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class InventoryController : MonoBehaviour {
    private static float GRAB_DISTANCE = 4;
    private static float GRAB_WIDTH = 1f;

    public GameObject[] helmetTypes;
    public GameObject[] chestplateTypes;

    public WeaponController rightWeaponController;
    public WeaponController leftWeaponController;

    public Animator viewmodelAnimator;
    private float interactCooldown;
	public StatsController statsController;
	public GameObject cam;//camera
	int upgradekits;
	int upgradePotions;
	bool canpickup;
	RaycastHit[] hitObjs;//the hopefully raycast of an item that it find
	public Armor helmet;
	public Armor chestPlate;
	Pickup pick;
    ItemStats get;

    private Text helpInteractText;  //text display to help interact
    private InventoryPanel inventoryPanel;

    private void Awake()
    {
        //get text
        helpInteractText = gameObject.GetComponent<InputGenerator>().uiController.hudController.helpInteractText;
    }

    void Start () {
		interactCooldown = .3f;

        //get stuff
        inventoryPanel = GetComponent<InputGenerator>().uiController.inventoryCanvas.inventoryPanel;
    }

	void Update () {
		hitObjs = Physics.CapsuleCastAll(cam.transform.position, cam.transform.position + cam.transform.forward * GRAB_DISTANCE, GRAB_WIDTH, cam.transform.forward);
        //Debug.DrawLine(cam.transform.position, cam.transform.position + cam.transform.forward * GRAB_DISTANCE);
		for(int i = 0; i < hitObjs.Length ;i++){

            ItemStats its;
            Usable usb;
            if (its = hitObjs[i].collider.GetComponent<ItemStats>()) {
                
				// Show GUI info here using its information
				//if (!helpInteractText.gameObject.activeSelf)
                //{
                //    helpInteractText.gameObject.SetActive(true);
                //}
				
            } 
			else if (usb = hitObjs[i].collider.GetComponentInParent<Usable>()) {
                
				// Show usetext using usb
                //Debug.Log(usb.getInfoText());
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

    /// <summary>
    /// Get value from InputGenerator and Iteract with Item
    /// </summary>
    /// <param name="value">True if user press "f"</param>
	public void Interact(bool value)
    {
        // If value is true, pick up
        if (value)
        {
			if (Time.time - interactCooldown <= 1) {
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
                    interactCooldown = Time.time;
                } else if (hitObjs[i].collider.GetComponentInParent<Usable>()) {
                    Usable itemToUse = hitObjs[i].collider.GetComponentInParent<Usable>();
                    itemToUse.Use();

                    interactCooldown = Time.time;

                } else {
                    continue;
                }



            }
        } else {
            //print("Nothing to interact with");
        }

    }

	public void pickUpItem(ItemStats item){

        Transform lastItem = null;
        if (item is Armor) {
            EquipArmor(item);
           
        }

        if (item is Magic) {
            //print("Picked up magic");
            leftWeaponController.EquipWeapon((Weapon)item);
		
            //add to inventory
            inventoryPanel.magicPanel.Add(item);
        } else  if (item is Weapon) {
            //print("Picked up weapon");
            rightWeaponController.EquipWeapon((Weapon)item);


            //add to weapon
            inventoryPanel.weaponPanel.Add(item);
        }

	}
    private void EquipArmor(ItemStats item) {
        Transform lastItem = null;
        switch (((Armor)item).type) {

            case Armor.ArmorPiece.helmet:

                //if there is already helment, throw the old one away
                if (helmet)
                    lastItem = helmet.transform;

                helmet = (Armor)item;
                break;
            case Armor.ArmorPiece.chestplate:

                //if there is already chestplate, throw the old one away
                if (chestPlate)
                    lastItem = chestPlate.transform;

                chestPlate = (Armor)item;
                break;
        }

        //Drop last item/armor
        if (lastItem) {

            //update GUI
            inventoryPanel.armorPanel.Drop(lastItem.GetComponent<ItemStats>());

            //update object               
            lastItem.position = transform.position + transform.forward * 3 + Vector3.up;
            lastItem.GetComponent<Rigidbody>().isKinematic = false;
            lastItem.parent = null;
            lastItem.GetComponent<Collider>().isTrigger = false;
            lastItem.gameObject.SetActive(true);

            if (!lastItem.GetComponent<DestroyOnLevelLoad>()) lastItem.gameObject.AddComponent<DestroyOnLevelLoad>();
        }

        // Set our newest item as our currently equipped item
        item.gameObject.SetActive(false);
        item.transform.parent = this.transform;
        item.transform.localPosition = Vector3.zero;
        item.GetComponent<Rigidbody>().isKinematic = true;
        item.GetComponent<Collider>().isTrigger = true;
        if (item.GetComponent<DestroyOnLevelLoad>()) Destroy(item.gameObject.GetComponent<DestroyOnLevelLoad>());


        //add to GUI inventory
        inventoryPanel.armorPanel.Add(item);
    }

    // Which armor: 0 for helmet, 1 for chestplate
    public void SetArmor(int whichArmor, int armorType, float armorCondition) {
        GameObject[] itemsToConsider = (whichArmor == 0 ? helmetTypes : chestplateTypes);
        GameObject spawnedArmor = GameObject.Instantiate(itemsToConsider[armorType], transform.position, Quaternion.identity);
        spawnedArmor.GetComponent<Armor>().condition = armorCondition;
        EquipArmor(spawnedArmor.GetComponent<ItemStats>());
    }

    private void OnTriggerEnter(Collider other)
    {
			if(other.gameObject.GetComponentInParent<Pickup>()!= null){
			    pick = other.gameObject.GetComponentInParent<Pickup> ();
			    //Debug.Log ("Player picked up " + pick.itemType);
                bool deletes = true;
			    switch(pick.itemType){
			        case Pickup.pickUpType.upgradeKit:
				        upgradekits += (int)pick.amount;
				        break;
				    case Pickup.pickUpType.upgradePotion:
					    upgradePotions+= (int)pick.amount;
                        break;
                    case Pickup.pickUpType.Magic:
                        deletes = (statsController.UpdateMagic(pick.amount) != pick.amount);
                        break;
                    case Pickup.pickUpType.Health:
                        deletes = (statsController.UpdateHealth(pick.amount) != pick.amount);
                        break;
                    case Pickup.pickUpType.Arrow:
                        deletes = (statsController.UpdateArrows((int)(pick.amount)) != pick.amount);
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
			upgradekits-=1;
			if(((Armor)i).type == Armor.ArmorPiece.helmet){
			}else{

			}
			return;
		}

		
    }



	public void useUpgradePotion(StatsController.StatType p){
		switch (p) {
		case StatsController.StatType.Health:
				statsController.UpgradeMaxHealth ();
				upgradePotions--;
				break;
		case StatsController.StatType.Magic:
                statsController.UpgradeMaxMagic();
                upgradePotions--;
                break;
		case StatsController.StatType.Light:
                statsController.UpgradeMaxLightt();
                upgradePotions--;
                break;
		}
	}

	public List<Armor> GetEquippedArmor() {
		List<Armor> armor = new List<Armor> ();
		if (chestPlate) armor.Add (chestPlate);
		if (helmet) armor.Add (helmet);
		return armor;
	}

	public int getUpgradePotions(){
		return upgradePotions;
	}

	public int getUpgradeKits(){
		return upgradekits;
	}

    public void SetUpgradePotions(int i)
    {
        upgradePotions = i;
    }
    public void SetUpgradeKits(int i)
    {
        upgradekits = i;
    }
}

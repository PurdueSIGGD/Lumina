using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
	public Text healthAmount;
	public Text magicAmount;
	public Text upgradePAmount;
	public Text upgradeKAmount;
	public Text helmet;
	public Text chestplate;
	public Text weapR1;
	public Text weapR2;
	public Text weapL1;
	public Text weapL2;
	public HUDController hudController;
	public InventoryController inventoryController;

	public bool changeState;

	bool pausing;
	bool upgrading;

	// Use this for initialization
	void Start () {
		changeState = false;
		pausing = false;
		upgrading = false;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public bool getPause(){
		return pausing;
	}

	public void setPause(bool b){
		pausing = b;
	}

	public void closeHUDOpenPause(){
		gameObject.SetActive (true);
		hudController.gameObject.SetActive (false);
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}

	public void closePauseOpenHUD(){
		gameObject.SetActive (false);
		hudController.gameObject.SetActive (true);
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	public void setHealthAmount(float amount){
		healthAmount.text = ""+amount;
	}

	public void setMagicAmount(float amount){
		magicAmount.text = "" + amount;
	}

	public void setUpgradePAmount(float amount){
		upgradePAmount.text = "" + amount;
	}

	public void setUpgradeKAmount(float amount){
		upgradeKAmount.text = "" + amount;
	}

	public void setEquipDescription(Text t, ItemStats iS){
		if (iS == null) {
			t.text = "None Equipped";
			return;
		}
		if(iS is Weapon){
			t.text = "Tier: "+iS.tier+"\nDamage: " + (((Weapon)iS).baseDamage * (iS.condition/iS.maxCondition)) + "\nDamageType: "+ ((Weapon)iS).damageType+"\nCooldown: "+((Weapon)iS).timeToCooldown+"\nCondition: "+iS.condition;
		}
		if(iS is Armor){
			t.text = "Tier: "+iS.tier+"\nDamageBlock: " + ((Armor)iS).flatDamageBlock+"\nPercentBlock: "+((Armor)iS).percentDamageBlock+"\nStrongAgainst: "+((Armor)iS).strongAgainst+"\nCondition: "+iS.condition;
		}
	}

	public void clickUpgrade(){
		upgrading = !upgrading;
		Debug.Log ("upgrade is "+ upgrading);
	}

	public void clickHealth(){
		Debug.Log ("Clicked Health");
		if (upgrading) {
			if (inventoryController.getUpgradePotions () > 0) {
				inventoryController.useUpgradePotion (StatsController.StatType.Health);
			}
		}
	}

	public void clickMagic(){
		Debug.Log ("Clicked Magic");
		if(upgrading){
			if (inventoryController.getUpgradePotions () > 0) {
				inventoryController.useUpgradePotion (StatsController.StatType.Magic);
			}
		}
	}

	public void clickLightt(){
		Debug.Log ("Clicked Light");
		if(upgrading){
			if (inventoryController.getUpgradePotions () > 0) {
				inventoryController.useUpgradePotion (StatsController.StatType.Light);
			}
		}
	}

	public void clickWeaponL1(){
		if(upgrading){
			if (inventoryController.getUpgradeKits () > 0) {
				inventoryController.useUpgradeKit (inventoryController.leftWeaponController.weapons [0]);
			}
		}
	}

	public void clickWeaponL2(){
		if(upgrading){
			if (inventoryController.getUpgradeKits () > 0) {
				inventoryController.useUpgradeKit (inventoryController.leftWeaponController.weapons [1]);
			}
		}
	}

	public void clickWeaponR1(){
		if(upgrading){
			if (inventoryController.getUpgradeKits () > 0) {
				inventoryController.useUpgradeKit (inventoryController.rightWeaponController.weapons [0]);
			}
		}
	}

	public void clickWeaponR2(){
		if(upgrading){
			if (inventoryController.getUpgradeKits () > 0) {
				inventoryController.useUpgradeKit (inventoryController.rightWeaponController.weapons [1]);
			}
		}
	}

	public void clickHelmet(){
		if(upgrading){
			if (inventoryController.getUpgradeKits () > 0) {
				inventoryController.useUpgradeKit (inventoryController.helmet);
			}
		}
	}

	public void clickChestplate(){
		if(upgrading){
			if (inventoryController.getUpgradeKits () > 0) {
				inventoryController.useUpgradeKit (inventoryController.chestPlate);
			}
		}
	}
}

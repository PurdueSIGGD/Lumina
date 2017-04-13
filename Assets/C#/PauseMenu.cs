using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
	public Text healthAmount;
	public Text magicAmount;
	public Text upgradeAmount;
	public HUDController hC;
	public InventoryController iC;

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
		hC.gameObject.SetActive (false);
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}

	public void closePauseOpenHUD(){
		gameObject.SetActive (false);
		hC.gameObject.SetActive (true);
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	public void setHealthAmount(float amount){
		healthAmount.text = ""+amount;
	}

	public void setMagicAmount(float amount){
		magicAmount.text = "" + amount;
	}

	public void setUpgradeAmount(float amount){
		upgradeAmount.text = "" + amount;
	}

	public void clickUpgrade(){
		upgrading = !upgrading;
		Debug.Log ("upgrade is "+ upgrading);
	}

	public void clickHealth(){
		Debug.Log ("Clicked Health");
		if (upgrading) {
			if(iC.getUpgradePotions() > 0)
				iC.useUpgradePotion (StatsController.StatType.Health);
		}
	}

	public void clickMagic(){
		Debug.Log ("Clicked Magic");
		if(upgrading){
			if(iC.getUpgradePotions() > 0)
				iC.useUpgradePotion (StatsController.StatType.Magic);
		}
	}

	public void clickLightt(){
		Debug.Log ("Clicked Light");
		if(upgrading){
			if(iC.getUpgradePotions() > 0)
				iC.useUpgradePotion (StatsController.StatType.Light);
		}
	}

	public void clickWeaponL1(){
		if(upgrading){
			if (iC.getUpgradeKits () > 0)
				iC.useUpgradeKit (iC.leftWeaponController.weapons[0]);
		}
	}

	public void clickWeaponL2(){
		if(upgrading){
			if (iC.getUpgradeKits () > 0)
				iC.useUpgradeKit (iC.leftWeaponController.weapons[1]);
		}
	}

	public void clickWeaponR1(){
		if(upgrading){
			if (iC.getUpgradeKits () > 0)
				iC.useUpgradeKit (iC.rightWeaponController.weapons[0]);
		}
	}

	public void clickWeaponR2(){
		if(upgrading){
			if (iC.getUpgradeKits () > 0)
				iC.useUpgradeKit (iC.rightWeaponController.weapons[1]);
		}
	}

	public void clickHelmet(){
		if(upgrading){
			if (iC.getUpgradeKits () > 0)
				iC.useUpgradeKit (iC.helmet);
		}
	}

	public void clickChestplate(){
		if(upgrading){
			if (iC.getUpgradeKits () > 0)
				iC.useUpgradeKit (iC.chestPlate);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {
	//GUI file to easily edit other parts of the GUI through code
	public Slider healthBar;
	public Slider magicBar;
	public Slider lightForceBar;

	public StatsController sC;
	public PauseMenu pm;
	 
	float factorH; //factor that you multiply that shows amount of health
	float startH;
	float factorM; //factor that you multiply that shows amount of Magic
	float startM;
	float factorL;//factor that you multiply that shows amount for Light
	float startL;

	// Use this for initialization
	void Start () {
		startH = .65585f * healthBar.maxValue;
		startM = .65585f * magicBar.maxValue;
		startL = .72727f * lightForceBar.maxValue;

		factorH = (1 - .65585f)/healthBar.maxValue;
		factorM = (1 - .65585f)/magicBar.maxValue;
		factorL = (1 - .72727f)/lightForceBar.maxValue;

		GUIsetHealth (sC.GetHealth());
		GUIsetMagic (sC.GetMagic());
		GUIsetLight (sC.GetLightt());
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void GUIsetHealth(float amount){
		healthBar.value = startH + factorH * amount;
	}

	public void GUIsetUpgradeHealth(float amount){
		healthBar.maxValue = amount;
		startH = .65585f * healthBar.maxValue;
		factorH = (1 - .65585f)/healthBar.maxValue;
		GUIsetHealth (sC.GetHealth());
	}

	public void GUIsetMagic(float amount){
		magicBar.value = startM + factorM * amount;

	}

	public void GUIsetUpgradeMagic(float amount){
		magicBar.maxValue = amount;
		startM = .65585f * magicBar.maxValue;
		factorM = (1 - .65585f)/magicBar.maxValue;
		GUIsetMagic (sC.GetMagic());
	}

	public void GUIsetLight(float amount){
		lightForceBar.value = startL + factorL * amount;
	}

	public void GUIsetUpgradeLight(float amount){
		lightForceBar.maxValue = amount;
		startL = .65585f * lightForceBar.maxValue;
		factorL = (1 - .72727f)/lightForceBar.maxValue;
		GUIsetLight (sC.GetLightt());
	}

	public void GUIsetHealthAmount(float amount){
		pm.healthAmount.text = ""+amount;
	}

	public void GUIsetMagicAmount(float amount){
	}
}

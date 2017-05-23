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

	// Use this for initialization
	void Start () {

		GUIsetHealth (sC.GetHealth());
		GUIsetMagic (sC.GetMagic());
		GUIsetLight (sC.GetLightt());
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void GUIsetHealth(float amount){
		healthBar.value = amount;
	}

	public void GUIsetUpgradeHealth(float amount){
		healthBar.maxValue = amount;
	}

	public void GUIsetMagic(float amount){
		magicBar.value = amount;

	}

	public void GUIsetUpgradeMagic(float amount){
		magicBar.maxValue = amount;
	}

	public void GUIsetLight(float amount){
		lightForceBar.value = amount;
	}

	public void GUIsetUpgradeLight(float amount){
		lightForceBar.maxValue = amount;
	}
}

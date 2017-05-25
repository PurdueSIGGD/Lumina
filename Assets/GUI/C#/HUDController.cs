using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {
	
    //GUI file to easily edit other parts of the GUI through code
	public Slider healthBar;
	public Slider magicBar;
	public Slider lightForceBar;

	public StatsController statsController;
	public PauseMenu pauseMenu;

	// Use this for initialization
	void Start () {

        SetupBar();
	}
	
	// Update is called once per frame
	void Update () {

	}


    public void SetupBar()
    {
        //health
        healthBar.maxValue = statsController.healthMax;
        healthBar.value = statsController.health;

        //magic
        magicBar.maxValue = statsController.magicMax;
        magicBar.value = statsController.magic;

        //light
        lightForceBar.maxValue = statsController.lighttMax;
        lightForceBar.value = statsController.lightt;
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

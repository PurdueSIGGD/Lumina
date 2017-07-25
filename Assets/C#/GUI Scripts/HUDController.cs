using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {
	
    //GUI file to easily edit other parts of the GUI through code
	public Slider healthBar;
    public Text healthText;

	public Slider magicBar;
    public Text magicText;

    public Slider lightForceBar;
    public Text lightText;

    public PauseMenu pauseMenu;

    //text to display instruction
    public Text helpInteractText;

    public StatsController statsController { get; set; }
    public UIController uiController { get; set; }


    private CanvasGroup canvasGroup;

    // Use this for initialization
    void Start () {

        SetupBar();
        canvasGroup = GetComponent<CanvasGroup>();
	}
	
	// Update is called once per frame
	void Update () {

	}

    /// <summary>
    /// Set GUI.alpha of the stats panel
    /// </summary>
    public float alpha {
        get
        {
            return canvasGroup.alpha;
        }
        set
        {
            canvasGroup.alpha = value;
        }
    }

    public void SetupBar()
    {
        //check
        if (statsController == null)
        {
            Debug.Log("HUD: not set StatsController yet");
            return;
        }

        //health
        healthBar.maxValue = statsController.healthMax;
        healthBar.value = statsController.health;
        updateHealthText();
        
        //magic
        magicBar.maxValue = statsController.magicMax;
        magicBar.value = statsController.magic;
        updateMagicText();

        //light
        lightForceBar.maxValue = statsController.lighttMax;
        lightForceBar.value = statsController.lightt;
        updateLightText();
    }

    public void GUIsetHealth(float amount){
		healthBar.value = amount;
        updateHealthText();
	}

	public void GUIsetUpgradeHealth(float amount){
		healthBar.maxValue = amount;
        updateHealthText();
    }

	public void GUIsetMagic(float amount){
		magicBar.value = amount;
        updateMagicText();
	}

	public void GUIsetUpgradeMagic(float amount){
		magicBar.maxValue = amount;
        updateMagicText();
	}

	public void GUIsetLight(float amount){
		lightForceBar.value = amount;
        updateLightText();
	}

	public void GUIsetUpgradeLight(float amount){
		lightForceBar.maxValue = amount;
        updateLightText();
	}

    public void updateHealthText()
    {
        healthText.text = healthBar.value.ToString() + "/" + healthBar.maxValue.ToString();
    }

    public void updateMagicText()
    {
        magicText.text = magicBar.value.ToString() + "/" + magicBar.maxValue.ToString();
    }

    public void updateLightText()
    {
        lightText.text = lightForceBar.value.ToString() + "/" + lightForceBar.maxValue.ToString();
    }
}

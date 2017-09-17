using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class SettingsCanvas : UICanvas {
  
 
    public UISettingsNavPanel navPanel;
    public UISettingsSidePanel settingsPanel;
    public UISettingsSidePanel loadGamePanel;
    public UISettingsSidePanel saveGamePanel;

    public UISettingsConfirmPanel mainMenuConfirmPanel;
    public UISettingsConfirmPanel quitConfirmPanel;

    [HideInInspector] public UIPanel currentSidePanel; //side panel beside nav
    [HideInInspector] public UIPanel currentConfirmPanel;

    public UIController uiController { get; set; }

    public InputGenerator inputGenerator { get; set; }

    private void Start()
    {
        //nav
        navPanel.controller = this;
        
        //main menu        
        mainMenuConfirmPanel.controller = this;

        //quit       
        quitConfirmPanel.controller = this;

        //settings        
        settingsPanel.controller = this;
        
        
    }

    public override void ToggleCanvas()
    {
        //if not open
        if (navPanel.gameObject.activeSelf == false)
        {
            navPanel.Open(); //navPanel contains the PauseGame and Resume Game
        }
        else
        {
            CloseSettingsCanvas();
        }
     
    }


    public void CloseSettingsCanvas()
    {
        if (currentConfirmPanel != null)
            currentConfirmPanel.Close();

        if (currentSidePanel != null)
            currentSidePanel.Close();

        navPanel.Close();
    }



}

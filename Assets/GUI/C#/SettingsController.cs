using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class SettingsController : MonoBehaviour {
  
    public EventSystem eventSystem;
    public InputGenerator inputGenerator;

    public UISettingsNavPanel navPanel;
    public UISettingsSidePanel settingsPanel;
    public UISettingsSidePanel loadGamePanel;
    public UISettingsSidePanel saveGamePanel;

    public UISettingsConfirmPanel mainMenuConfirmPanel;
    public UISettingsConfirmPanel quitConfirmPanel;

    [HideInInspector] public UIPanel currentSidePanel; //side panel beside nav
    [HideInInspector] public UIPanel currentConfirmPanel;

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

        //load game       
        loadGamePanel.controller = this;

        //save game      
        saveGamePanel.controller = this;
        
    }

    public void ToggleSettingsCanvas()
    {
        //if not open
        if (navPanel.gameObject.activeSelf == false)
        {
            navPanel.Open();
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

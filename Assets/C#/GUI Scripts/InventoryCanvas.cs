using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCanvas : UICanvas {

    public InventoryPanel inventoryPanel;
    public UINormalPanel avatarPanel;
    public UINormalPanel descriptionPanel;

    public UIController uiController { get; set; }

    public override void ToggleCanvas()
    {
        if (inventoryPanel.gameObject.activeSelf == false)
        {
            Open();
        }
        else
        {
            Close();
        }
    }

    public void Open()
    {
        //just in case
        //gameObject.SetActive(true);

        //open stuff
        inventoryPanel.Open();
        avatarPanel.Open();
    }

    public void Close()
    {
        inventoryPanel.Close();
        avatarPanel.Close();
    }
    
}

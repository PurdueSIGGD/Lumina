using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCanvas : UICanvas {

    public InventoryPanel inventoryPanel;
    public AvatarPanel avatarPanel;
    public UINormalPanel descriptionPanel;

	public RectTransform[] rects;

    public UIController uiController { get; set; }

    private List<UIPanel> listPanels;

    private void Awake()
    {
        listPanels = new List<UIPanel>()
        {
            inventoryPanel, avatarPanel, descriptionPanel
        };

        
    }

    public override void ToggleCanvas()
    {
        if (!inventoryPanel.active)
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
        //open itself
        this.SetActiveUI(true);
        
        //open stuff
        listPanels.ForEach(x => x.Open());

		foreach (RectTransform r in rects) {
			r.anchoredPosition = new Vector3(r.anchoredPosition.x, 0);
		}
    }

    public void Close()
    {
        //close all panel
        listPanels.ForEach(x => x.Close());
    }


    
}

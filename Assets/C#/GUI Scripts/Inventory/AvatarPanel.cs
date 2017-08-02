using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// handle displaying the avatar such as face, weapon icon, arrows count
/// </summary>
public class AvatarPanel : UIPanel {

    public Text arrowText;  //where to display arrow

    private UIController uiController;
    private Animator anim;

    private void Awake()
    {
        //get component
        anim = GetComponent<Animator>();    
    }

    private void Start()
    {
        uiController = GetComponentInParent<InventoryCanvas>().uiController;
    }

    /// <summary>
    /// Access StatsController and take the information itself.
    /// </summary>
    public void UpdateArrowCount()
    {
        //check
        if (this.uiController == null)
        {
            Debug.Log("AvatarPanel: UI Controller is not set.");
            return;
        }


        //get StatsController
        StatsController stats = uiController.player.GetComponent<StatsController>();

        //update UI
        arrowText.text = "Arrow Count: " + stats.arrowCount.ToString() + "/" + stats.arrowMax.ToString();
    }

    public override void Open()
    {
        SetActiveUI(true);
        anim.SetTrigger("Open");
    }

    public override void Close()
    {
        anim.SetTrigger("Close");
    }
}

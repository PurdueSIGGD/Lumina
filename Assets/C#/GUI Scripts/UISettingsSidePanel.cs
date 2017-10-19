using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISettingsSidePanel : UIPanel {

    private Animator anim;

    [HideInInspector] public SettingsCanvas controller;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Toggle()
    {
        //if this is already open, close it
        if (controller.currentSidePanel != null && controller.currentSidePanel == this)
        {
            Close();           
            return;
        }

        //if another one is open
        if (controller.currentSidePanel != null && controller.currentSidePanel != this)
        {
            controller.currentSidePanel.Close();
        }

        //open this
        Open();

        
    }

    public override void Close()
    {
        anim.SetTrigger("Close");
        controller.currentSidePanel = null;
    }

    public override void Open()
    {
        
        //open
        gameObject.SetActive(true);
        anim.SetTrigger("Open");

        //set current side
        controller.currentSidePanel = this;
    }


}

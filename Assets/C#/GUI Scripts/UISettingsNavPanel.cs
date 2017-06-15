using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UISettingsNavPanel : UIPanel
{
   
    
    public SettingsCanvas controller { get; set; }
   
    private Animator anim;
    

    public void SetController(SettingsCanvas controller)
    {
        this.controller = controller;        
    }

    /*
     * Use Awake() because this object is inactive
     */ 
    private void Awake()
    {         
        anim = GetComponent<Animator>();
    }
   
    public override void Close()
    {
        //close
        anim.SetTrigger("Close");

    }

    public override void Open()
    {
        //set active
        gameObject.SetActive(true);
     
        //open panel        
        anim.SetTrigger("Open");
    }

    
}

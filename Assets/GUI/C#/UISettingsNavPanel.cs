using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UISettingsNavPanel : UIPanel
{
   
    
    [HideInInspector] public SettingsCanvas controller; //don't need to assign in Editor

    [HideInInspector] public InputGenerator inputGenerator;
    private Animator anim;
    private CanvasGroup canvasGroup;

    public bool interactable {
        get
        {
            return canvasGroup.interactable;
        }
        set
        {
            canvasGroup.interactable = value;
        }
    }

    public void SetController(SettingsCanvas controller)
    {
        this.controller = controller;        
    }

    /*
     * Use Awake() because this object is inactive
     */ 
    private void Awake()
    {
        inputGenerator = controller.inputGenerator;
        anim = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
   
    public override void Close()
    {
        //close
        anim.SetTrigger("Close");

        //resume game
        inputGenerator.ResumeGame();
    }

    public override void Open()
    {
        //set active
        gameObject.SetActive(true);

        //pause game
        inputGenerator.PauseGame();

        //open panel        
        anim.SetTrigger("Open");
    }

    
}

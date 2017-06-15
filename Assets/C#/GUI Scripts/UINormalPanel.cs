
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Normal Panel, only used to control the Open and Close of a panel
/// </summary>
public class UINormalPanel : UIPanel
{

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// Close the panel
    /// </summary>
    public override void Close()
    {
        anim.SetTrigger("Close");
    }

    /// <summary>
    /// Open the panel
    /// </summary>
    public override void Open()
    {
        gameObject.SetActive(true);
        anim.SetTrigger("Open");
    }


}



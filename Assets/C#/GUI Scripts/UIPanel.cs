﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public abstract class UIPanel : UIBase
{
    

    /// <summary>
    /// should simply open the panel
    /// </summary>
    public abstract void Open();

    /// <summary>
    /// Close the panel and disable the gameObject
    /// so that this would not affect screen raycasting
    /// </summary>
    public abstract void Close();
}

    



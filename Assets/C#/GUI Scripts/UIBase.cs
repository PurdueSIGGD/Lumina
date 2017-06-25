using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base of most of UI component in this project to do with 
/// Animation and Interactive
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public abstract class UIBase : MonoBehaviour {


    /// <summary>
    /// The interactable of CanvasGroup attached to UI gameObject
    /// </summary>
    public bool interactable
    {
        get
        {
            return GetComponent<CanvasGroup>().interactable;
        }
        set
        {
            GetComponent<CanvasGroup>().interactable = value;
        }
    }

    /// <summary>
    /// The alpha of the canvasGroup attached to UI gameObject
    /// </summary>
    public float alpha
    {
        get
        {
            return GetComponent<CanvasGroup>().alpha;
        }

        set
        {
            GetComponent<CanvasGroup>().alpha = value;
        }
    }

    public bool blocksRayCasts
    {
        get
        {
            return GetComponent<CanvasGroup>().blocksRaycasts;
        }

        set
        {
            GetComponent<CanvasGroup>().blocksRaycasts = value;
        }
    }

    /// <summary>
    /// UI Active of the game object
    /// </summary>
    public bool active
    {
        get
        {
            return interactable;
        }

        set
        {
            SetActiveUI(value);
        }
    }

    /// <summary>
    /// Set Active of UI
    /// Same with ui.active
    /// </summary>
    /// <param name="active"></param>
    public void SetActiveUI(bool active)
    {
        interactable = active;
        blocksRayCasts = active;
        alpha = active ? 1 : 0;
    }

    
}

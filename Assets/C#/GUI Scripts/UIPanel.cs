using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public abstract class UIPanel : MonoBehaviour
{
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
    public abstract void Open();

    public abstract void Close();
}

    



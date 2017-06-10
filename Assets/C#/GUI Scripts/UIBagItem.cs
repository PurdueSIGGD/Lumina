using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used to control the UI element of a bag item more easily
/// </summary>
public class UIBagItem : MonoBehaviour {


    public Text displayNameText;
    public Image image;

    /// <summary>
    /// information to show in help panel
    /// </summary>
    public string description { get; set; }
    
    /// <summary>
    /// icon to display in bag
    /// </summary>
    public Sprite icon { get; set; }

    /// <summary>
    /// Display name to use in bag
    /// </summary>
    public string displayName { get; set; }

    /// <summary>
    /// Position Y to help adjust the panel
    /// </summary>
    public float posY {
        get
        {
            return rect.position.y;
        }
        set
        {
            rect.position = new Vector3(0, value);
        }
    }  


    private RectTransform rect;
    public UIInventoryBagPanel bag {get; set; }

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void SetIcon(Sprite sprite)
    {
        icon = sprite;
    }

    /// <summary>
    /// fast way to set up an element of UI Bag Item
    /// </summary>
    /// <param name="displayName"> display to use in bag</param>
    /// <param name="description"> description in the help Panel</param>
    /// <param name="icon"> icon to display</param>
    public void Setup(string displayName, string description, Sprite icon)
    {
        this.displayName = displayName;
        this.description = description;
        this.icon = icon;
    }
}

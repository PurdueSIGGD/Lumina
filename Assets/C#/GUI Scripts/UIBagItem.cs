using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used to control the UI element of a bag item more easily
/// </summary>
public class UIBagItem : MonoBehaviour {

    public Text displayNameText;    //place to display name
    public Image image; //place to display icon

    [HideInInspector] public RectTransform rect;

    public UIInventoryBagPanel bag {get; set; } //UI Inventory Bag that hold this

    /// <summary>
    /// bag item that this UI is attached to
    /// </summary>
    public BagItem item { get; set; }

    /// <summary>
    /// item stats that this UI is attached to
    /// </summary>
    public ItemStats itemStats { get; set; }    

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    /// <summary>
    /// Set up the item so that the UI element can use these information
    /// </summary>
    /// <param name="item"></param>
    public void Setup(BagItem item)
    {
        //set fields
        this.item = item;
        this.itemStats = item.GetComponent<ItemStats>();

        //set information
        displayNameText.text = item.displayName;
        image.sprite = item.sprite;
    }

    /// <summary>
    /// Show more information on the description panel
    /// </summary>
    public void DisplayDescription()
    {
        //find game Object
        GameObject gameObject  = GameObject.FindGameObjectWithTag("DescriptionPanel");

        //get component
        UIDescriptionPanel descriptionPanel = gameObject.GetComponent<UIDescriptionPanel>();

        //display
        if (descriptionPanel != null)
        {
            descriptionPanel.DisplayBagItemInfo(this);
        }
        
    }

   
}

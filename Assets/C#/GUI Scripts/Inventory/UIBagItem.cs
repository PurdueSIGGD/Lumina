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
    public static float height = 100;    //height of UI Rect when displaying

    [HideInInspector] public RectTransform rect;

    public InventoryBagPanel bag {get; set; } //UI Inventory Bag that hold this

    /// <summary>
    /// item stats that this UI is attached to
    /// </summary>
    public ItemStats itemStats { get; set; }    

    private void Awake()
    {
        //set basics, just in case
        rect = GetComponent<RectTransform>();
        rect.localScale = new Vector3(1, 1, 1);
    }

    /// <summary>
    /// Set up the item so that the UI element can use these information
    /// </summary>
    /// <param name="item">item Stats</param>
    public void Setup(ItemStats itemStats)
    {        
        //set
        this.itemStats = itemStats;

        //set information
        displayNameText.text = itemStats.displayName;
        image.sprite = itemStats.sprite;
    }
   
    /// <summary>
    /// Show more information on the description panel
    /// </summary>
    public void DisplayDescription()
    {
        //find game Object
        DescriptionPanel descriptionPanel
            = bag.inventoryPanel.GetComponentInParent<InventoryCanvas>().
            descriptionPanel.GetComponent<DescriptionPanel>();

        //display
        if (descriptionPanel != null)
        {
            descriptionPanel.DisplayBagItemInfo(this);
            this.GetComponentInParent<InventoryPanel>().displayedItem = this;
        }
        else
        {
            Debug.Log("UI Bag Item: cannot find Description Panel");
        }
        
    }

   
}

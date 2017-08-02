using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Linked with InventoryController
/// 
/// control database and UI elements of bag items being displayed.
/// </summary>
public class InventoryBagPanel : MonoBehaviour {

    public RectTransform content;   //rect of content, to adjust the height according to number of item in bag

    /// <summary>
    /// key: item, value: number of items
    /// </summary>
    [HideInInspector] public Dictionary<ItemStats, int> itemDict;

    /// <summary>
    /// to control the UI element of bag item
    /// </summary>
    private List<UIBagItem> itemUIList;
    
    public InventoryPanel inventoryPanel { get; set; } 

    private void Awake()
    {
        //init
        itemDict = new Dictionary<ItemStats, int>();
        itemUIList = new List<UIBagItem>();
    }

    private void Start()
    {
        //clear all children in case of debugging
        ClearAllItems();
    }


    /// <summary>
    /// Clear all children
    /// </summary>
    private void ClearAllItems()
    {
        List<GameObject> childList = new List<GameObject>();
        foreach( Transform child in content )
        {
            childList.Add(child.gameObject);
        }

        childList.ForEach(x => Destroy(x));
    }

    /// <summary>
    /// Add bag_item to the bag
    /// </summary>
    /// <param name="item">item to be added</param>
    public void Add(ItemStats item)
    {
        //debug
        if (itemDict == null || itemUIList == null)
        {
            print("Item Dict is not init");
        }

        //check if item is init for bag
        if (item == null)
            return;

        //if already contain, increase the number     
        if (itemDict.ContainsKey(item)) {
          
            //increment
            itemDict[item] += 1;

            //update the UI as well
            UIBagItem i = itemUIList.Find(x => x.itemStats.compareTo(item));
            i.displayNameText.text = i.itemStats.displayName + " (" + itemDict[item] + ")";

            return;
        }

        //if not exist, add new
        itemDict.Add(item, 1);
        AddNewUIBagItem(item);
    }

    private void AddNewUIBagItem(ItemStats item)
    {
        //make new bag item
        UIBagItem bagItem = Instantiate(inventoryPanel.genericBagItem, content);

        //set up information
        bagItem.Setup(item);
        //bagItem.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        

        //add to list
        itemUIList.Add(bagItem);
        UpdateBagContent();
    }

    private void UpdateBagContent()
    {
        //content.rect.height = itemUIList.Count;
        float height = itemUIList.Count * UIBagItem.height;
        content.sizeDelta = new Vector2(0, height);
    }
    
    

}

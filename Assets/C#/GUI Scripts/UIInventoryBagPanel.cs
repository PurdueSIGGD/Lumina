using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Linked with InventoryController
/// 
/// control the list of bag item being displayed.
/// </summary>
public class UIInventoryBagPanel : MonoBehaviour {

    public RectTransform content;   //rect of content, to adjust the height according to number of item in bag

    /// <summary>
    /// key: item, value: number of items
    /// </summary>
    [HideInInspector] public Dictionary<BagItem, int> itemDict;

    /// <summary>
    /// to control the UI element of bag item
    /// </summary>
    private List<UIBagItem> itemUIList;
    
    public InventoryPanel inventoryPanel { get; set; } 

    private void Awake()
    {
        //init
        itemDict = new Dictionary<BagItem, int>();
        itemUIList = new List<UIBagItem>();
    }

    private void Start()
    {
        //clear all children in case of debugging
        ClearAllItems();
    }

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
    public void Add(BagItem item)
    {
        if (itemDict == null || itemUIList == null)
        {
            print("Item Dict is not init");
        }

        //check if item is init for bag
        if (item == null)
            return;

        //if already contain, increase the number
        if (itemDict.ContainsKey(item)) {
            itemDict[item] += 1;
            return;
        }

        //if not exist, add new
        itemDict.Add(item, 1);
        AddNewUIBagItem(item);
    }

    private void AddNewUIBagItem(BagItem item)
    {
        //make new bag item
        UIBagItem bagItem = Instantiate(inventoryPanel.genericBagItem, content);

        //set up information
        bagItem.Setup(item);
        bagItem.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        

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

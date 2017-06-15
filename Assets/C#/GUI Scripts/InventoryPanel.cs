using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : UIPanel
{

    private Animator anim;

    public UIInventoryBagPanel weaponPanel;
    public UIInventoryBagPanel magicPanel;
    public UIInventoryBagPanel armorPanel;
    public UIInventoryBagPanel kitPanel;
    public UIBagItem genericBagItem;    //used as a guide line for normal bag item

    private List<UIInventoryBagPanel> listBags;

    /// <summary>
    /// current UI Inventory Bag being open
    /// </summary>
    public UIInventoryBagPanel currentBag { get; set; }

  

    private void Awake()
    {
        //get component
        anim = GetComponent<Animator>();

        //init and assign
        listBags = new List<UIInventoryBagPanel>()
        {
            weaponPanel, magicPanel, armorPanel, kitPanel
        };

        listBags.ForEach(x => x.inventoryPanel = this);
    }

    private void Start()
    {
        //make all bag non-active
        currentBag = null;
        //listBags.ForEach(x => x.gameObject.SetActive(false));
        listBags.ForEach(x => x.GetComponent<UIPanel>().SetActiveUI(false));
    }

    public override void Close()
    {
        anim.SetTrigger("Close");
    }

    public override void Open()
    {
        SetActiveUI(true);
        anim.SetTrigger("Open");
    }

    public void ToggleBag(UIInventoryBagPanel bag)
    {
        //if no current Open Bag
        if (currentBag == null)
        {
            //bag.gameObject.SetActive(true);
            bag.GetComponent<UIPanel>().SetActiveUI(true);
            currentBag = bag;
            return;
        }

        //if open Bag != bag
        if (currentBag != bag)
        {
            //currentBag.gameObject.SetActive(false);
            currentBag.GetComponent<UIPanel>().SetActiveUI(false);

            //bag.gameObject.SetActive(true);
            bag.GetComponent<UIPanel>().SetActiveUI(true);

            currentBag = bag;
            return;
        }

        //if open Bag == bag
        //do nothing
    }
   

}

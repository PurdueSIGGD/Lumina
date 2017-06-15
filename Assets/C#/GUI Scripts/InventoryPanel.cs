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

    private List<UIInventoryBagPanel> listBags;

    /// <summary>
    /// current UI Inventory Bag being open
    /// </summary>
    public UIInventoryBagPanel currentBag { get; set; }

  

    private void Awake()
    {
        anim = GetComponent<Animator>();
        listBags = new List<UIInventoryBagPanel>()
        {
            weaponPanel, magicPanel, armorPanel, kitPanel
        };
    }

    private void Start()
    {
        //make all bag non-active
        currentBag = null;
        listBags.ForEach(x => x.gameObject.SetActive(false));
    }

    public override void Close()
    {
        anim.SetTrigger("Close");
    }

    public override void Open()
    {
        gameObject.SetActive(true);
        anim.SetTrigger("Open");
    }

    public void ToggleBag(UIInventoryBagPanel bag)
    {
        //if no current Open Bag
        if (currentBag == null)
        {
            bag.gameObject.SetActive(true);
            currentBag = bag;
            return;
        }

        //if open Bag != bag
        if (currentBag != bag)
        {
            currentBag.gameObject.SetActive(false);
            bag.gameObject.SetActive(true);
            currentBag = bag;
            return;
        }

        //if open Bag == bag
        //do nothing
    }
   

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// control the GUI part of the Inventory Panel, part of the Inventory Canvas
/// </summary>
public class InventoryPanel : UIPanel
{

    private Animator anim;

    public InventoryBagPanel weaponPanel;
    public InventoryBagPanel magicPanel;
    public InventoryBagPanel armorPanel;
    public InventoryBagPanel kitPanel; //upgrade kit, arrows
    public UIBagItem genericBagItem;    //used as a guide line for normal bag item

    private List<InventoryBagPanel> listBags;

    public CanvasGroup upgrades;
    public Text upgradePotion;
    public Text upgradeKit;


    StatsController myStats;
    InventoryController myInventory;

    /// <summary>
    /// current UI Inventory Bag being open
    /// </summary>
    public InventoryBagPanel currentBag { get; set; }

    public UIBagItem displayedItem;

  

    private void Awake()
    {
        //get component
        anim = GetComponent<Animator>();

        //init and assign
        listBags = new List<InventoryBagPanel>()
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


        myStats = this.GetComponentInParent<StatsController>();
        myInventory = this.GetComponentInParent<InventoryController>();
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

    public void ToggleBag(InventoryBagPanel bag)
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
    public void Update() {
        if (displayedItem != null) {
            // Update on time
            displayedItem.DisplayDescription();
        }
        upgradePotion.text = String.Format("{0} Upgrade Potions", myInventory.getUpgradePotions());
        upgradeKit.text = String.Format("{0} Upgrade Kits", myInventory.getUpgradeKits());
        if (myInventory.getUpgradePotions() == 0) {
            upgrades.interactable = false;
            upgrades.alpha = 0.8f;
        } else {
            upgrades.interactable = true;
            upgrades.alpha = 1f;
        }

    }
    public void UseUpgradeKit() {
        if (displayedItem == null) { Debug.LogWarning("No currently displayed item found");  }
        else {
            
            if (myInventory.getUpgradeKits() > 0) {
                myInventory.useUpgradeKit(displayedItem.itemStats);
            }


        }

    }
    public void UpgradeLight() {
        myStats.UpgradeMaxLightt();
        myInventory.SetUpgradePotions(myInventory.getUpgradePotions() - 1);
    }
    public void UpgradeMagic() {
        myStats.UpgradeMaxMagic();
        myInventory.SetUpgradePotions(myInventory.getUpgradePotions() - 1);

    }
    public void UpgradeHealth() {
        myStats.UpgradeMaxHealth();
        myInventory.SetUpgradePotions(myInventory.getUpgradePotions() - 1);
    }


}

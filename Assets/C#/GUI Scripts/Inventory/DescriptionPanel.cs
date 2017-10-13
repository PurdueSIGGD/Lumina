using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used to control the stats panel to
/// display stats and description
/// </summary>
public class DescriptionPanel : MonoBehaviour {

    public Text titleText;          //place to put name or title
    public Text statsTitle;         //put type of weapon or magic to see clearly
    public Text statsText;          //place to put stats panel
    public Text descriptionText;    //place to put the description
    public Image image;             //place to put icon
    public Button upgradeButton; // Upgrade button
    

 public void Start() {
        foreach (Scrollbar s in this.GetComponentsInChildren<Scrollbar>()) {
            s.value = 0;
        }
    }
    /// <summary>
    /// Displace information associated with item
    /// </summary>
    /// <param name="i">Bag Item</param>
    public void DisplayBagItemInfo(UIBagItem i)
    {
        //title
        titleText.text = i.itemStats.displayName;
        image.sprite = i.itemStats.sprite;

        //description
        descriptionText.text = i.itemStats.description;

        //stats
        string stats = "";

        //general stats from ItemStats
        stats += "Tier: " + i.itemStats.tier.ToString() + "\n";      
        stats += "Condition: " + System.Math.Round(i.itemStats.condition, 2) + "/" + i.itemStats.maxCondition.ToString() + "\n";

        //specific stats from Item
        if (i.itemStats is Weapon)
        {
            Weapon weapon = (Weapon)i.itemStats;
            stats += "Damage: " + System.Math.Round(weapon.baseDamage * weapon.condition / weapon.maxCondition, 2) + "\n";
            stats += "Damage Type: " + weapon.damageType.ToString() + "\n";
            stats += "Cool down: "   + weapon.timeToCooldown.ToString() + "s" + "\n";
            stats += "Range: "       + weapon.range.ToString() + "\n";
        }

        //magic inherits weapon
        if (i.itemStats is Magic) {
            //set title
            statsTitle.text = "Magic";

            //add to stats description
            Magic magic = (Magic)i.itemStats;
            if (i.itemStats is BeamMagic) {
                stats += "Cost per second: " + ((BeamMagic)magic).magicDraw + "\n";
            }
            if (i.itemStats is BurstMagic) {
                stats += "Cost per burst: " + ((BurstMagic)magic).magicDraw + "\n";
            }


            upgradeButton.gameObject.SetActive(false);
        } else {
            upgradeButton.gameObject.SetActive(true);
            if (this.GetComponentInParent<InventoryController>().getUpgradeKits() > 0 &&
                i.itemStats.condition < i.itemStats.maxCondition) {
                upgradeButton.interactable = true;
            } else {
                upgradeButton.interactable = false;
            }

        }


        //projectile weapon inherit weapon
        if (i.itemStats is ProjectileWeapon)
        {
            //set title
            statsTitle.text = "Projectile Weapon";

            //add to stats description
            ProjectileWeapon pw = (ProjectileWeapon)i.itemStats;
            stats += "LaunchSpeed: " + pw.launchSpeed + "\n";

            
        }

        //swingingWeapon inherit weapon
        if (i.itemStats is SwingingWeapon)
        {
            //set title
            statsTitle.text = "Swinging Weapon";

            //add to stats description
            SwingingWeapon sw = (SwingingWeapon)i.itemStats;
            stats += "Width: " + sw.width.ToString() + "\n";

            
        }

        //Armor
        if (i.itemStats is Armor)
        {
            //set title
            statsTitle.text = "Armor";

            //add to stats description
            Armor armor = (Armor)i.itemStats;
            stats += "Type: " + armor.type.ToString() + "\n";
            stats += "Strong against: " + armor.strongAgainst.ToString() + "\n";
            stats += "Flat Damage Block: " + Math.Round(armor.flatDamageBlock, 2)  + "\n";
            stats += "Percent Damage Block: " + Math.Round(armor.percentDamageBlock, 2) + "\n";
        }


        //set stats description
        statsText.text = stats;
    }

}

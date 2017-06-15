using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used to control the stats panel to
/// display stats and description
/// </summary>
public class UIDescriptionPanel : MonoBehaviour {

    public Text titleText;   //place to put name or title
    public Text statsText;  //place to put stats panel
    public Text descriptionText;    //place to put the description
    public Image image; //place to put icon


    /// <summary>
    /// Displace information associated with item
    /// </summary>
    /// <param name="i">Bag Item</param>
    public void DisplayBagItemInfo(UIBagItem i)
    {
        //title
        titleText.text = i.item.displayName;
        image.sprite = i.item.sprite;

        //description
        descriptionText.text = i.item.description;

        //stats
        string stats = "";

        //general stats from ItemStats
        stats += "Tier: " + i.itemStats.tier.ToString() + "\n";
        stats += "Condition: " + i.itemStats.condition.ToString() + "\n";

            //specific stats from Item
        if (i.itemStats is Weapon)
        {
            Weapon weapon = (Weapon)i.itemStats;
            stats += "Damage Type: " + weapon.damageType.ToString();

        }

        
    }

}

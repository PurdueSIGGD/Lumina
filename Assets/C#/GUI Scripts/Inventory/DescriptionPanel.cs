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
        stats += "Condition: " + i.itemStats.condition.ToString() + "/" + i.itemStats.maxCondition.ToString() + "\n";

        //specific stats from Item
        if (i.itemStats is Weapon)
        {
            Weapon weapon = (Weapon)i.itemStats;
            stats += "Damage Type: " + weapon.damageType.ToString() + "\n";
            stats += "Cool down: "   + weapon.timeToCooldown.ToString() + "s" + "\n";
            stats += "Range: "       + weapon.range.ToString() + "\n";
            stats += "Base Damage: " + weapon.baseDamage.ToString() + "\n";
        }

        //magic inherits weapon
        if (i.itemStats is Magic)
        {
            Magic magic = (Magic)i.itemStats;
            stats += "Width: " + magic.width + "\n";

            //set title
            statsTitle.text = "Magic";
        }

        //projectile weapon inherit weapon
        if (i.itemStats is ProjectileWeapon)
        {
            ProjectileWeapon pw = (ProjectileWeapon)i.itemStats;
            stats += "LaunchSpeed: " + pw.launchSpeed;

            //set title
            statsTitle.text = "Projectile Weapon";
        }

        //swingingWeapon inherit weapon
        if (i.itemStats is SwingingWeapon)
        {
            SwingingWeapon sw = (SwingingWeapon)i.itemStats;
            stats += "Width: " + sw.width.ToString() + "\n";

            //set title
            statsTitle.text = "Swinging Weapon";
        }

        //set stats description
        statsText.text = stats;
    }

}

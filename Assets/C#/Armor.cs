using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : ItemStats {

	public const float STRENGTH_MODIFIER = 0.05F;

    /* I (Andrew G) changed this from "armorType" to "ArmorPiece" because it
	 * better describes what it was meant to show.
	 */
    public override string getBlurb() {
        return "Type: " + strongAgainst + ", Blocks: " + Math.Round(flatDamageBlock, 2) + " + " + Math.Round(percentDamageBlock, 2) + "%";
    }
    public enum ArmorPiece {helmet, chestplate};
	public ArmorPiece type;
	public Hittable.DamageType strongAgainst; //TODO: Default to Hittable.DamageType.Neutral
	public float flatDamageBlock; //TODO: Default 0
	public float percentDamageBlock; //TODO: Default 0
}

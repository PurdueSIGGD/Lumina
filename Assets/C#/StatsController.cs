using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StatsController : Hittable
{
	const float DEFAULT_MAX_HEALTH = 100.0F;
	const float DEFAULT_MAX_MAGIC = 100.0F;
	const float DEFAULT_MAX_LIGHT = 100.0F;
	const float HEALTH_INCREASE_AMOUNT = 10.0F;
	const float MAGIC_INCREASE_AMOUNT = 10.0F;
	const float LIGHT_INCREASE_AMOUNT = 10.0F;
	const float WEAKNESS_MODIFIER = 1.1F;
	const float STRENGTH_MODIFIER = 0.9F;

	float health;
	float healthMax;
	float magic;
	float magicMax;
	float lightt;
	float lighttMax;
	Hittable.DamageType weakAgainst;
	Hittable.DamageType strongAgainst;

	bool outside;

	public InventoryController iC;
	public HUDController gui;

	// Use this for initialization
	void Start ()
	{
		outside = true;
		health = 0;
		healthMax = DEFAULT_MAX_HEALTH;
		magic = 0;
		magicMax = DEFAULT_MAX_MAGIC;
		lightt = 0;
		lighttMax = DEFAULT_MAX_LIGHT;
	}

	public override void Hit(float damage, Vector3 Direction, DamageType type) {
		damage = ApplyDamageTypeHitMod (damage, type);
		damage = ApplyArmorHitMod (damage, type);
		float leftover = UpdateHealth(-1 * damage);
		gui.GUIsetHealth (health);
		if (leftover < 0) {
			Kill ();
		}
	}

	public float GetHealth(){
		return health;
	}

	public float GetHealthMax(){
		return healthMax;
	}

	public float GetMagic(){
		return magic;
	}

	public float GetMagicMax(){
		return magicMax;
	}

	public float GetLightt(){
		return lightt;
	}

	public float GetLighttMax(){
		return lighttMax;
	}

	public bool GetOutside(){
		return outside;
	}

	public void UpgradeMaxHealth(){
		this.healthMax += HEALTH_INCREASE_AMOUNT;
		gui.GUIsetUpgradeHealth (healthMax);
	}

	public void UpgradeMaxMagic(){
		this.magicMax += MAGIC_INCREASE_AMOUNT;
		gui.GUIsetUpgradeMagic (magicMax);
	}

	public void UpgradeMaxLightt(){
		this.lighttMax += LIGHT_INCREASE_AMOUNT;
		gui.GUIsetUpgradeLight (lighttMax);
	}

	//Returns leftover (if any) ((can be negative))
	public float UpdateHealth(float amount) {
		if (health < 0) {
			health = 0;
		}
		if (amount > 0) {
			if (health + amount > healthMax) {
				float leftover = health + amount - healthMax;
				health = healthMax;
				gui.GUIsetHealth (health);
				return leftover;
			}
			health += amount;
			gui.GUIsetHealth (health);
			return 0;
		} else if (amount < 0) {
			if (health + amount < 0) {
				float leftover = health + amount;
				health = 0;
				gui.GUIsetHealth (health);
				gui.GUIsetHealth (health);
				return leftover;
			}
			health += amount;
			gui.GUIsetHealth (health);
			return 0;
		}
		gui.GUIsetHealth (health);
		return 0;
	}

	//Returns leftovers (if any) ((can be negative))
	public float UpdateMagic(float amount) {
		if (magic < 0) {
			magic = 0;
		}
		if (amount > 0) {
			if (magic + amount > magicMax) {
				float leftover = magic + amount - magicMax;
				magic = magicMax;
				return leftover;
			}
			magic += amount;
			return 0;
		} else if (amount < 0) {
			if (magic + amount < 0) {
				float leftover = magic + amount;
				magic = 0;
				return leftover;
			}
			magic += amount;
			return 0;
		}
		return 0;
	}

	//Returns leftovers (if any) ((can be negative))
	public float UpdateLightt(float amount) {
		if (lightt < 0) {
			lightt = 0;
		}
		if (amount > 0) {
			if (lightt + amount > lighttMax) {
				float leftover = lightt + amount - lighttMax;
				lightt = lighttMax;
				return leftover;
			}
			lightt += amount;
			return 0;
		} else if (amount < 0) {
			if (lightt + amount < 0) {
				float leftover = lightt + amount;
				lightt = 0;
				return leftover;
			}
			lightt += amount;
			return 0;
		}
		return 0;
	}

	/* Applies a change in damage from the vulnerability of the creature
	 * to a certain DamageType and returns the damage after modification.
	 * 
	 * @param  damage - the amount of damage before modification
	 * @param  type - The type of damage being dealt
	 * 
	 * @return The amount to modify damage being dealt by (damage is multiplied by this)
	 */
	private float ApplyDamageTypeHitMod(float damage, DamageType type) {
		if (type == Hittable.DamageType.Neutral)
			return damage;
		if (type == weakAgainst) {
			damage = damage * WEAKNESS_MODIFIER;
		} else if (type == strongAgainst) {
			damage = damage * STRENGTH_MODIFIER;
		}
		return damage;
	}

	/* Applies a reduction of damage from a hit based on the creature's armor, and returns
	 * the damage after modification
	 * 
	 * @param  damage - the amount of damage before modification
	 * @param  type - The type of damage being dealt
	 * 
	 * @return The damage to deal after modification
	 */
	private float ApplyArmorHitMod(float damage, DamageType type) {
		List<Armor> armor = iC.GetEquippedArmor ();

		foreach (Armor amr in armor) {
			if (type == amr.strongAgainst) {
				
			}
		}

		float flatDamageReduction = 0;
		float percentDamageReduction = 0;
		foreach (Armor amr in armor) {
			flatDamageReduction += amr.flatDamageBlock;
			percentDamageReduction += amr.percentDamageBlock;
		}
		float dmgRedFromPercent = damage * (0.01F * percentDamageReduction);
		damage -= flatDamageReduction;
		damage -= dmgRedFromPercent;
		if (damage < 0)
			damage = 0;
		return damage;
	}

	/* Kills the entity.
	 */
	public void Kill () {
		//TODO: PUT DEATH CODE HERE
	}
}


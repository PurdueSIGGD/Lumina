using UnityEngine;
using System.Collections;

public class StatsController : Hittable
{
	const float DEFAULT_MAX_HEALTH = 100.0F;
	const float DEFAULT_MAX_MAGIC = 100.0F;
	const float DEFAULT_MAX_LIGHT = 100.0F;
	const float HEALTH_INCREASE_AMOUNT = 10.0F;
	const float MAGIC_INCREASE_AMOUNT = 10.0F;
	const float LIGHT_INCREASE_AMOUNT = 10.0F;

	float health;
	float healthMax;
	float magic;
	float magicMax;
	float lightt;
	float lighttMax;

	bool outside;

	public InventoryController iC;

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
		float typeModifier = getDamageTypeModifier (type);
		float armorModifier = 
	}

	public float getHealth(){
		return health;
	}

	public float getHealthMax(){
		return healthMax;
	}

	public float getMagic(){
		return magic;
	}

	public float getMagicMax(){
		return magicMax;
	}

	public float getlightt(){
		return lightt;
	}

	public float getLighttMax(){
		return lighttMax;
	}

	public bool getOutside(){
		return outside;
	}

	public void upgradeMaxHealth(){
		this.healthMax += HEALTH_INCREASE_AMOUNT;
	}

	public void upgradeMaxMagic(){
		this.magicMax += MAGIC_INCREASE_AMOUNT;
	}

	public void upgradeMaxLightt(){
		this.lighttMax += LIGHT_INCREASE_AMOUNT;
	}

	//Returns leftover (if any) ((can be negative))
	public float updateHealth(float amount, Hittable.DamageType type) {
		if (health < 0) {
			health = 0;
		}
		if (amount > 0) {
			if (health + amount > healthMax) {
				float leftover = health + amount - healthMax;
				health = healthMax;
				return leftover;
			}
			health += amount;
			return 0;
		} else if (amount < 0) {
			if (health + amount < 0) {
				float leftover = health + amount;
				health = 0;
				return leftover;
			}
			health += amount;
			return 0;
		}
		return 0;
	}

	//Returns leftovers (if any) ((can be negative))
	public float updateMagic(float amount) {
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
	public float updateLightt(float amount) {
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

	public float getDamageTypeModifier(DamageType type) {
		//TODO: Implement this.
		return 1;
	}
}


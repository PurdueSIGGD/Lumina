using UnityEngine;
using System.Collections;

public class StatsController : MonoBehaviour
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
	float light;
	float lightMax;

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
		light = 0;
		lightMax = DEFAULT_MAX_LIGHT;
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

	public float getlight(){
		return light;
	}

	public float getLightMax(){
		return lightMax;
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

	public void upgradeMaxLight(){
		this.lightMax += LIGHT_INCREASE_AMOUNT;
	}

	//Returns leftover (if any) ((can be negative))
	public float updateHealth(float amount, DamageType type) {
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
	public float updateLight(float amount) {
		if (light < 0) {
			light = 0;
		}
		if (amount > 0) {
			if (light + amount > lightMax) {
				float leftover = light + amount - lightMax;
				light = lightMax;
				return leftover;
			}
			light += amount;
			return 0;
		} else if (amount < 0) {
			if (light + amount < 0) {
				float leftover = light + amount;
				light = 0;
				return leftover;
			}
			light += amount;
			return 0;
		}
		return 0;
	}
}


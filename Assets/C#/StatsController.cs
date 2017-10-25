using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class StatsController : Hittable
{
    public enum StatType { Health, Magic, Light }

	const float DEFAULT_MAX_HEALTH = 100.0F;
	const float DEFAULT_MAX_MAGIC = 100.0F;
	const float DEFAULT_MAX_LIGHT = 100.0F;
	const float HEALTH_INCREASE_AMOUNT = 10.0F;
	const float MAGIC_INCREASE_AMOUNT = 10.0F;
	const float LIGHT_INCREASE_AMOUNT = 10.0F;
	const float WEAKNESS_MODIFIER = 1.1F;
	const float STRENGTH_MODIFIER = 0.9F;

    const float LIGHT_LOSS_RATE = 1.0f;


	public float health;
    public float healthMax;
    public float magic;
    public float magicMax;
    public float lightt;
    public float lighttMax;
    public int arrowCount;
    public int arrowMax;
    Hittable.DamageType weakAgainst;
	Hittable.DamageType strongAgainst;

    public Animator myAnim, hurtAnim;
    public Light healthLight;
    public InventoryController inventoryController;
    public GameOverCanvas gameOverCanvas;

    private float healthLightStartIntensity;
    private float lightDamageCooldown;
	bool outside;
    public bool dead;
    public AudioClip[] dungeonFootsteps;
    public AudioClip[] outdoorFootsteps;
    public RandomAudioSource footstepSource;
	//public PauseMenu pauseMenu;

    public HUDController gui { get; set; } 

    // Use this for initialization
    void Start ()
	{

        //set condition for health
        outside = true;
        lightDamageCooldown = Time.time;

        //set GUI
        gui = GetComponent<InputGenerator>().uiController.hudController;
        gui.statsController = this;

        healthLightStartIntensity = healthLight.intensity;

        // We may want to change these to the prefab specifically
        /*health = 0;
		healthMax = DEFAULT_MAX_HEALTH;
		magic = 0;
		magicMax = DEFAULT_MAX_MAGIC;
		lightt = 0;
		lighttMax = DEFAULT_MAX_LIGHT;*/
    }
    void DungeonFeet() {
        footstepSource.clips = dungeonFootsteps;
        print("dungeon feet");
    }
    void WorldFeet() {
        footstepSource.clips = outdoorFootsteps;
        print("world feet");

    }

    void Update() {
        // if in a dungeon
        if (dead) {
            // flicker the light till it goes out
            UpdateLightt(LIGHT_LOSS_RATE * -40 * Time.deltaTime);
        } else if (SceneManager.GetActiveScene().name == "Dungeon") {
            // In a dungeon, slowly lower light
            UpdateLightt(Time.deltaTime * -1 * LIGHT_LOSS_RATE);
            if (!healthLight.enabled) {
                healthLight.enabled = true;
            }
            if (GetLightt() <= 0) {
                if (Time.time - lightDamageCooldown > .5f) {
                    lightDamageCooldown = Time.time;
                    Hit(5, DamageType.Umbra);
                } else {
                    //lightDamageCooldown += Time.deltaTime;
                }
            }
        } else if (SceneManager.GetActiveScene().name == "Island3") {
            // Regenerate slower, show light
            UpdateLightt(Time.deltaTime * 7 * LIGHT_LOSS_RATE);
            if (!healthLight.enabled) {
                healthLight.enabled = true;
            }
        } else { 
            // Outside or somewhere else, turn light off
            UpdateLightt(Time.deltaTime * 10 * LIGHT_LOSS_RATE);
            if (healthLight.enabled) {
                healthLight.enabled = false;
            }
        }
        healthLight.intensity = (lightt / lighttMax) * healthLightStartIntensity;

    }

    public override void Hit(float damage, Vector3 Direction, DamageType type) {
        // Setting the player's feedback for getting hit
        myAnim.SetLayerWeight(6, Random.Range(0f, 1f));
        float left = Random.Range(-.5f, .5f);
        if (left > 0) {
            myAnim.SetLayerWeight(7, left);
            myAnim.SetLayerWeight(8, 0);
        } else {
            myAnim.SetLayerWeight(7, 0);
            myAnim.SetLayerWeight(8, Mathf.Abs(left));
        }
        myAnim.SetTrigger("Damage");
        if (!dead) {
            hurtAnim.SetTrigger("Hurt");
        }
		damage = ApplyDamageTypeHitMod (damage, type);
        // Umbra (shadow) does not have any armor hit modification
		if (type != DamageType.Umbra) damage = ApplyArmorHitMod (damage, type);

		float leftover = UpdateHealth(-1 * damage);
        if (GetHealth() <= 0 && !dead) {
            Kill();
        }

        gui.GUIsetHealth (health);
		
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
        UpdateHealth(HEALTH_INCREASE_AMOUNT);
	}
    public void SetMaxHealth(float f)
    {
        this.healthMax = f;
        gui.GUIsetUpgradeHealth(healthMax);
    }
    public void SetMaxMagic(float f)
    {
        this.magicMax = f;
        gui.GUIsetUpgradeMagic(magicMax);
    }
    public void SetMaxLight(float f) {
        this.lighttMax = f;
        gui.GUIsetUpgradeLight(lighttMax);
    }

	public void UpgradeMaxMagic() {
        this.magicMax += MAGIC_INCREASE_AMOUNT;
        gui.GUIsetUpgradeMagic (magicMax);
        UpdateMagic(MAGIC_INCREASE_AMOUNT);
    }

	public void UpgradeMaxLightt() {
        this.lighttMax += LIGHT_INCREASE_AMOUNT;
        gui.GUIsetUpgradeLight (lighttMax);
        UpdateLightt(LIGHT_INCREASE_AMOUNT);
	}

    public void SetHealth(float amount)
    {
        health = amount;
        gui.GUIsetHealth(health);
    }

    public void SetMagic(float amount)
    {
        magic = amount;
        gui.GUIsetMagic(magic);
    }
    public void SetLight(float amount) {
        lightt = amount;
        gui.GUIsetLight(lightt);
    }
    public void SetArrows(int amount) {
        arrowCount = amount;
        UpdateArrowsUI();
    }

	//Returns leftover (if any) ((can be negative))
	public float UpdateHealth(float amount) {
        float returnVal = 0;
        health += amount;
        if (health > healthMax) {
            returnVal = health - healthMax;
            health = healthMax;
        } else if (health < 0) {
            returnVal = health;
            health = 0;
        }
        gui.GUIsetHealth(health);
        return returnVal;
	}

	//Returns leftovers (if any) ((can be negative))
	public float UpdateMagic(float amount) {
        float returnVal = 0;
        magic += amount;
        if (magic > magicMax) {
            returnVal = magic - magicMax;
            magic = magicMax;
        } else if (magic < 0) {
            returnVal = magic;
            magic = 0;
        }
        gui.GUIsetMagic(magic);
        return returnVal;
    }
    public int UpdateArrows(int amount) {

        int returnVal = 0;
        arrowCount += amount;
        if (arrowCount > arrowMax) {
            returnVal = arrowCount - arrowMax;
            arrowCount = arrowMax;
        } else if (arrowCount < 0) {
            returnVal = arrowCount;
            arrowCount = 0;
        }
        UpdateArrowsUI();
        return returnVal;
    }

    /*
     * Update the UI part of display arrows
     */ 
    public void UpdateArrowsUI()
    {
        UIController uiController = GetComponent<InputGenerator>().uiController;

        uiController.inventoryCanvas.avatarPanel.UpdateArrowCount();
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
                gui.GUIsetLight(lightt);
                return leftover;
			}
			lightt += amount;
            gui.GUIsetLight(lightt);
            return 0;
		} else if (amount < 0) {
			if (lightt + amount < 0) {
				float leftover = lightt + amount;
				lightt = 0;
                gui.GUIsetLight(lightt);
                return leftover;
			}
			lightt += amount;
            gui.GUIsetLight(lightt);
            return 0;
		}
        gui.GUIsetLight(lightt);
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
		List<Armor> armor = inventoryController.GetEquippedArmor ();

        //fix null ref
        if (armor == null || armor.Count == 0)
            return damage;

        if (armor.Count > 0) {
            foreach (Armor amr in armor) {
                if (type == amr.strongAgainst) {

                }
            }
        }
		

		float flatDamageReduction = 0;
		float percentDamageReduction = 0;
		foreach (Armor amr in armor) {
            amr.DamageCondition(damage * .1f);
			//flatDamageReduction += amr.flatDamageBlock;
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
        dead = true;
        gameOverCanvas.gameObject.SetActive(true);
        gameOverCanvas.GameOver();
        this.BroadcastMessage("Death");
	}
    public void UnKill() {
        dead = false;
        gameOverCanvas.SendMessage("NotGameOver");
        this.BroadcastMessage("NotDeath");
    }
}


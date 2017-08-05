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

    public Animator myAnim;
    public Light healthLight;
    private float healthLightStartIntensity;

	bool outside;
    bool dead;

	[HideInInspector] public InventoryController inventoryController;	
	//public PauseMenu pauseMenu;

    public HUDController gui { get; set; } 

    // Use this for initialization
    void Start ()
	{
        //assign var
        inventoryController = GetComponent<InventoryController>();

        //set condition for health
		outside = true;


        //set GUI
        gui = GetComponent<InputGenerator>().uiController.hudController;
        gui.statsController = this;

        healthLightStartIntensity = healthLight.intensity;
        DontDestroyOnLoad(gui.gameObject);
        //DontDestroyOnLoad(pauseMenu);

        // We may want to change these to the prefab specifically
        /*health = 0;
		healthMax = DEFAULT_MAX_HEALTH;
		magic = 0;
		magicMax = DEFAULT_MAX_MAGIC;
		lightt = 0;
		lighttMax = DEFAULT_MAX_LIGHT;*/
    }

    void Update() {
        // if in a dungeon
        if (dead) {
            // flicker the light till it goes out
            UpdateLightt(LIGHT_LOSS_RATE * -20 * Time.deltaTime);
        } else if (SceneManager.GetActiveScene().name == "Dungeon") {
            // In a dungeon, slowly lower light
            UpdateLightt(Time.deltaTime * -1 * LIGHT_LOSS_RATE);
            if (!healthLight.enabled) {
                healthLight.enabled = true;
            }
            if (GetLightt() <= 0) {
                Kill();
            }
        } else {
            // Outside or somewhere else, turn light off
            UpdateLightt(Time.deltaTime * 5 * LIGHT_LOSS_RATE);
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

		damage = ApplyDamageTypeHitMod (damage, type);
		damage = ApplyArmorHitMod (damage, type);

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
		if (health <= 0) {
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
				gui.GUIsetMagic (magic);
				return leftover;
			}
			magic += amount;
			gui.GUIsetMagic (magic);
			return 0;
		} else if (amount < 0) {
			if (magic + amount < 0) {
				float leftover = magic + amount;
				magic = 0;
				gui.GUIsetMagic (magic);
				return leftover;
			}
			magic += amount;
			gui.GUIsetMagic (magic);
			return 0;
		}
		gui.GUIsetMagic (magic);
		return 0;
	}
    public int UpdateArrows(int amount) {
        if (arrowCount < 0) {
            arrowCount = 0;
        }
        if (amount > 0) {
            if (arrowCount + amount > arrowMax) {
                int leftover = arrowCount + amount - arrowMax;
                arrowCount = arrowMax;

                UpdateArrowsUI();
                return leftover;
            }
            arrowCount += amount;

            UpdateArrowsUI();
            return 0;
        }

        else if (amount < 0) {
            if (arrowCount + amount < 0) {
                int leftover = arrowCount + amount;
                arrowCount = 0;

                UpdateArrowsUI();
                return leftover;
            }

            arrowCount += amount;
            UpdateArrowsUI();
            return 0;
        }

        UpdateArrowsUI();
        return 0;
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
        dead = true;
        this.BroadcastMessage("Death");
	}
    public void UnKill() {
        dead = false;
        this.BroadcastMessage("NotDeath");
    }
}


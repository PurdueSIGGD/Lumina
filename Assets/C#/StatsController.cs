using UnityEngine;
using System.Collections;

public class StatsController : MonoBehaviour
{
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
	}
	
	// Update is called once per frame
	void Update ()
	{
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

	public void upgradeMaxHealth(){
	}

	public void upgradeMaxMagic(){
	}

	public void upgradeMaxLight(){
	}
}


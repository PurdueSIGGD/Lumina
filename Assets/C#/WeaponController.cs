using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

    Weapon[] weapon = new Weapon[2];
    int weaponIndex;
	//TODO: StatsController


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Attack() {
        //Play animation
        //Hit detection
        //Deal damage if necessary
    }

    public void SwitchWeapon() {
        if (weaponIndex == 1) {
            weaponIndex = 0;
        } else {
            weaponIndex = 1;
        }
    }

    public void SetWeapon(Weapon newWeapon) {
        weapon[weaponIndex] = newWeapon;
    }

    public Weapon getCurrentWeapon() {
		return weapon [weaponIndex];
    }


}
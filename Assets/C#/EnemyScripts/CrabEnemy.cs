using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabEnemy : PatrolGroundEnemy {

    public const string ATTACK = "Attack";
    public const string DEATH = "Death";
    public const string DAMAGE = "Damage";
    public const string HORIZONTAL_SPEED = "HorizSpeed";
    public const string ABS_HORIZONTAL_SPEED = "Abs(HorizSpeed)";
    public const string FORWARD_SPEED = "ForwardSpeed";
    public const string ABS_FORWARD_SPEED = "Abs(ForwardSpeed)";



    public override IEnumerator Attack()
    {
        return null;
    }

    public override void Movement()
    {
        
    }

    public override void OnDamage(float damage, DamageType type)
    {
        
    }

    public override void OnDeath()
    {
        
    }

    // Use this for initialization
    void Start () {
		
	}
	
	
}

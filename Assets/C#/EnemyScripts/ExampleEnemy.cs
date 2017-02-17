using UnityEngine;
using System.Collections;

public class ExampleEnemy : BaseEnemy {

	void Start(){
		health = 5;
	}

	public override void Attack(){
		//If within range
		//Player.TakeDamage(9001);
	}

	public override void Movement(){
		Debug.Log ("MOVING");
		//Movement style of the particular enemy
	}
}

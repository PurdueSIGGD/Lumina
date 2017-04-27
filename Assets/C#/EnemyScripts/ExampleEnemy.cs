using UnityEngine;
using System.Collections;

public class ExampleEnemy : BaseEnemy {

	void Start(){
		health = 5;
	}

	public override IEnumerator Attack(){
		//If within range
		yield return new WaitForSeconds(1);
		//Player.TakeDamage(9001);
	}

	public override void Movement(){
		Debug.Log ("MOVING");
		//Movement style of the particular enemy
	}
    public override void OnDeath() {
        // IDK do whatever
    }
    public override void OnDamage(float damage) {

    }
}

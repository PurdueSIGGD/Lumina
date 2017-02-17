using UnityEngine;
using System.Collections;

abstract public class BaseEnemy : MonoBehaviour {

	public GameObject [] drops; 	//Array of possible drops for this enemy

	public int minDrops;			//Mininum number of possible drops
	public int maxDrops;			//Maximum number of possible drops
	public float health;			//Enemy health
	public float movementSpeed;		//Enemy movement speed

	/*
	 * Method called when enemy dies
	 * Identifies the number of drops the enemy drops based on the minimum and maximum values
	 * Then, for each of the individual drops, find a random GameObject in the drops array to drop
	 * Instantiate that drop
	 */
	void OnDeath(){
		int numberOfDrops = Mathf.RoundToInt (Random.Range (minDrops, maxDrops));

		for (int i = 0; i < numberOfDrops; i++) {
			int dropIndex = Mathf.RoundToInt (Random.Range (0, drops.Length));
			Instantiate (drops [dropIndex], transform.position, Quaternion.identity);
		}
	}

	/*
	 * The damage done to the enemies health based on the parameter defined damage
	 */
	public void TakeDamage (float dmg){
		health -= dmg;
	}

	/*
	 * Each frame, call enemies movement and attack methods
	 * Also check if health is below or equal to zero and call OnDeath if true
	 * note: has to be public because it needs to be in order for the sub-class to use it
	 */
	public void Update () {
		Movement ();
		Attack();
		if (health <= 0) {
			OnDeath ();
		}
	}

	abstract public void Attack();		//Abstract method that checks if the player is within range and then damages player
	abstract public void Movement();	//Abstract method defining how the specific enemy moves 
}

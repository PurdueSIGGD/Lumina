using UnityEngine;
using System.Collections;

abstract public class BaseEnemy : Hittable {

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
    void GenericDeath(){

        OnDeath();

        // Prevent from moving
        //TODO ragdoll
        this.GetComponent<Rigidbody>().freezeRotation = true;


        int numberOfDrops = Mathf.RoundToInt (Random.Range (minDrops, maxDrops));

		for (int i = 0; i < numberOfDrops; i++) {
			int dropIndex = Mathf.RoundToInt (Random.Range (0, drops.Length));
			Instantiate (drops [dropIndex], transform.position, Quaternion.identity);
		}
	}

	/*
	 * The damage done to the enemies health based on the parameter defined damage
	 */
	public override void Hit(float damage, Vector3 direction, DamageType type) {
        //print("enemy hit");
        // TODO add enemy vulnerabilities for damage types
        float originalHealth = health;
        if (health - damage > 0) {
            health -= damage;
        } else {
            health = 0;
        }
        
        if (health <= 0 && originalHealth > 0) {
            GenericDeath();
        } else {
            OnDamage(damage, type);
        }
	}

	/*
	 * Each frame, call enemies movement method if it's not attacking and attack method if it is
	 * Also check if health is below or equal to zero and call OnDeath if true
	 * note: has to be public because it needs to be in order for the sub-class to use it
	 */
	public void Update () {
		if (health > 0) Movement ();
        else {

        }
	}

   

	abstract public IEnumerator Attack();		//Abstract method that checks if the player is within range and then damages player
	abstract public void Movement();			//Abstract method defining how the specific enemy moves 
    abstract public void OnDeath();             //What to do when the enemy dies
    abstract public void OnDamage(float damage, DamageType type);            //What do to when the enemy takes damage
}

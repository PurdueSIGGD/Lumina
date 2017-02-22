using UnityEngine;
using System.Collections;

public class SlimeEnemy : BaseEnemy {

	public Transform target; 	//The player, for the slime to face and move toward
	public int thrust;			//The amount of force used to propel the slime forward

	private Rigidbody rb;		//The slimes rigidbody which allows us to propel it

	//Variable initialization 
	void Start(){
		health = 5;
		movementSpeed = 1;
		thrust = 500;
		rb = GetComponent<Rigidbody> ();
	}
		
	/* The attack method is an IEnumarator to allow the slime to wait before executing the attack
	 * The slime will then propel itself towards the player
	 * It will wait a couple of seconds before moving again
	 */
	public override IEnumerator Attack(){

		yield return new WaitForSeconds(1); //Wait a single second before attack
		rb.AddForce(transform.forward*thrust);

		yield return new WaitForSeconds(2); //Time waited to move again
		isAttacking = false; //This will start movement again in the Update method
	}

	/* This is a simple movement method, 
	 * it looks in the players direction and moves that way constantly
	 */
	public override void Movement(){
		if (target != null) {
			transform.LookAt (target);
		}
		transform.position = transform.position + (transform.forward * Time.deltaTime * movementSpeed);
	}

	/* This access the slime's Sphere Collider where there is a trigger 
	 * The trigger is the range of the slime's attack and will call the Attack coroutine 
	 * when triggered
	 */
	public void OnTriggerEnter(Collider col){
		if (col.tag == target.tag) {
			isAttacking = true;
			StartCoroutine (Attack ());
		}
	}
}

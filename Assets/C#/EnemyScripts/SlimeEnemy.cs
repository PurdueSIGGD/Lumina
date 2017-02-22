using UnityEngine;
using System.Collections;

public class SlimeEnemy : BaseEnemy {

	public Transform target; 				//The player, for the slime to face and move toward
	public int thrust;						//The amount of force used to propel the slime forward
	public bool isAttacking;				//If the enemy is currently attacking, return true

	private Vector3 startPos;				//The slimes starting position
	private Rigidbody rb;					//The slimes rigidbody which allows us to propel it
	private float changeDirectionCount = 0;	//The time counter for the slime to change direction while the player isn't around

	//Variable initialization 
	void Start(){
		health = 5;
		movementSpeed = 1;
		thrust = 500;

		startPos = transform.position;
		rb = GetComponent<Rigidbody> ();
		rb.constraints = RigidbodyConstraints.FreezeRotation;
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
			if (!isAttacking) {
				transform.LookAt (target);
				transform.position = transform.position + (transform.forward * Time.deltaTime * movementSpeed);
			} 
		}

		if (target == null) {
			changeDirectionCount += Time.deltaTime;
			transform.position = transform.position + (transform.forward * Time.deltaTime * movementSpeed);
			if (changeDirectionCount > 4f) {
				transform.rotation = Quaternion.Euler(0, Random.Range (15, 360), 0);
				changeDirectionCount = 0;
				Debug.Log ("CHANGE");
			}
		}

	}

	/* This access the slime's Sphere Collider where there is a trigger 
	 * The trigger is the range of the slime's attack and will call the Attack coroutine 
	 * when triggered
	 */
	public void OnTriggerEnter(Collider col){
		if (target != null) {
			if (col.tag == target.tag && !isAttacking) {
				isAttacking = true;
				StartCoroutine (Attack ());
			}
		}
	}
}

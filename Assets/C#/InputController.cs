using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

	Rigidbody playerPhysics;
	BoxCollider playerCollider;

	// Use this for initialization
	void Start () {
		playerPhysics = GetComponentInParent<Rigidbody> ();
		playerCollider = GetComponentInParent<BoxCollider> ();
	}
	
	// Update is called once per frame
	void Update () {
		SetMovement ();
	}

	/*
	* Setting the movement and applying the force to the player
	*/
	void SetMovement(){
		playerPhysics.AddForce(new Vector3(Input.GetAxis ("Horizontal")*10,0,0));
		playerPhysics.AddForce(new Vector3(0,0,Input.GetAxis ("Vertical")*20));
		if (!IsAirborne()) {
			playerPhysics.AddForce (new Vector3 (0, Input.GetAxis ("Jump") * 20, 0));
		}
	}

	bool IsAirborne(){
		RaycastHit[] hits = Physics.BoxCastAll(this.transform.position-(Vector3.down*playerCollider.bounds.extents.y), playerCollider.bounds.extents, Vector3.down);
		bool hitValid = false;
		foreach (RaycastHit hit in hits) {
			Collider col = hit.collider;
			print ("Collider is: " + col);
			if(!col.CompareTag("Player")) {
				print ("Collider was not player");
				hitValid = true;
				break;
			}
		}
		if (hitValid) {
			return false;
		}
		return true;
	}
}

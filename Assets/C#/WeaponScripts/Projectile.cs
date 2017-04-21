using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
	public Hittable.DamageType damageType;
	public float damage;
    private Rigidbody myRigid;
    public bool pointsWhenFast;
    void Start() {
        myRigid = this.GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (pointsWhenFast) {
            Vector3 vel = myRigid.velocity;

            if (Vector3.Magnitude(vel) > 10) {
                /*print("Aiming");
                float rotX = Mathf.Atan2(vel.y, vel.z) * Mathf.Rad2Deg; //moving the rotation of the center here
                float rotY = Mathf.Atan2(vel.x, vel.z) * Mathf.Rad2Deg; //moving the rotation of the center here
                float rotZ = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg; //moving the rotation of the center here
                transform.rotation = Quaternion.Euler(rotX, rotY, rotZ);*/
                transform.LookAt(transform.position + vel);
            }
        }
    }
}

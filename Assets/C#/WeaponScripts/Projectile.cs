using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
	public Hittable.DamageType damageType;
	public float damage;
    private Rigidbody myRigid;
    public bool pointsWhenFast;
    public bool sticky;
    public bool destroysOnDeath;
    public bool pushesOnHit;
    public bool doubleCheckStick;
    public Transform creator; //Do not hit creator

    // README: Please set creator when creating this object, or else it will not damage anything

    private bool doubleChecked;

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
    void LateUpdate () {
        if (doubleCheckStick && sticky && !doubleChecked && transform.parent != null) {
            // right now, this assumes you use a box collider
            BoxCollider myBox = this.GetComponent<BoxCollider>();
            print(myBox.size * 2);
            RaycastHit[] hits = Physics.BoxCastAll(myBox.center, myBox.size * 2, Vector3.zero);
            
            bool found = false;
            foreach (RaycastHit hit in hits) {
                if (hit.transform == transform.parent) {
                    found = true;
                }
            }
            if (found) {
                doubleChecked = true;
            } else {
                UnStick();
            }
        }
    }
    void OnCollisionEnter(Collision col) {
        Vector3 velocity = myRigid.velocity;
        if (    // All cases that we want to ignore the collision
            creator == null || // If we spawn it right off the bat, it won't have a creator and collide with the creator, if inside it. We don't want that.
            // Don't hit creator
            col.transform == creator
            ) return;
        //print(col.collider.transform);
        // Push on it, as if it was a real collision
        if (pushesOnHit && col.rigidbody) {
            col.rigidbody.AddForce(myRigid.velocity);
        }
        // Stick
        if (sticky &&
            // We ignore anything equippable since setenabled for some components enables sub components, making our collider become enabled while being held by the player
            !col.transform.GetComponent<ItemStats>()) {
            if (Vector3.Magnitude(velocity) > 10) {
                //myRigid.drag = 10;
                //myRigid.velocity = myRigid.velocity / 10; //slow dooown
                this.transform.parent = col.collider.transform;
                // Disable collider
                this.GetComponent<Collider>().enabled = false;
                myRigid.isKinematic = true;
            } else {
                this.GetComponent<Collider>().isTrigger = false;
            }
        } else {
            this.GetComponent<Collider>().isTrigger = false;
        }
        // Hit for damage
        Hittable h;
        if (h = col.transform.GetComponent<Hittable>()) {
            float immediateDamage = 0;
            float magVelocity = Vector3.Magnitude(col.relativeVelocity);
            
            // 50 is the high speed I think
            if (magVelocity > 50) {
                immediateDamage = damage;
            } else {
                immediateDamage = damage * magVelocity / 50;
            }
            h.Hit(immediateDamage, transform.position, damageType);
        }
        if (destroysOnDeath) {
            Destroy(this.gameObject);
        }
    }
   public void UnStick() {
        // Change parent
        this.transform.parent = null;
        // Disable collider
        this.GetComponent<Collider>().isTrigger = false;
        myRigid.isKinematic = false;
        doubleChecked = false;
    }
    void OnDestroy() {
        /*print("Arrow was attempted to be destroyed, enabling physics and collider");
        // Enable collider
        this.GetComponent<Collider>().isTrigger = false;
        // Start moving
        myRigid.isKinematic = false;*/
    }
}

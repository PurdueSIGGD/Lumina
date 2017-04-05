using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DestructibleItem : Hittable {
    /**
     * This is a class I created to help you guys practice out hitting.
     * If your hit method works, it will destroy items such as this.
     * All it does right now is fall apart into a fractured block.
     * Try it out!
     */
    
    public GameObject gibs;
    public override void Hit(float f, Vector3 direction, Hittable.DamageType damage) {

        //print("was hit");
        this.GetComponent<Collider>().isTrigger = true;
        Rigidbody myRigid = this.GetComponent<Rigidbody>();

        GameObject spawn = (GameObject)GameObject.Instantiate(gibs, transform.position, transform.rotation);
        Vector3 myVelocity = myRigid.velocity;
        
        foreach (Rigidbody r in spawn.GetComponentsInChildren<Rigidbody>()) {
            r.velocity = myRigid.GetPointVelocity(r.transform.position);
        }
        GameObject.Destroy(this.gameObject);

    }
    void Start() {
		
        //this.gameObject.GetComponent<Hittable>().Hit();
    }
}

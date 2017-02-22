using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleItem : Hittable {
    /**
     * This is a class I created to help you guys practice out hitting.
     * If your hit method works, it will destroy items such as this.
     * All it does right now is fall apart into a fractured block.
     * Try it out!
     */
     
    public GameObject gibs; 	
    public override void Hit(float f, Vector3 direction, DamageType damage) {
        print("was hit");
        GameObject.Instantiate(gibs, transform.position, transform.rotation);
        GameObject.Destroy(this.gameObject);
    }
    void Start() {
		
        //this.gameObject.GetComponent<Hittable>().Hit();
    }
}
